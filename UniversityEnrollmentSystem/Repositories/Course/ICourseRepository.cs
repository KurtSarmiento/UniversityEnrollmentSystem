namespace UniversityEnrollmentSystem.Repositories.Course
{
    public interface ICourseRepository
    {
        Task<Models.Database.Course> GetByIdAsync(int id);
        Task<IEnumerable<Models.Database.Course>> GetAllAsync();
        Task AddAsync(Models.Database.Course course);
        Task UpdateAsync(Models.Database.Course course);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CourseCodeExistsAsync(string courseCode);
        Task<bool> HasEnrollmentsAsync(int courseId);
    }
}
