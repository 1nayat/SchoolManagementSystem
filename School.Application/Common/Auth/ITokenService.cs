using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using School.Domain.Entities;

namespace School.Application.Common.Auth;

public interface ITokenService
{
    string GenerateToken(User user);
}

