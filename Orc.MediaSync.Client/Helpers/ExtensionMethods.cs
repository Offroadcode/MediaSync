using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.MediaSync.Client.Helpers
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Extensiton method to convert a filesize into a human readable format, kB MB etc
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string ToFileSize(this long size)
        {
            return String.Format(new FileSizeFormatProvider(), "{0:fs}", size);
        }
    }
}
