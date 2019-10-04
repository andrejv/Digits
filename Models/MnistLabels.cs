using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Digits.Extensions;

namespace Digits.Models
{
    public class MnistLabels : IDisposable
    {
        private Stream _stream;
        private int _numberOfLabels;

        public MnistLabels(string path)
        {
            _stream = File.OpenRead(path);

            var cookie = _stream.ReadIntBigEndian();
            if (cookie != 2049)
            {
                throw new InvalidEnumArgumentException("File not in mnist labels format");
            }

            _numberOfLabels = _stream.ReadIntBigEndian() ?? 0;
        }

        public IEnumerable<byte> GetLabels()
        {
            while (true)
            {
                var buffer = new byte[1];
                if (_stream.Read(buffer) < 1) break;
                yield return buffer[0];
            }
        }

        public int NumberOfLabels => _numberOfLabels;

        public void Dispose()
        {
            _stream?.Dispose();
        }
    }
}