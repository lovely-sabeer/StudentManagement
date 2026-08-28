using StudentManagement.Dtos;

namespace StudentManagement.Services.Interfaces
{
    public interface IStudentService
    {
        Task<StudentRes?> CreateStudent(StudentCreateReq req);
        Task<List<StudentRes>> GetAllStudents();
        Task<StudentRes?> GetStudentById(Guid id);
        Task<StudentRes?> UpdateStudent(Guid id, StudentUpdateReq req);
        Task<List<Guid>?> DeleteStudents(List<Guid> ids);
    }
}
