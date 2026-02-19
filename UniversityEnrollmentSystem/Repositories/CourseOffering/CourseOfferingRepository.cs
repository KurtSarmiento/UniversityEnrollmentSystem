using Microsoft.EntityFrameworkCore;
using UniversityEnrollmentSystem.Models.Database;

namespace UniversityEnrollmentSystem.Repositories.CourseOffering
{
    public class CourseOfferingRepository
    {
        private readonly UniversityContext _context;

        public CourseOfferingRepository(UniversityContext context)
        {
            _context = context;
        }

        public async Task<Models.Database.CourseOffering?> GetByIdAsync(int id)
        {
            return await _context.CourseOfferings
                .Include(x => x.Course)
                .Include(x => x.Semester)
                .Include(x => x.Instructor)
                .Include(x => x.Enrollments)
                .FirstOrDefaultAsync(x => x.CourseOfferingId == id);
        }

        public async Task<IEnumerable<Models.Database.CourseOffering>> GetAllAsync()
        {
            return await _context.CourseOfferings
                .Include(x => x.Course)
                .Include(x => x.Semester)
                .ToListAsync();
        }

        public async Task AddAsync(Models.Database.CourseOffering courseOffering)
        {
            await _context.CourseOfferings.AddAsync(courseOffering);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Models.Database.CourseOffering courseOffering)
        {
            _context.CourseOfferings.Update(courseOffering);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var co = await _context.CourseOfferings.FindAsync(id);
            if (co != null)
            {
                _context.CourseOfferings.Remove(co);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.CourseOfferings.AnyAsync(e => e.CourseOfferingId == id);
        }

        public async Task<bool> IsDuplicateOfferingAsync(int courseId, int semesterId)
        {
            return await _context.CourseOfferings
                .AnyAsync(co => co.CourseId == courseId && co.SemesterId == semesterId);
        }
    }
}
