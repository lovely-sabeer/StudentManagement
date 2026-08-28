using Microsoft.AspNetCore.Mvc;
using StudentManagement.Dtos;

namespace StudentManagement.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserRes?> Register(RegisterReq req);
        Task<UserRes?> Login(LoginReq req);
    }
}
