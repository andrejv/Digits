using System;
using System.IO;
using System.Linq;

namespace Digits.Extensions
{
    public static class StreamBigEndian
    {
        public static int? ReadIntBigEndian(this Stream stream)
        {
            var buffer = new byte[4];
            if (stream.Read(buffer) < 4) return null;
            return BitConverter.ToInt32(buffer.Reverse().ToArray());
        }
    }
}