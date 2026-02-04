using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Application.Invites.DTOs;

public class SetPasswordRequest
{
    public string Token { get; set; } = null!;
    public string Password { get; set; } = null!;
}

