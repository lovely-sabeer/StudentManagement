using StudentManagement.Dtos;

namespace StudentManagement.Services.Interfaces
{
    public interface IEnrollmentService
    {
        Task<List<EnrollmentRes>?> Enroll(EnrollmentCreateReq req);
        Task<List<EnrollmentRes>> GetByStudentId(Guid studentId);
        Task<List<EnrollmentRes>> GetBySubjectId(Guid subjectId);
        Task<List<Guid>?> Delete(List<Guid> ids);
    }
}