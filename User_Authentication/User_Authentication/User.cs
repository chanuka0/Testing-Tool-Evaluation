using System;
using System.Text.RegularExpressions;

namespace AuthenticationSystem
{
    public class User
    {
        public string Username { get; set; }
        public string Email { get; set; }
        private string _passwordHash;

        public User(string username, string email)
        {
            Username = username;
            Email = email;
        }

        public bool Register(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty");

            if (!IsValidPassword(password))
                throw new ArgumentException("Password must be at least 8 characters and contain uppercase, lowercase, and a number");

            if (!IsValidEmail(Email))
                throw new ArgumentException("Invalid email format");

            _passwordHash = HashPassword(password);
            return true;
        }

        public bool Login(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty");

            if (_passwordHash == null)
                throw new InvalidOperationException("User not registered");

            return VerifyPassword(password, _passwordHash);
        }

        public static bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                return false;

            bool hasUppercase = Regex.IsMatch(password, @"[A-Z]");
            bool hasLowercase = Regex.IsMatch(password, @"[a-z]");
            bool hasDigit = Regex.IsMatch(password, @"[0-9]");

            return hasUppercase && hasLowercase && hasDigit;
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

                return Regex.IsMatch(email, pattern) && addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private static string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private static bool VerifyPassword(string password, string hash)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput.Equals(hash);
        }
    }
}