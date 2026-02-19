using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityEnrollmentSystem.Models.Database;

namespace UniversityEnrollmentSystem.Services
{
    public interface ICourseOfferingService
    {
        Task<bool> CreateOfferingAsync(CourseOffering offering);
        Task<CourseOffering> GetOfferingByIdAsync(int id);
        Task<IEnumerable<CourseOffering>> GetAllOfferingsAsync();
        Task<bool> DeleteOfferingAsync(int id);
    }
}