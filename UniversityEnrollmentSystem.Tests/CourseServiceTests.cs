using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using UniversityEnrollmentSystem.Models.Database;
using UniversityEnrollmentSystem.Repositories.Course;
using UniversityEnrollmentSystem.Services.Courses;

namespace UniversityEnrollmentSystem.Tests
{
    public class CourseServiceTests
    {
        private readonly Mock<ICourseRepository> _mock;
        private readonly ICourseService _service;

        public CourseServiceTests()
        {
            _mock = new Mock<ICourseRepository>();
            _service = new CourseService(_mock.Object);
        }
        [Fact]
        public async Task CourseCode_ShouldBeUnique()
        {
            var offering = new Course { CourseCode = "ELECT2"};
            _mock.Setup(x => x.CourseCodeExistsAsync(offering.CourseCode))
                             .ReturnsAsync(true);
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateCourseAsync(offering));
            Assert.Equal("Course code already exists for this semester.", ex.Message);
            _mock.Verify(x => x.AddAsync(It.IsAny<Course>()), Times.Never);
        }
    }
}
