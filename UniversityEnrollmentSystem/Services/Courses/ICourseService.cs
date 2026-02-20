using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityEnrollmentSystem.Models.Database;
using UniversityEnrollmentSystem.Repositories.Course;
using UniversityEnrollmentSystem.Repositories.CourseOffering;

namespace UniversityEnrollmentSystem.Services.Courses
{
    public interface ICourseService
    {
        Task<bool> CreateCourseAsync(Course newCourse);
    }
}
