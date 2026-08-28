using StudentManagement.Dtos;

namespace StudentManagement.Services.Interfaces
{
    public interface IResultService
    {
        Task<ResultRes?> GetByStudentId(Guid studentId);
        Task<List<RankingRes>> GetRanking();
    }
}