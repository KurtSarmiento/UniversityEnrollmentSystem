using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityEnrollmentSystem.Models.Database;
using UniversityEnrollmentSystem.Repositories.Course;
using UniversityEnrollmentSystem.Repositories.CourseOffering;

namespace UniversityEnrollmentSystem.Services.Courses
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _Repo;

        public CourseService(ICourseRepository courseRepository) 
        {
            _Repo = courseRepository;
        }

        public async Task<bool> CreateCourseAsync(Course newCourse)
        {
            if (await _Repo.CourseCodeExistsAsync(newCourse.CourseCode))
            {
                throw new InvalidOperationException("Course code already exists for this semester.");
            }

            await _Repo.AddAsync(newCourse);
            return true;
        }
    }
}
