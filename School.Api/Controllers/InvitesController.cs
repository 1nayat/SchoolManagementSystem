using Microsoft.AspNetCore.Mvc;
using School.Application.Invites.DTOs;
using School.Application.Invites.Interfaces;

[ApiController]
[Route("api/invites")]
public class InvitesController : ControllerBase
{
    private readonly IInviteAcceptanceService _service;

    public InvitesController(IInviteAcceptanceService service)
    {
        _service = service;
    }

    [HttpPost("accept")]
    public async Task<IActionResult> Accept(AcceptInviteRequest request)
    {
        await _service.AcceptAsync(request);
        return Ok("Invite valid");
    }

    [HttpPost("set-password")]
    public async Task<IActionResult> SetPassword(SetPasswordRequest request)
    {
        await _service.SetPasswordAsync(request);
        return Ok("Account created successfully");
    }
}
