using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Dtos;
using StudentManagement.Services.Interfaces;

namespace StudentManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Tags("Results")]
    [Authorize]
    public class ResultController(IResultService service) : ControllerBase
    {
        private readonly IResultService _service = service;

        [HttpGet("getbystudent/{studentId}")]
        [Authorize(Roles = "Staff,Student")]
        public async Task<IActionResult> GetByStudentId([FromRoute] Guid studentId)
        {
            try
            {
                if (studentId == Guid.Empty)
                    return BadRequest(new { message = "Invalid student ID." });

                var data = await _service.GetByStudentId(studentId);
                if (data == null)
                    return BadRequest(new { message = "Student result not found." });

                return Ok(new { message = "Student result retrieved successfully.", data });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "An error occurred while retrieving the student result." });
            }
        }

        [HttpGet("ranking")]
        [Authorize(Roles = "Staff,Student")]
        public async Task<IActionResult> GetRanking()
        {
            try
            {
                var data = await _service.GetRanking();
                return Ok(new { message = "Student ranking retrieved successfully.", data });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "An error occurred while retrieving the student ranking." });
            }
        }
    }
}