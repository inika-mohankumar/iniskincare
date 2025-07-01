﻿// File: Helpers/PasswordHelper.cs
using System.Security.Cryptography;
using System.Text;

namespace iniskincare.Helpers
{
    public static class PasswordHelper
    {
        // Hash the password using SHA256
        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        // Verify input password against hashed password
        public static bool VerifyPassword(string inputPassword, string storedHash)
        {
            var inputHash = HashPassword(inputPassword);
            return inputHash == storedHash;
        }
    }
}
