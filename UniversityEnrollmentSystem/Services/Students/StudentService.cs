using UniversityEnrollmentSystem.Models.Database;
using UniversityEnrollmentSystem.Repositories.Students;

namespace UniversityEnrollmentSystem.Services.Students
{
    public class StudentService(IStudentRepository studentRepository) : IStudentService
    {
        private readonly IStudentRepository _studentRepository = studentRepository;
        public async Task CreateStudent(Student student)
        {
            var existing = await _studentRepository.GetStudentByStudentNumber(student.StudentNumber);
            if (string.IsNullOrEmpty(student.FirstName))
            {
                throw new Exception("Student name must not be empty");
            }
            if (existing != null)
            {
                throw new Exception("Student number must be unique");
            }
            await _studentRepository.AddStudent(student);
        }

        public async Task DeleteStudent(int id)
        {
            if (!await _studentRepository.ExistsAsync(id))
            {
                throw new Exception("Student not found");
            }
            await _studentRepository.DeleteStudent(id);
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
            var exists = await _studentRepository.ExistsAsync(student.StudentId);

            if (!exists)
            {
                throw new Exception($"Student with ID {student.StudentId} not found.");
            }
            await _studentRepository.UpdateStudent(student);
        }

        public async Task<Student> GetStudentByStudentNumber(int studentNumber)
        {

            return await _studentRepository.GetStudentByStudentNumber(studentNumber);
        }
    }
}
