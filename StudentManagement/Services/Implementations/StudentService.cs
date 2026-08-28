using Microsoft.EntityFrameworkCore;
using StudentManagement.Database;
using StudentManagement.Dtos;
using StudentManagement.Models;
using StudentManagement.Services.Interfaces;

namespace StudentManagement.Services.Implementations
{
    public class StudentService(DataContext context, IJwtService jwtService) : IStudentService
    {
        private readonly DataContext _context = context;
        private readonly IJwtService _jwtService = jwtService;

        public async Task<StudentRes?> CreateStudent(StudentCreateReq req)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (await _context.Users.AnyAsync(x => x.Email == req.Email && !x.IsDeleted))
                    return null;

                var password = new string(req.Name.Where(char.IsLetterOrDigit).ToArray());

                if (password.Length > 8)
                    password = password[..8];

                var user = new User
                {
                    Name = req.Name,
                    Email = req.Email,
                    PasswordHash = _jwtService.Hash(password),
                    Role = "Student",
                };

                var student = new Student
                {
                    UserId = user.Id,
                    Name = req.Name,
                    Email = req.Email,
                };

                _context.Users.Add(user);
                _context.Students.Add(student);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new StudentRes
                {
                    Id = student.Id,
                    UserId = student.UserId,
                    Name = student.Name,
                    Email = student.Email,
                    Password = password
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        } 
        public async Task<List<StudentRes>> GetAllStudents()
        {
            return await _context.Students
                .Where(x => !x.IsDeleted)
                .Select(x => new StudentRes
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    Name = x.Name,
                    Email = x.Email
                })
                .ToListAsync();
        }
        public async Task<StudentRes?> GetStudentById(Guid id)
        {
            return await _context.Students
                .Where(x => x.Id == id && !x.IsDeleted)
                .Select(x => new StudentRes
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    Name = x.Name,
                    Email = x.Email
                })
                .FirstOrDefaultAsync();
        }
        public async Task<StudentRes?> UpdateStudent(Guid id, StudentUpdateReq req)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var student = await _context.Students
                    .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

                if (student == null)
                    return null;

                var user = await _context.Users
                    .FirstOrDefaultAsync(x => x.Id == student.UserId && !x.IsDeleted);

                if (user == null)
                    return null;

                var emailExists = await _context.Users
                    .AnyAsync(x => x.Email == req.Email && x.Id != user.Id && !x.IsDeleted);

                if (emailExists)
                    return null;

                student.Name = req.Name;
                student.Email = req.Email;
                student.UpdatedAt = DateTime.UtcNow;

                user.Name = req.Name;
                user.Email = req.Email;
                user.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new StudentRes
                {
                    Id = student.Id,
                    UserId = student.UserId,
                    Name = student.Name,
                    Email = student.Email
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<List<Guid>?> DeleteStudents(List<Guid> ids)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var students = await _context.Students
                    .Where(x => ids.Contains(x.Id) && !x.IsDeleted)
                    .ToListAsync();

                if (students.Count == 0)
                    return null;

                var userIds = students.Select(x => x.UserId).ToList();

                var users = await _context.Users
                    .Where(x => userIds.Contains(x.Id) && !x.IsDeleted)
                    .ToListAsync();

                var deletedAt = DateTime.UtcNow;

                foreach (var student in students)
                {
                    student.IsDeleted = true;
                    student.DeletedAt = deletedAt;
                    student.UpdatedAt = deletedAt;
                }

                foreach (var user in users)
                {
                    user.IsDeleted = true;
                    user.DeletedAt = deletedAt;
                    user.UpdatedAt = deletedAt;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return students.Select(x => x.Id).ToList();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}