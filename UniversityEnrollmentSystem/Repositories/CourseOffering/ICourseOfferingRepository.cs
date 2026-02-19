namespace UniversityEnrollmentSystem.Repositories.CourseOffering
{
        public interface ICourseOfferingRepository
        {
            Task<Models.Database.CourseOffering> GetByIdAsync(int id);
            Task<IEnumerable<Models.Database.CourseOffering>> GetAllAsync();
            Task AddAsync(Models.Database.CourseOffering courseOffering);
            Task UpdateAsync(Models.Database.CourseOffering courseOffering);
            Task DeleteAsync(int id);
            Task<bool> ExistsAsync(int id);
            Task<bool> IsDuplicateOfferingAsync(int courseId, int semesterId);
            Task<bool> HasEnrollmentsForCourseAsync(int courseId);
    }
    }
