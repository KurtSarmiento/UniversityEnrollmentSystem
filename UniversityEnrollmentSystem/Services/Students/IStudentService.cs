using Microsoft.EntityFrameworkCore.Storage;
using UniversityEnrollmentSystem.Models.Database;
namespace UniversityEnrollmentSystem.Services.Students
{
    public interface IStudentService
    {
        Task CreateStudent(Student student);
        Task UpdateStudent(Student student);
        Task DeleteStudent(int id);
        Task<List<Student>> GetAllStudents();
        Task<Student?> GetStudentById(int id);
    }
}
