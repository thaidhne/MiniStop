using System.Text;
using System.Security.Cryptography;
using MiniStop.Service.Interface;


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

        public string GenFirstPassword(int length = 16)
        {
            if (length < 8)
                throw new ArgumentException("Mật khẩu nên có ít nhất 8 ký tự.");

            const string lower = "abcdefghijklmnopqrstuvwxyz";
            const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string digits = "0123456789";
            const string specials = "!@#$%^&*()-_=+[]{}|;:,.<>/?";

            char GetRandomChar(string chars) =>
                chars[RandomNumberGenerator.GetInt32(chars.Length)];

            var password = new char[length];
            password[0] = GetRandomChar(lower);
            password[1] = GetRandomChar(upper);
            password[2] = GetRandomChar(digits);
            password[3] = GetRandomChar(specials);

            var allChars = lower + upper + digits + specials;
            for (int i = 4; i < length; i++)
            {
                password[i] = GetRandomChar(allChars);
            }

            // Trộn ký tự bằng cách xáo trộn vị trí
            return new string(password.OrderBy(_ => RandomNumberGenerator.GetInt32(length)).ToArray());
        }
    }
 }
