using Microsoft.EntityFrameworkCore.Storage;
using UniversityEnrollmentSystem.Models.Database;
using UniversityEnrollmentSystem.Repositories.Students;

namespace UniversityEnrollmentSystem.Repositories.Students
{
    public interface IStudentRepository
    {
        Task AddStudent(Models.Database.Student student);
        Task UpdateStudent(Models.Database.Student student);
        Task DeleteStudent(int id);
        Task <bool> ExistsAsync(int id);
        Task<List<Models.Database.Student>> GetAllStudents();
        Task<Models.Database.Student?> GetStudentById(int id);
        Task<Models.Database.Student?> GetStudentByStudentNumber(int studentNumber);
    }
}
