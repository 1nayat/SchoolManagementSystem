using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace School.Infrastructure.Security
{
    public static class PasswordHasherHelper
    {
        private static readonly PasswordHasher<string> _hasher = new();

        public static string Hash(string password)
            => _hasher.HashPassword("seed", password);
    }
}
