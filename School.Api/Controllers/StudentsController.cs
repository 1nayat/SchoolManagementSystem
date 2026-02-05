using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.Common.Auth;
using School.Application.Students.DTOs;
using School.Application.Students.Interfaces;

[Authorize]
[ApiController]
[Route("api/students")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpPost("upsert")]
    public async Task<IActionResult> UpsertStudent(
        UpsertStudentRequest request)
    {
        var studentId = await _studentService.UpsertAsync(request);
        return Ok(new { studentId });
    }

    [HttpGet]
    public async Task<IActionResult> GetStudents()
    {
        return Ok(await _studentService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStudent(Guid id)
    {
        return Ok(await _studentService.GetByIdAsync(id));
    }

    [Authorize(Roles = Roles.SchoolAdmin)]
    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(
        Guid id,
        UpdateStudentStatusRequest request)
    {
        await _studentService.UpdateStatusAsync(id, request.IsActive);
        return NoContent();
    }
}
