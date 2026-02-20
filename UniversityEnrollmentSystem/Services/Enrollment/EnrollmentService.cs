using System;
using System.Linq;
using System.Threading.Tasks;
using UniversityEnrollmentSystem.Models.Database;
using UniversityEnrollmentSystem.Repositories.CourseOffering;
using UniversityEnrollmentSystem.Repositories.Enrollment;

namespace UniversityEnrollmentSystem.Services.Enrollment
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _enrollmentRepo;
        private readonly ICourseOfferingRepository _offeringRepo;

        public EnrollmentService(IEnrollmentRepository enrollmentRepo, ICourseOfferingRepository offeringRepo)
        {
            _enrollmentRepo = enrollmentRepo;
            _offeringRepo = offeringRepo;
        }

        public async Task<bool> EnrollStudentAsync(int studentId, int courseOfferingId)
        {
            var offering = await _offeringRepo.GetByIdAsync(courseOfferingId);
            if (offering == null)
                throw new ArgumentException("Invalid Course Offering.");

            var currentDate = DateOnly.FromDateTime(DateTime.Now);
            if (currentDate < offering.Semester.StartDate || currentDate > offering.Semester.EndDate)
                throw new InvalidOperationException("Cannot enroll outside of the semester dates.");

            if (offering.Enrollments != null && offering.Enrollments.Count >= offering.Capacity)
                throw new InvalidOperationException("Course is full.");

            if (offering.Enrollments != null && offering.Enrollments.Any(e => e.StudentId == studentId))
                throw new InvalidOperationException("Student is already enrolled in this offering.");

            var enrollment = new Models.Database.Enrollment
            {
                StudentId = studentId,
                CourseOfferingId = courseOfferingId,
            };

            await _enrollmentRepo.AddAsync(enrollment);

            await _enrollmentRepo.LogHistoryAsync(new EnrollmentHistory
            {
                EnrollmentId = enrollment.EnrollmentId,
                StudentId = studentId,
                CourseOfferingsId = courseOfferingId,
                ActionType = "INSERT",
                ActionDate = DateTime.Now
            });
            return true;
        }

        public async Task<bool> AssignGradeAsync(int enrollmentId, decimal grade)
        {
            var enrollment = await _enrollmentRepo.GetByIdAsync(enrollmentId);
            if (enrollment == null) throw new ArgumentException("Enrollment not found.");
            if (grade < 0 || grade > 100) throw new ArgumentOutOfRangeException(nameof(grade), "Grade must be between 0 and 100.");

            enrollment.FinalGrade = grade;
            await _enrollmentRepo.UpdateAsync(enrollment);

            await _enrollmentRepo.LogHistoryAsync(new EnrollmentHistory
            {
                EnrollmentId = enrollment.EnrollmentId,
                StudentId = enrollment.StudentId,
                CourseOfferingsId = enrollment.CourseOfferingId,
                FinalGrade = grade,
                ActionType = "UPDATE_GRADE",
                ActionDate = DateTime.Now
            });

            return true;
        }

        public async Task<bool> DropCourseAsync(int enrollmentId)
        {
            var enrollment = await _enrollmentRepo.GetByIdAsync(enrollmentId);
            if (enrollment == null) return false;

            await _enrollmentRepo.LogHistoryAsync(new EnrollmentHistory
            {
                EnrollmentId = enrollment.EnrollmentId,
                StudentId = enrollment.StudentId,
                CourseOfferingsId = enrollment.CourseOfferingId,
                ActionType = "DELETE/DROP",
                ActionDate = DateTime.Now
            });

            await _enrollmentRepo.DeleteAsync(enrollmentId);
            return true;
        }
    }
}