using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Dtos;
using StudentManagement.Services.Interfaces;

namespace StudentManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Tags("Students")]
    [Authorize(Roles = "Staff")]
    public class StudentController(IStudentService service) : ControllerBase
    {
        private readonly IStudentService _service = service;
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] StudentCreateReq request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Name))
                    return BadRequest(new { message = "Student name is required." });

                if (string.IsNullOrWhiteSpace(request.Email))
                    return BadRequest(new { message = "Student email is required." });

                var data = await _service.CreateStudent(request);
                if(data == null) return BadRequest(new { message = "Failed to create student." });

                return Ok(new { message = "Student created successfully.", data });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "An error occurred while creating the student." });
            }
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var data = await _service.GetAllStudents();

                return Ok(new { message = "Students retrieved successfully.", data });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "An error occurred while retrieving students." });
            }
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest(new { message = "Invalid student ID." });

                var data = await _service.GetStudentById(id);
                if (data == null)
                    return BadRequest(new { message = "Student not found." });

                return Ok(new { message = "Student retrieved successfully.", data });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "An error occurred while retrieving the student." });
            }
        }

        [HttpPost("update/{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] StudentUpdateReq request)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest(new { message = "Invalid student ID." });

                if (string.IsNullOrWhiteSpace(request.Name))
                    return BadRequest(new { message = "Student name is required." });

                if (string.IsNullOrWhiteSpace(request.Email))
                    return BadRequest(new { message = "Student email is required." });

                var data = await _service.UpdateStudent(id, request);
                if (data == null)
                    return BadRequest(new { message = "Student not found." });

                return Ok(new { message = "Student updated successfully.", data });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "An error occurred while updating the student." });
            }
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] List<Guid> ids)
        {
            try
            {
                if (ids == null || ids.Count == 0)
                    return BadRequest(new { message = "At least one student ID is required." });

                if (ids.Any(id => id == Guid.Empty))
                    return BadRequest(new { message = "One or more student IDs are invalid." });

                var data = await _service.DeleteStudents(ids);
                if (data == null)
                    return BadRequest(new { message = "Failed to delete students." });

                return Ok(new { message = "Students deleted successfully.", data });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "An error occurred while deleting students." });
            }
        }
    }
}