using StudentManagement.Models;

namespace StudentManagement.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        string Hash(string password);
        bool Verify(string password, string passwordHash);
    }
}
