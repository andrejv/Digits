using System.Linq;
using Microsoft.ML.Data;

namespace Digits.Models
{
    public class DataPoint
    {
        public DataPoint(byte label, byte[] image)
        {
            Label = (uint) label;
            Features = image.Select(b => b > 0x20 ? 1.0f : 0.0f).ToArray();
        }
        
        public uint Label { get; set; }
        [VectorType(784)]
        public float[] Features { get; set; }
    }

    public class Prediction
    {
        [ColumnName("Label")]
        public uint Digit { get; set; }
        [ColumnName("PredictedLabel")]
        public uint Predicted { get; set; }
    }
}