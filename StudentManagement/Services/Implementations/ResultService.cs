using Microsoft.EntityFrameworkCore;
using StudentManagement.Database;
using StudentManagement.Dtos;
using StudentManagement.Services.Interfaces;

namespace StudentManagement.Services.Implementations
{
    public class ResultService(DataContext context) : IResultService
    {
        private readonly DataContext _context = context;

        public async Task<ResultRes?> GetByStudentId(Guid studentId)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(x => x.Id == studentId && !x.IsDeleted);

            if (student == null)
                return null;

            var marks = await _context.Marks
                .Where(x => x.StudentId == studentId && !x.IsDeleted && x.MaximumMarks > 0)
                .Select(x => new
                {
                    x.SubjectId,
                    SubjectName = x.Subject!.Name,
                    x.Marks,
                    x.MaximumMarks
                })
                .ToListAsync();

            if (marks.Count == 0)
                return null;

            var subjects = marks.Select(x => new SubjectMarkRes
            {
                SubjectId = x.SubjectId,
                SubjectName = x.SubjectName,
                Marks = x.Marks,
                MaximumMarks = x.MaximumMarks,
                Percentage = Math.Round((x.Marks / x.MaximumMarks) * 100, 2)
            }).ToList();

            var percentage = Math.Round(subjects.Average(x => x.Percentage), 2);

            var ranking = await GetRanking();

            var rank = ranking
                .FirstOrDefault(x => x.StudentId == studentId)?.Rank ?? 0;

            return new ResultRes
            {
                StudentId = studentId,
                StudentName = student.Name,
                Subjects = subjects,
                Percentage = percentage,
                Rank = rank
            };
        }

        public async Task<List<RankingRes>> GetRanking()
        {
            var marks = await _context.Marks
                .Where(x => !x.IsDeleted && x.MaximumMarks > 0)
                .Select(x => new
                {
                    x.StudentId,
                    StudentName = x.Student!.Name,
                    x.Marks,
                    x.MaximumMarks
                })
                .ToListAsync();

            var ranking = marks
                .GroupBy(x => new { x.StudentId, x.StudentName })
                .Select(x => new
                {
                    x.Key.StudentId,
                    x.Key.StudentName,
                    Percentage = Math.Round(
                        x.Average(m => (m.Marks / m.MaximumMarks) * 100), 2)
                })
                .OrderByDescending(x => x.Percentage)
                .ToList();

            return ranking
                .Select((x, index) => new RankingRes
                {
                    Rank = index + 1,
                    StudentId = x.StudentId,
                    StudentName = x.StudentName,
                    Percentage = x.Percentage
                })
                .ToList();
        }
    }
}