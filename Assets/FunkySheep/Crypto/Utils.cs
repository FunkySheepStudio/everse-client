using System.Text;

namespace FunkySheep
{
    public static class Crypto
    {
        /// <summary>
        /// Get the hash of a string using MD5
        /// </summary>
        /// <param name="String to be hashed"></param>
        /// <returns>The MD5 hash of the string</returns>
        public static string Hash(string str)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(str);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}