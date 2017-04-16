using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.DataModel.Extensions
{
    public static class StringExtensions
    {
        public static string GetHashString(this string str)
        {
            // The used hash does not need to be cryptographically secure.
            // However, it needs to be performant.
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                StringBuilder builder = new StringBuilder();

                byte[] bytes = Encoding.UTF8.GetBytes(str);
                byte[] hash = sha1.ComputeHash(bytes);

                foreach (byte b in hash)
                {
                    builder.Append(b.ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}
