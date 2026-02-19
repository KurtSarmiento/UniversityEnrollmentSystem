using UniversityEnrollmentSystem.Models.Database;
using UniversityEnrollmentSystem.Repositories.Students;
using Microsoft.EntityFrameworkCore;

namespace UniversityEnrollmentSystem.Repositories.Student
{
    public class StudentRepository(UniversityContext context) : IStudentRepository
    {
        private readonly UniversityContext _context = context;
        public async Task AddStudent(Models.Database.Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteStudent(int id)
        {
            var student = _context.Students.FirstOrDefault(s => s.StudentId == id);
            _context.Remove(student);
            _context.SaveChanges();
        }

        public async Task<List<Models.Database.Student>> GetAllStudents()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Models.Database.Student?> GetStudentById(int id)
        {
            return await _context.Students.FirstOrDefaultAsync(s => s.StudentId == id);
        }

        public async Task UpdateStudent(Models.Database.Student student)
        {
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
        }

        public async Task<Models.Database.Student?> GetStudentByStudentNumber(int studentNumber)
        {
            return await _context.Students.FirstOrDefaultAsync(s => s.StudentNumber == studentNumber);
        }
    }
}
