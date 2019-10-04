using System;
using System.IO;
using System.Linq;
using Digits.Models;
using Microsoft.ML;

namespace Digits
{
    class Program
    {
        static void Main(string[] args)
        {
            var dir = args.Length == 1 ? args[0] : ".";
            
            var trainImagesPath = Path.Combine(dir, "train-images.idx3-ubyte");
            var trainLabelsPath = Path.Combine(dir, "train-labels.idx1-ubyte");
            var testImagesPath = Path.Combine(dir, "t10k-images.idx3-ubyte");
            var testLabelsPath = Path.Combine(dir, "t10k-labels.idx1-ubyte");
            
            if (!Directory.Exists(dir) || !File.Exists(trainImagesPath) || !File.Exists(trainLabelsPath)
                || !File.Exists(testImagesPath) || !File.Exists(testLabelsPath))
            {
                Console.WriteLine("Please provide the directory with downloaded training and test files as the first argument.");
                return;
            }
            
            try
            {
                var ctx = new MLContext();

                using var trainMnistData = new MnistData(trainImagesPath, trainLabelsPath);
                using var testMnistData = new MnistData(testImagesPath, testLabelsPath);
                
                var pipeline = ctx.Transforms.Conversion
                    .MapValueToKey(nameof(DataPoint.Label))
                    .Append(ctx.MulticlassClassification.Trainers.NaiveBayes());

                Console.WriteLine("Fitting the model");
                var trainData = ctx.Data.LoadFromEnumerable(trainMnistData.DataPoints().Take(10000));
                var model = pipeline.Fit(trainData);

                Console.WriteLine("Predicting");
                var testData = ctx.Data.LoadFromEnumerable(testMnistData.DataPoints());
                var predictions = ctx.Data.CreateEnumerable<Prediction>(model.Transform(testData), false)
                    .ToList();

                var correct = 0;
                var misses = 0;

                foreach (var prediction in predictions)
                {
                    if (prediction.Digit == prediction.Predicted) correct++;
                    else misses++;
                }

                Console.WriteLine($"Done: correct = {correct}, misses = {misses}: {1.0 * correct / (correct + misses)}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }
    }
}