using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.Common.Auth;
using School.Application.Teachers.DTOs;
using School.Application.Teachers.Interfaces;

[Authorize]
[ApiController]
[Route("api/teachers")]
public class TeachersController : ControllerBase
{
    private readonly ITeacherInviteService _inviteService;
    private readonly ITeacherService _teacherService;

    public TeachersController(
        ITeacherInviteService inviteService,
        ITeacherService teacherService)
    {
        _inviteService = inviteService;
        _teacherService = teacherService;
    }

    [Authorize(Policy = Policies.RequireSchoolAdmin)]
    [HttpPost("invite")]
    public async Task<IActionResult> InviteTeacher(
     InviteTeacherRequest request)
    {
        var token = await _inviteService.InviteAsync(request);

        if (token != null)
            return Ok(new { message = "Invitation sent", token });

        return Ok(new { message = "Invitation sent" });
    }


    [Authorize(Policy = Policies.RequireSchoolAdmin)]
    [HttpGet]
    public async Task<IActionResult> GetTeachers()
    {
        return Ok(await _teacherService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTeacher(Guid id)
    {
        return Ok(await _teacherService.GetByIdAsync(id));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTeacher(
        Guid id,
        UpdateTeacherRequest request)
    {
        await _teacherService.UpdateAsync(id, request);
        return NoContent();
    }

    [Authorize(Policy = Policies.RequireSchoolAdmin)]
    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(
        Guid id,
        UpdateTeacherStatusRequest request)
    {
        await _teacherService.UpdateStatusAsync(id, request.IsActive);
        return NoContent();
    }
}
