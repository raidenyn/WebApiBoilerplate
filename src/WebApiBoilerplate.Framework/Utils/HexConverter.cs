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

            var length = hex.Length;
            var bytes = new byte[length / 2];
            for (var i = 0; i < length; i += 2)
            {
                bytes[i] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return bytes;
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
