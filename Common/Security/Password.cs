﻿using System.Text;
using System.Security.Cryptography;
using MiniStop.Interface;

namespace MiniStop.Common.Security
{
    public class Password : IPassword
    {
        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
