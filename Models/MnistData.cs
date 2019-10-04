using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Digits.Models
{
    public class MnistData: IDisposable
    {
        private readonly MnistImages _images;
        private readonly MnistLabels _labels;

        public MnistData(string imagesPath, string labelsPath)
        {
            _images = new MnistImages(imagesPath);
            _labels = new MnistLabels(labelsPath);
        }
        public IEnumerable<DataPoint> DataPoints()
        {
            return _images.ReadImages()
                .Zip(_labels.GetLabels(), (image, label) => new DataPoint(label, image)); 
        }

        public void Dispose()
        {
            _images?.Dispose();
            _labels?.Dispose();
        }
    }
}