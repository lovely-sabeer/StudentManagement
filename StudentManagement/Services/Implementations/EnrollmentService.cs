using Microsoft.EntityFrameworkCore;
using StudentManagement.Database;
using StudentManagement.Dtos;
using StudentManagement.Models;
using StudentManagement.Services.Interfaces;

namespace StudentManagement.Services.Implementations
{
    public class EnrollmentService(DataContext context) : IEnrollmentService
    {
        private readonly DataContext _context = context;

        public async Task<List<EnrollmentRes>?> Enroll(EnrollmentCreateReq req)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var student = await _context.Students
                    .FirstOrDefaultAsync(x => x.Id == req.StudentId && !x.IsDeleted);

                if (student == null)
                    return null;

                var subjects = await _context.Subjects
                    .Where(x => req.SubjectIds.Contains(x.Id) && !x.IsDeleted)
                    .ToListAsync();

                if (subjects.Count != req.SubjectIds.Count)
                    return null;

                var existingSubjectIds = await _context.Enrollments
                    .Where(x => x.StudentId == req.StudentId &&
                                req.SubjectIds.Contains(x.SubjectId) &&
                                !x.IsDeleted)
                    .Select(x => x.SubjectId)
                    .ToListAsync();

                if (existingSubjectIds.Count > 0)
                    return null;

                var enrollments = subjects.Select(subject => new Enrollment
                {
                    StudentId = student.Id,
                    SubjectId = subject.Id
                }).ToList();

                _context.Enrollments.AddRange(enrollments);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return enrollments.Select(x =>
                {
                    var subject = subjects.First(s => s.Id == x.SubjectId);

                    return new EnrollmentRes
                    {
                        Id = x.Id,
                        StudentId = student.Id,
                        StudentName = student.Name,
                        SubjectId = subject.Id,
                        SubjectName = subject.Name
                    };
                }).ToList();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<EnrollmentRes>> GetByStudentId(Guid studentId)
        {
            return await _context.Enrollments
                .Where(x => x.StudentId == studentId && !x.IsDeleted)
                .Select(x => new EnrollmentRes
                {
                    Id = x.Id,
                    StudentId = x.StudentId,
                    StudentName = x.Student!.Name,
                    SubjectId = x.SubjectId,
                    SubjectName = x.Subject!.Name
                })
                .ToListAsync();
        }

        public async Task<List<EnrollmentRes>> GetBySubjectId(Guid subjectId)
        {
            return await _context.Enrollments
                .Where(x => x.SubjectId == subjectId && !x.IsDeleted)
                .Select(x => new EnrollmentRes
                {
                    Id = x.Id,
                    StudentId = x.StudentId,
                    StudentName = x.Student!.Name,
                    SubjectId = x.SubjectId,
                    SubjectName = x.Subject!.Name
                })
                .ToListAsync();
        }

        public async Task<List<Guid>?> Delete(List<Guid> ids)
        {
            var enrollments = await _context.Enrollments
                .Where(x => ids.Contains(x.Id) && !x.IsDeleted)
                .ToListAsync();

            if (enrollments.Count == 0)
                return null;

            var deletedAt = DateTime.UtcNow;

            foreach (var enrollment in enrollments)
            {
                enrollment.IsDeleted = true;
                enrollment.DeletedAt = deletedAt;
                enrollment.UpdatedAt = deletedAt;
            }

            await _context.SaveChangesAsync();

            return enrollments.Select(x => x.Id).ToList();
        }
    }
}