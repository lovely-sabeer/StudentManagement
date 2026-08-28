using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Dtos;
using StudentManagement.Services.Interfaces;

namespace StudentManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Tags("Subjects")]
    [Authorize(Roles = "Staff")]
    public class SubjectController(ISubjectService service) : ControllerBase
    {
        private readonly ISubjectService _service = service;

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] SubjectCreateReq request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Name))
                    return BadRequest(new { message = "Subject name is required." });

                var data = await _service.Create(request);
                if (data == null)
                    return BadRequest(new { message = "Subject already exists." });

                return Ok(new { message = "Subject created successfully.", data });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "An error occurred while creating the subject." });
            }
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var data = await _service.GetAll();
                return Ok(new { message = "Subjects retrieved successfully.", data });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "An error occurred while retrieving subjects." });
            }
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest(new { message = "Invalid subject ID." });

                var data = await _service.GetById(id);
                if (data == null)
                    return BadRequest(new { message = "Subject not found." });

                return Ok(new { message = "Subject retrieved successfully.", data });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "An error occurred while retrieving the subject." });
            }
        }

        [HttpPost("update/{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] SubjectUpdateReq request)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest(new { message = "Invalid subject ID." });

                if (string.IsNullOrWhiteSpace(request.Name))
                    return BadRequest(new { message = "Subject name is required." });

                var data = await _service.Update(id, request);
                if (data == null)
                    return BadRequest(new { message = "Subject not found or name already exists." });

                return Ok(new { message = "Subject updated successfully.", data });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "An error occurred while updating the subject." });
            }
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] List<Guid> ids)
        {
            try
            {
                if (ids == null || ids.Count == 0)
                    return BadRequest(new { message = "At least one subject ID is required." });

                if (ids.Any(id => id == Guid.Empty))
                    return BadRequest(new { message = "One or more subject IDs are invalid." });

                var data = await _service.Delete(ids);
                if (data == null)
                    return BadRequest(new { message = "No subjects found." });

                return Ok(new { message = "Subjects deleted successfully.", data });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "An error occurred while deleting subjects." });
            }
        }
    }
}