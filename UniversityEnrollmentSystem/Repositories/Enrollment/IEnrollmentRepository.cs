using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityEnrollmentSystem.Models.Database;

namespace UniversityEnrollmentSystem.Repositories.Enrollment
{
    public interface IEnrollmentRepository
    {
        Task<Models.Database.Enrollment> GetByIdAsync(int id);
        Task<IEnumerable<Models.Database.Enrollment>> GetAllAsync();
        Task AddAsync(Models.Database.Enrollment enrollment);
        Task UpdateAsync(Models.Database.Enrollment enrollment);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> IsStudentEnrolledInOfferingAsync(int studentId, int courseOfferingId);
    }
}