using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Dtos;
using StudentManagement.Services.Interfaces;

namespace StudentManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Tags("Enrollments")]
    [Authorize(Roles = "Staff")]
    public class EnrollmentController(IEnrollmentService service) : ControllerBase
    {
        private readonly IEnrollmentService _service = service;

        [HttpPost("enroll")]
        public async Task<IActionResult> Enroll([FromBody] EnrollmentCreateReq request)
        {
            try
            {
                if (request.StudentId == Guid.Empty)
                    return BadRequest(new { message = "Invalid student ID." });

                if (request.SubjectIds == null || request.SubjectIds.Count == 0)
                    return BadRequest(new { message = "At least one subject ID is required." });

                if (request.SubjectIds.Any(id => id == Guid.Empty))
                    return BadRequest(new { message = "One or more subject IDs are invalid." });

                var data = await _service.Enroll(request);
                if (data == null)
                    return BadRequest(new { message = "Student or subject not found, or student is already enrolled." });

                return Ok(new { message = "Student enrolled successfully.", data });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "An error occurred while enrolling the student." });
            }
        }

        [HttpGet("getbystudent/{studentId}")]
        public async Task<IActionResult> GetByStudentId([FromRoute] Guid studentId)
        {
            try
            {
                if (studentId == Guid.Empty)
                    return BadRequest(new { message = "Invalid student ID." });

                var data = await _service.GetByStudentId(studentId);
                return Ok(new { message = "Student enrollments retrieved successfully.", data });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "An error occurred while retrieving student enrollments." });
            }
        }

        [HttpGet("getbysubject/{subjectId}")]
        public async Task<IActionResult> GetBySubjectId([FromRoute] Guid subjectId)
        {
            try
            {
                if (subjectId == Guid.Empty)
                    return BadRequest(new { message = "Invalid subject ID." });

                var data = await _service.GetBySubjectId(subjectId);
                return Ok(new { message = "Subject enrollments retrieved successfully.", data });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "An error occurred while retrieving subject enrollments." });
            }
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] List<Guid> ids)
        {
            try
            {
                if (ids == null || ids.Count == 0)
                    return BadRequest(new { message = "At least one enrollment ID is required." });

                if (ids.Any(id => id == Guid.Empty))
                    return BadRequest(new { message = "One or more enrollment IDs are invalid." });

                var data = await _service.Delete(ids);
                if (data == null)
                    return BadRequest(new { message = "No enrollments found." });

                return Ok(new { message = "Enrollments deleted successfully.", data });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "An error occurred while deleting enrollments." });
            }
        }
    }
}