using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Api
{
    public static class StreamExtensions
    {
        public static byte[] ReadAllBytes(this Stream stream)
        {
            if (stream is MemoryStream)
            {
                return ((MemoryStream)stream).ToArray();
            }
            else
            {
                using (var result = new MemoryStream())
                {
                    stream.CopyTo(result);

                    return result.ToArray();
                }
            }
        }
    }
}
