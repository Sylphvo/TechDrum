using System;
using System.Security.Cryptography;

namespace TechDrum.Core.Utils
{
    public class GenerateTokenHelper
    {
        public static string RandomTokenString()
        {
            using (RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[40];
                provider.GetBytes(randomBytes);
                return BitConverter.ToString(randomBytes).Replace("-", "");
            }
        }
    }
}
