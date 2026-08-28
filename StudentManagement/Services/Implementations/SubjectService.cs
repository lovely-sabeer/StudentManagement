using Microsoft.EntityFrameworkCore;
using StudentManagement.Database;
using StudentManagement.Dtos;
using StudentManagement.Models;
using StudentManagement.Services.Interfaces;

namespace StudentManagement.Services.Implementations
{
    public class SubjectService(DataContext context) : ISubjectService
    {
        private readonly DataContext _context = context;

        public async Task<SubjectRes?> Create(SubjectCreateReq req)
        {
            if (await _context.Subjects.AnyAsync(x => x.Name == req.Name && !x.IsDeleted))
                return null;

            var subject = new Subject
            {
                Name = req.Name.Trim()
            };

            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();

            return new SubjectRes
            {
                Id = subject.Id,
                Name = subject.Name
            };
        }

        public async Task<List<SubjectRes>> GetAll()
        {
            return await _context.Subjects
                .Where(x => !x.IsDeleted)
                .Select(x => new SubjectRes
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();
        }

        public async Task<SubjectRes?> GetById(Guid id)
        {
            return await _context.Subjects
                .Where(x => x.Id == id && !x.IsDeleted)
                .Select(x => new SubjectRes
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .FirstOrDefaultAsync();
        }

        public async Task<SubjectRes?> Update(Guid id, SubjectUpdateReq req)
        {
            var subject = await _context.Subjects
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (subject == null)
                return null;

            if (await _context.Subjects.AnyAsync(x =>
                x.Name == req.Name && x.Id != id && !x.IsDeleted))
                return null;

            subject.Name = req.Name.Trim();
            subject.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new SubjectRes
            {
                Id = subject.Id,
                Name = subject.Name
            };
        }

        public async Task<List<Guid>?> Delete(List<Guid> ids)
        {
            var subjects = await _context.Subjects
                .Where(x => ids.Contains(x.Id) && !x.IsDeleted)
                .ToListAsync();

            if (subjects.Count == 0)
                return null;

            var deletedAt = DateTime.UtcNow;

            foreach (var subject in subjects)
            {
                subject.IsDeleted = true;
                subject.DeletedAt = deletedAt;
                subject.UpdatedAt = deletedAt;
            }

            await _context.SaveChangesAsync();

            return subjects.Select(x => x.Id).ToList();
        }
    }
}