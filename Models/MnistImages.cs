using System;
using System.Collections.Generic;
using System.IO;
using Digits.Extensions;

namespace Digits.Models
{
    public class MnistImages : IDisposable
    {
        private Stream _stream;
        private int _numberOfImages;
        private int _rows;
        private int _columns;

        public MnistImages(string path)
        {
            _stream = File.OpenRead(path);

            var cookie = _stream.ReadIntBigEndian();
            if (cookie != 2051)
            {
                throw new ArgumentException("File is not in mnist data format");
            }

            _numberOfImages = _stream.ReadIntBigEndian() ?? 0;
            _rows = _stream.ReadIntBigEndian() ?? 0;
            _columns = _stream.ReadIntBigEndian() ?? 0;
        }

        public IEnumerable<byte[]> ReadImages()
        {
            while (true)
            {
                var image = new byte[_rows * _columns];
                if (_stream.Read(image) < _rows * _columns) break;
                yield return image;
            }
        }

        public int NumberOfImages => _numberOfImages;
        public int Rows => _rows;
        public int Cols => _columns;

        public void Dispose()
        {
            _stream?.Dispose();
        }
    }
}