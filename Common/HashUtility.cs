using System.Text;
using System.Security.Cryptography;

namespace weather.Common
{
    public static class HashUtility
    {
        public static string ComputeSHA256Hash(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                var hash = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower(); // Hex encoding
            }
        }
    }
}
