using System;
using System.Linq;
using JetBrains.Annotations;

namespace WebApiBoilerplate.Framework.Utils
{
    public static class HexConverter
    {
        [ContractAnnotation("null => null; notnull => notnull")]
        public static byte[] StringToHex(string hex)
        {
            if (hex == null)
            {
                return null;
            }

            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        [ContractAnnotation("bytes:null => null; bytes:notnull => notnull")]
        public static string HexToString(this byte[] bytes, [NotNull] string format = "X2")
        {
            if (bytes == null)
            {
                return null;
            }

            return HexToString(bytes, 0, bytes.Length, format);
        }

        [ContractAnnotation("bytes:null => null; bytes:notnull => notnull")]
        public static string HexToString(this byte[] bytes, int startIndex, int length, [NotNull] string format = "X2")
        {
            if (bytes == null)
            {
                return null;
            }

            return String.Concat(bytes.Skip(startIndex).Take(length).Select(b => b.ToString(format)));
        }
    }
}
