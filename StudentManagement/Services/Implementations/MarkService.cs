using Microsoft.EntityFrameworkCore;
using StudentManagement.Database;
using StudentManagement.Dtos;
using StudentManagement.Models;
using StudentManagement.Services.Interfaces;

namespace StudentManagement.Services.Implementations
{
    public class MarkService(DataContext context) : IMarkService
    {
        private readonly DataContext _context = context;

        public async Task<MarkRes?> Create(MarkCreateReq req)
        {
            try
            {

                var student = await _context.Students
                    .FirstOrDefaultAsync(x => x.Id == req.StudentId && !x.IsDeleted);

                if (student == null)
                    return null;

                var subject = await _context.Subjects
                    .FirstOrDefaultAsync(x => x.Id == req.SubjectId && !x.IsDeleted);

                if (subject == null)
                    return null;

                var enrolled = await _context.Enrollments.AnyAsync(x =>
                    x.StudentId == req.StudentId &&
                    x.SubjectId == req.SubjectId &&
                    !x.IsDeleted);

                if (!enrolled)
                    return null;

                var exists = await _context.Marks.AnyAsync(x =>
                    x.StudentId == req.StudentId &&
                    x.SubjectId == req.SubjectId &&
                    !x.IsDeleted);

                if (exists)
                    return null;

                var mark = new Mark
                {
                    StudentId = req.StudentId,
                    SubjectId = req.SubjectId,
                    Marks = req.Marks,
                    MaximumMarks = req.MaximumMarks
                };

                _context.Marks.Add(mark);
                await _context.SaveChangesAsync();

                return new MarkRes
                {
                    Id = mark.Id,
                    StudentId = student.Id,
                    StudentName = student.Name,
                    SubjectId = subject.Id,
                    SubjectName = subject.Name,
                    Marks = mark.Marks,
                    MaximumMarks = mark.MaximumMarks
                };
            }
            catch(Exception ex)
            {
                // Log the exception (you can use a logging framework like Serilog, NLog, etc.)
                Console.WriteLine($"An error occurred while creating a mark: {ex.Message}");
                return null;
            }
        }

        public async Task<List<MarkRes>> GetByStudentId(Guid studentId)
        {
            return await _context.Marks
                .Where(x => x.StudentId == studentId && !x.IsDeleted)
                .Select(x => new MarkRes
                {
                    Id = x.Id,
                    StudentId = x.StudentId,
                    StudentName = x.Student!.Name,
                    SubjectId = x.SubjectId,
                    SubjectName = x.Subject!.Name,
                    Marks = x.Marks,
                    MaximumMarks = x.MaximumMarks
                })
                .ToListAsync();
        }

        public async Task<List<MarkRes>> GetBySubjectId(Guid subjectId)
        {
            return await _context.Marks
                .Where(x => x.SubjectId == subjectId && !x.IsDeleted)
                .Select(x => new MarkRes
                {
                    Id = x.Id,
                    StudentId = x.StudentId,
                    StudentName = x.Student!.Name,
                    SubjectId = x.SubjectId,
                    SubjectName = x.Subject!.Name,
                    Marks = x.Marks,
                    MaximumMarks = x.MaximumMarks
                })
                .ToListAsync();
        }

        public async Task<MarkRes?> Update(Guid id, MarkUpdateReq req)
        {
            var mark = await _context.Marks
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (mark == null)
                return null;

            mark.Marks = req.Marks;
            mark.MaximumMarks = req.MaximumMarks;
            mark.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return await _context.Marks
                .Where(x => x.Id == id)
                .Select(x => new MarkRes
                {
                    Id = x.Id,
                    StudentId = x.StudentId,
                    StudentName = x.Student!.Name,
                    SubjectId = x.SubjectId,
                    SubjectName = x.Subject!.Name,
                    Marks = x.Marks,
                    MaximumMarks = x.MaximumMarks
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<Guid>?> Delete(List<Guid> ids)
        {
            var marks = await _context.Marks
                .Where(x => ids.Contains(x.Id) && !x.IsDeleted)
                .ToListAsync();

            if (marks.Count == 0)
                return null;

            var deletedAt = DateTime.UtcNow;

            foreach (var mark in marks)
            {
                mark.IsDeleted = true;
                mark.DeletedAt = deletedAt;
                mark.UpdatedAt = deletedAt;
            }

            await _context.SaveChangesAsync();

            return marks.Select(x => x.Id).ToList();
        }
    }
}