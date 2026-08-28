using Microsoft.EntityFrameworkCore;
using StudentManagement.Database;
using StudentManagement.Dtos;
using StudentManagement.Models;
using StudentManagement.Services.Interfaces;

namespace StudentManagement.Services.Implementations
{
    public class AuthService(IJwtService jwtService, DataContext context) : IAuthService
    {
        private readonly IJwtService _jwtService = jwtService;
        private readonly DataContext _context = context;

        public async Task<UserRes?> Register(RegisterReq req)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                if (await _context.Users.AnyAsync(x => x.Email == req.Email && !x.IsDeleted))
                    return null;

                var user = new User
                {
                    Name = req.Name,
                    Email = req.Email,
                    PasswordHash = _jwtService.Hash(req.Password),
                    Role = "Staff",
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new UserRes
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role,
                    Token = string.Empty
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserRes?> Login(LoginReq req)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == req.Email && !x.IsDeleted);

            if (user == null)
                return null;

            if (!_jwtService.Verify(req.Password, user.PasswordHash))
                return null;

            var token = _jwtService.GenerateToken(user);

            return new UserRes
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Token = token
            };
        }
    }
}