using UniversityEnrollmentSystem.Models.Database;
using UniversityEnrollmentSystem.Repositories.Students;

namespace UniversityEnrollmentSystem.Services.Students
{
    public class StudentService(IStudentRepository studentRepository) : IStudentService
    {
        private readonly IStudentRepository _studentRepository = studentRepository;
        public async Task CreateStudent(Student student)
        {
            _studentRepository.AddStudent(student);
        }

        public async Task DeleteStudent(int id)
        {
            _studentRepository.DeleteStudent(id);
        }

        public Task<List<Student>> GetAllStudents()
        {
            return _studentRepository.GetAllStudents();
        }

        public Task<Student?> GetStudentById(int id)
        {
            return _studentRepository.GetStudentById(id);
        }

        public async Task UpdateStudent(Student student)
        {
            _studentRepository.UpdateStudent(student);
        }
    }
}
