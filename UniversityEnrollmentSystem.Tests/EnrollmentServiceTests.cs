using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UniversityEnrollmentSystem.Models.Database;
using UniversityEnrollmentSystem.Repositories.CourseOffering;
using UniversityEnrollmentSystem.Repositories.Enrollment;
using UniversityEnrollmentSystem.Services.Enrollment;
using Xunit;

namespace UniversityEnrollmentSystem.Tests
{
    public class EnrollmentServiceTests
    {
        private readonly Mock<IEnrollmentRepository> _mockEnrollmentRepo;
        private readonly Mock<ICourseOfferingRepository> _mockOfferingRepo;
        private readonly EnrollmentService _service;

        public EnrollmentServiceTests()
        {
            _mockEnrollmentRepo = new Mock<IEnrollmentRepository>();
            _mockOfferingRepo = new Mock<ICourseOfferingRepository>();
            _service = new EnrollmentService(_mockEnrollmentRepo.Object, _mockOfferingRepo.Object);
        }

        [Fact]
        public async Task EnrollStudent_ShouldFail_WhenCourseIsFull()
        {
            var offering = new CourseOffering
            {
                CourseOfferingId = 1,
                Capacity = 1,
                Enrollments = new List<Models.Database.Enrollment> { new Models.Database.Enrollment { StudentId = 2 } }, 
                Semester = new Semester
                {
                    StartDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-10)),
                    EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(10))
                }
            };

            _mockOfferingRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(offering);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.EnrollStudentAsync(1, 1));
            Assert.Equal("Course is full.", ex.Message);
            _mockEnrollmentRepo.Verify(r => r.AddAsync(It.IsAny<Models.Database.Enrollment>()), Times.Never);
        }

        [Fact]
        public async Task EnrollStudent_ShouldFail_WhenAlreadyEnrolled()
        {
            var offering = new CourseOffering
            {
                CourseOfferingId = 1,
                Capacity = 30,
                Enrollments = new List<Models.Database.Enrollment> { new Models.Database.Enrollment { StudentId = 1 } }, 
                Semester = new Semester
                {
                    StartDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-10)),
                    EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(10))
                }
            };

            _mockOfferingRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(offering);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.EnrollStudentAsync(1, 1));
            Assert.Equal("Student is already enrolled in this offering.", ex.Message);
        }

        [Fact]
        public async Task EnrollStudent_ShouldSucceed_WhenValid()
        {
            var offering = new CourseOffering
            {
                CourseOfferingId = 1,
                Capacity = 30,
                Enrollments = new List<Models.Database.Enrollment>(),
                Semester = new Semester
                {
                    StartDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-10)),
                    EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(10))
                }
            };

            _mockOfferingRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(offering);

            var result = await _service.EnrollStudentAsync(1, 1);

            Assert.True(result);
            _mockEnrollmentRepo.Verify(r => r.AddAsync(It.IsAny<Models.Database.Enrollment>()), Times.Once);
        }

        [Fact]
        public async Task EnrollStudent_ShouldFail_WhenSemesterExpired()
        {
            var offering = new CourseOffering
            {
                CourseOfferingId = 1,
                Capacity = 30,
                Enrollments = new List<Models.Database.Enrollment>(),
                Semester = new Semester
                {
                    StartDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-30)),
                    EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1))
                }
            };

            _mockOfferingRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(offering);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.EnrollStudentAsync(1, 1));
            Assert.Equal("Cannot enroll outside of the semester dates.", ex.Message);
        }

        [Fact]
        public async Task AssignGrade_ShouldFail_WhenNotEnrolled()
        {
            _mockEnrollmentRepo.Setup(repo => repo.GetByIdAsync(99)).ReturnsAsync((Models.Database.Enrollment)null);

            var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.AssignGradeAsync(99, 85.5m));
            Assert.Equal("Enrollment not found.", ex.Message);
        }

        [Fact]
        public async Task AssignGrade_ShouldFail_WhenGradeOutOfRange()
        {
            var enrollment = new Models.Database.Enrollment { EnrollmentId = 1 };
            _mockEnrollmentRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(enrollment);

            var ex = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _service.AssignGradeAsync(1, 105m));
            Assert.Contains("Grade must be between 0 and 100.", ex.Message);
        }

        [Fact]
        public async Task AssignGrade_ShouldSucceed_WhenValid()
        {
            var enrollment = new Models.Database.Enrollment { EnrollmentId = 1 };
            _mockEnrollmentRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(enrollment);

            var result = await _service.AssignGradeAsync(1, 95.0m);

            Assert.True(result);
            Assert.Equal(95.0m, enrollment.FinalGrade);
            _mockEnrollmentRepo.Verify(r => r.UpdateAsync(enrollment), Times.Once);
        }

        [Fact]
        public async Task Enrollment_ShouldBeUnique_PerStudentOffering()
        {
            var offering = new CourseOffering
            {
                CourseOfferingId = 1,
                Capacity = 30,
                Enrollments = new List<Enrollment>
        {
            new Enrollment { StudentId = 1 }
        },
                Semester = new Semester
                {
                    StartDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-10)),
                    EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(10))
                }
            };

            _mockOfferingRepo
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(offering);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.EnrollStudentAsync(1, 1));

            Assert.Equal("Student is already enrolled in this offering.", ex.Message);

            _mockEnrollmentRepo.Verify(
                r => r.AddAsync(It.IsAny<Models.Database.Enrollment>()),
                Times.Never);
        }

        [Fact]
        public async Task ForeignKey_ShouldPreventInvalidEnrollment()
        {
            _mockOfferingRepo
                .Setup(repo => repo.GetByIdAsync(999))
                .ReturnsAsync((CourseOffering?)null);

            var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.EnrollStudentAsync(1, 999));

            Assert.Equal("Invalid Course Offering.", ex.Message);

            _mockEnrollmentRepo.Verify(
                r => r.AddAsync(It.IsAny<Models.Database.Enrollment>()),
                Times.Never);
        }
    }
}