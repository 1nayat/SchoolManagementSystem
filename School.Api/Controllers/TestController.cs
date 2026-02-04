using Microsoft.AspNetCore.Mvc;
using School.Application.Common.Email;

[ApiController]
[Route("api/test/email")]
public class EmailTestController : ControllerBase
{
    private readonly IEmailService _emailService;

    public EmailTestController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpGet]
    public async Task<IActionResult> SendTestEmail()
    {
        await _emailService.SendAsync(
            "inayatwani477@gmail.com", 
            "SMTP Test",
            "<h3>Email works </h3>"
        );

        return Ok("Email sent successfully");
    }
}
