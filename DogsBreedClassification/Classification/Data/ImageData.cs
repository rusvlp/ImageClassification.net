//using SciSharp.Tensorflow.Redist;

using Microsoft.ML.Data;
using Tensorflow;

namespace DogsBreedClassification.Classification;

public class ImageData
{
    [LoadColumn(0)]
    public string? ImagePath;
    [LoadColumn(1)]
    public string? Label;
    
}