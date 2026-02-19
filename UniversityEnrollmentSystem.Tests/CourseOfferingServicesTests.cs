using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityEnrollmentSystem.Models.Database;
using UniversityEnrollmentSystem.Repositories.CourseOffering;
using UniversityEnrollmentSystem.Services;

namespace UniversityEnrollmentSystem.Tests
{
        public class CourseOfferingServiceTests
        {
            private readonly Mock<ICourseOfferingRepository> _mock;
            private readonly ICourseOfferingService _service;

            public CourseOfferingServiceTests()
            {
                _mock = new Mock<ICourseOfferingRepository>();
                _service = new CourseOfferingService(_mock.Object);
            }

            [Fact]
            public async Task CreateOffering_ShouldFail_WhenDuplicateCourseSemester()
            {
                var offering = new CourseOffering { CourseId = 1, SemesterId = 2 };

                _mock.Setup(repo => repo.IsDuplicateOfferingAsync(offering.CourseId, offering.SemesterId))
                                 .ReturnsAsync(true);

                var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateOfferingAsync(offering));
                Assert.Equal("Course offering already exists for this semester.", ex.Message);

                _mock.Verify(repo => repo.AddAsync(It.IsAny<CourseOffering>()), Times.Never);
            }
        [Fact]
        public async Task DeleteCourse_ShouldReturnFalse_WhenEnrollmentsExist()
        {
            int courseId = 1;

            _mock.Setup(repo => repo.ExistsAsync(courseId))
                           .ReturnsAsync(true);
            _mock.Setup(repo => repo.HasEnrollmentsForCourseAsync(courseId))
                            .ReturnsAsync(true);
            var result = await _service.DeleteCourseAsync(courseId);

            Assert.False(result);

            _mock.Verify(repo => repo.DeleteAsync(It.IsAny<int>()), Times.Never); //Fix this error
        }
    }
}
