using Microsoft.EntityFrameworkCore;
using UniversityEnrollmentSystem.Models.Database;

namespace UniversityEnrollmentSystem.Repositories.Course
{
    public class CourseRepository
    {
        private readonly UniversityContext _context;

        public CourseRepository(UniversityContext context)
        {
            _context = context;
        }

        public async Task<Models.Database.Course?> GetByIdAsync(int id)
        {
            return await _context.Courses
                .Include(x => x.CourseOfferings)
                .FirstOrDefaultAsync(x => x.CourseId == id);
        }

        public async Task<IEnumerable<Models.Database.Course>> GetAllAsync()
        {
            return await _context.Courses.ToListAsync();
        }

        public async Task AddAsync(Models.Database.Course course)
        {
            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Models.Database.Course course)
        {
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            if (await HasEnrollmentsAsync(id))
            {
                throw new InvalidOperationException("Cannot delete course because there are active enrollments.");
            }

            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Courses.AnyAsync(x => x.CourseId == id);
        }

        public async Task<bool> CourseCodeExistsAsync(string courseCode)
        {
            return await _context.Courses.AnyAsync(x => x.CourseCode == courseCode);
        }

        public async Task<bool> HasEnrollmentsAsync(int courseId)
        {
            return await _context.CourseOfferings
                .Include(x => x.Enrollments)
                .Where(x => x.CourseId == courseId)
                .AnyAsync(x => x.Enrollments.Any());
        }
    }
}
