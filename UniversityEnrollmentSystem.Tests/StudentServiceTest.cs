using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityEnrollmentSystem.Models.Database;
using UniversityEnrollmentSystem.Repositories.Students;
using UniversityEnrollmentSystem.Services.Students;

namespace UniversityEnrollmentSystem.Tests
{
    public class StudentServiceTest
    {
        private readonly Mock<IStudentRepository> _repo;
        private readonly StudentService _studentService;

        public StudentServiceTest()
        {
            _repo = new Mock<IStudentRepository>();
            _studentService = new StudentService(_repo.Object);
        }
        private static Student CreateValidStudent() => new()
        {
            StudentId = 1,
            StudentNumber = 123456,
            FirstName = "John",
            LastName = "Doe",
            Email = "testmail",
            DateCreated = DateOnly.FromDateTime(DateTime.Now)
        };
        [Fact]
        public async Task AddStudent_ShouldInsertRecord()
        {
            var student = CreateValidStudent();

            _repo.Setup(r => r.AddStudent(student))
                 .Returns(Task.CompletedTask);

            await _studentService.CreateStudent(student);

            _repo.Verify(r => r.AddStudent(student), Times.Once);
        }

        [Fact]
        public async Task StudentNumber_ShouldBeUnique()
        {
            var student = CreateValidStudent();

            _repo.Setup(r => r.GetStudentByStudentNumber(student.StudentNumber))
                 .ReturnsAsync(new Student());

            await Assert.ThrowsAsync<Exception>(() =>
                _studentService.CreateStudent(student)
            );

            _repo.Verify(r => r.AddStudent(It.IsAny<Student>()), Times.Never);
        }
        [Fact]
        public async Task RegisterStudent_ShouldPreventDuplicateStudentNumber()
        {
            var student = CreateValidStudent();

            _repo.Setup(r => r.GetStudentByStudentNumber(student.StudentNumber))
                 .ReturnsAsync(new Student());

            await Assert.ThrowsAsync<Exception>(() =>
                _studentService.CreateStudent(student));

            _repo.Verify(r => r.AddStudent(It.IsAny<Student>()), Times.Never);
        }

        [Fact]
        public async Task RequiredFields_ShouldRejectNull_WhenStudentNameIsNull()
        {
            var student = new Student
            {
                StudentNumber = 2024001,
                FirstName = "", // Required field missing
                LastName = "Sarmiento",
            };

            await Assert.ThrowsAsync<Exception>(() =>
                _studentService.CreateStudent(student));
        }


    }
}
