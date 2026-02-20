using System.Threading.Tasks;

namespace UniversityEnrollmentSystem.Services.Enrollment
{
    public interface IEnrollmentService
    {
        Task<bool> EnrollStudentAsync(int studentId, int courseOfferingId);
        Task<bool> AssignGradeAsync(int enrollmentId, decimal grade);
        Task<bool> DropCourseAsync(int enrollmentId);
    }
}