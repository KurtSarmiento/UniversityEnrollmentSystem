using Microsoft.EntityFrameworkCore.Storage;
using UniversityEnrollmentSystem.Models.Database;
namespace UniversityEnrollmentSystem.Services.Students
{
    public interface IStudentService
    {
        Task CreateStudent(Student student);
    }
}
