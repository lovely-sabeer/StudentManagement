using StudentManagement.Dtos;

namespace StudentManagement.Services.Interfaces
{
    public interface IMarkService
    {
        Task<MarkRes?> Create(MarkCreateReq req);
        Task<List<MarkRes>> GetByStudentId(Guid studentId);
        Task<List<MarkRes>> GetBySubjectId(Guid subjectId);
        Task<MarkRes?> Update(Guid id, MarkUpdateReq req);
        Task<List<Guid>?> Delete(List<Guid> ids);
    }
}