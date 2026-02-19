using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityEnrollmentSystem.Models.Database;
using UniversityEnrollmentSystem.Repositories.CourseOffering;

namespace UniversityEnrollmentSystem.Services
{
    public class CourseOfferingService : ICourseOfferingService
    {
        private readonly ICourseOfferingRepository _Repo;

        public CourseOfferingService(ICourseOfferingRepository courseOfferingRepo)
        {
            _Repo = courseOfferingRepo;
        }

        public async Task<bool> CreateOfferingAsync(CourseOffering offering)
        {
            if (await _Repo.IsDuplicateOfferingAsync(offering.CourseId, offering.SemesterId))
            {
                throw new InvalidOperationException("Course offering already exists for this semester.");
            }

            await _Repo.AddAsync(offering);
            return true;
        }

        public async Task<CourseOffering> GetOfferingByIdAsync(int id)
        {
            return await _Repo.GetByIdAsync(id);
        }

        public async Task<IEnumerable<CourseOffering>> GetAllOfferingsAsync()
        {
            return await _Repo.GetAllAsync();
        }

        public async Task<bool> DeleteOfferingAsync(int id)
        {
            if (!await _Repo.ExistsAsync(id))
            {
                return false;
            }

            await _Repo.DeleteAsync(id);
            return true;
        }
    }
}