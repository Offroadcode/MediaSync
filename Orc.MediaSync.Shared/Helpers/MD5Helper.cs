using System;
using System.IO;
using System.Security.Cryptography;

namespace Orc.MediaSync.Shared.Helpers
{
    public static class MD5Helper
    {

        /// <summary>
        /// Hashes a file using a stream
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string Hash(string file)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(file))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-","").ToLower();
                }
            }
        }
    }
}
