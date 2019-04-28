using System;
using System.Security.Cryptography;

namespace AMI.Core.Security
{
    /// <summary>
    /// Provides helper methods related to cryptography.
    /// </summary>
    public static class Cryptography
    {
        /// <summary>
        /// Calculates the SHA1 hash of the byte array.
        /// </summary>
        /// <param name="byteArray">The byte array.</param>
        /// <returns>The SHA1 hash of the byte array.</returns>
        public static string CalculateSha1Hash(byte[] byteArray)
        {
            try
            {
                if (byteArray != null)
                {
                    using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider())
                    {
                        return Convert.ToBase64String(sha1.ComputeHash(byteArray));
                    }
                }

                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
