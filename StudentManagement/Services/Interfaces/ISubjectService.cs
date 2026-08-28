using StudentManagement.Dtos;

namespace StudentManagement.Services.Interfaces
{
    public interface ISubjectService
    {
        Task<SubjectRes?> Create(SubjectCreateReq req);
        Task<List<SubjectRes>> GetAll();
        Task<SubjectRes?> GetById(Guid id);
        Task<SubjectRes?> Update(Guid id, SubjectUpdateReq req);
        Task<List<Guid>?> Delete(List<Guid> ids);
    }
}