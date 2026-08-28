using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Dtos;
using StudentManagement.Services.Interfaces;

namespace StudentManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Tags("Marks")]
    [Authorize]
    public class MarkController(IMarkService service) : ControllerBase
    {
        private readonly IMarkService _service = service;

        [HttpPost("create")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Create([FromBody] MarkCreateReq request)
        {
            try
            {
                if (request.StudentId == Guid.Empty)
                    return BadRequest(new { message = "Invalid student ID." });

                if (request.SubjectId == Guid.Empty)
                    return BadRequest(new { message = "Invalid subject ID." });

                if (request.MaximumMarks <= 0)
                    return BadRequest(new { message = "Maximum marks must be greater than zero." });

                if (request.Marks < 0 || request.Marks > request.MaximumMarks)
                    return BadRequest(new { message = "Marks must be between zero and maximum marks." });

                var data = await _service.Create(request);
                if (data == null)
                    return BadRequest(new { message = "Student or subject not found, student is not enrolled in the subject, or marks already exist." });

                return Ok(new { message = "Mark created successfully.", data });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "An error occurred while creating the mark." });
            }
        }

        [HttpGet("getbystudent/{studentId}")]
        [Authorize(Roles = "Staff,Student")]
        public async Task<IActionResult> GetByStudentId([FromRoute] Guid studentId)
        {
            try
            {
                if (studentId == Guid.Empty)
                    return BadRequest(new { message = "Invalid student ID." });

                var data = await _service.GetByStudentId(studentId);
                return Ok(new { message = "Student marks retrieved successfully.", data });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "An error occurred while retrieving student marks." });
            }
        }

        [HttpGet("getbysubject/{subjectId}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> GetBySubjectId([FromRoute] Guid subjectId)
        {
            try
            {
                if (subjectId == Guid.Empty)
                    return BadRequest(new { message = "Invalid subject ID." });

                var data = await _service.GetBySubjectId(subjectId);
                return Ok(new { message = "Subject marks retrieved successfully.", data });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "An error occurred while retrieving subject marks." });
            }
        }

        [HttpPost("update/{id}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] MarkUpdateReq request)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest(new { message = "Invalid mark ID." });

                if (request.MaximumMarks <= 0)
                    return BadRequest(new { message = "Maximum marks must be greater than zero." });

                if (request.Marks < 0 || request.Marks > request.MaximumMarks)
                    return BadRequest(new { message = "Marks must be between zero and maximum marks." });

                var data = await _service.Update(id, request);
                if (data == null)
                    return BadRequest(new { message = "Mark not found." });

                return Ok(new { message = "Mark updated successfully.", data });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "An error occurred while updating the mark." });
            }
        }

        [HttpPost("delete")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Delete([FromBody] List<Guid> ids)
        {
            try
            {
                if (ids == null || ids.Count == 0)
                    return BadRequest(new { message = "At least one mark ID is required." });

                if (ids.Any(id => id == Guid.Empty))
                    return BadRequest(new { message = "One or more mark IDs are invalid." });

                var data = await _service.Delete(ids);
                if (data == null)
                    return BadRequest(new { message = "No marks found." });

                return Ok(new { message = "Marks deleted successfully.", data });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "An error occurred while deleting marks." });
            }
        }
    }
}