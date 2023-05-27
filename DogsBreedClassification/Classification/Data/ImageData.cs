//using SciSharp.Tensorflow.Redist;

using Tensorflow;

namespace DogsBreedClassification.Classification;

public class ImageData
{
    public string? ImagePath;
    public string? Label;
    public Tensor imageTensor;
}