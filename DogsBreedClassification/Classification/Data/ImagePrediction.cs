using System.Drawing;

namespace DogsBreedClassification.Classification;

public class ImagePrediction : ImageData
{
    public float[]? Score;

    public string? PredictedLabelValue;
}