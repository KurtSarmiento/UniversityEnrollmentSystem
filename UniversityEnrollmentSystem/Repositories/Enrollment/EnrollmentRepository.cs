using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UniversityEnrollmentSystem.Models.Database;

namespace UniversityEnrollmentSystem.Repositories.Enrollment
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly UniversityContext _context;

        public EnrollmentRepository(UniversityContext context)
        {
            _context = context;
        }

        public async Task<Models.Database.Enrollment?> GetByIdAsync(int id)
        {
            return await _context.Enrollments
                .Include(x => x.Student)
                .Include(x => x.CourseOffering)
                .ThenInclude(x => x.Course)
                .FirstOrDefaultAsync(x => x.EnrollmentId == id);
        }

        public async Task<IEnumerable<Models.Database.Enrollment>> GetAllAsync()
        {
            return await _context.Enrollments
                .Include(x => x.Student)
                .Include(x => x.CourseOffering)
                .ToListAsync();
        }

        public async Task AddAsync(Models.Database.Enrollment enrollment)
        {
            await _context.Enrollments.AddAsync(enrollment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Models.Database.Enrollment enrollment)
        {
            _context.Enrollments.Update(enrollment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment != null)
            {
                _context.Enrollments.Remove(enrollment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Enrollments.AnyAsync(e => e.EnrollmentId == id);
        }

        public async Task<bool> IsStudentEnrolledInOfferingAsync(int studentId, int courseOfferingId)
        {
            return await _context.Enrollments
                .AnyAsync(e => e.StudentId == studentId && e.CourseOfferingId == courseOfferingId);
        }

    }
}