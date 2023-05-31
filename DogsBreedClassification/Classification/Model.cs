using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace DogsBreedClassification.Classification;

public class Model
{
    #region Singleton

    public static Model Instance;
    private Model(){}
    
    public static Model getInstance()
    {
        if (Instance == null)
            Instance = new Model();
        return Instance;
    }

    #endregion
    
    
    private TextBlock debug;
    private MLContext _mlContext;
    private ITransformer model;
    private IDataView trainingData;
    //private static DataViewSchema modelSchema;
    
    public void Condfigure()
    {
        MLContext mlContext = new MLContext();  // Общий контекст для всех операций ML.NET
        _mlContext = mlContext;
        model = GenerateModel(mlContext, "C:\\Users\\Vlad\\Documents\\ic_dataset\\archive\\train");
    }
    
    
    struct InceptionSettings
    {
        public const int ImageHeight = 224;
        public const int ImageWidth = 224;
        public const float Mean = 117;
        public const float Scale = 1;
        public const bool ChannelsLast = true;
    }
    
    
     ITransformer GenerateModel(MLContext mlContext, string _imagesFolder)
    {
        TextBlock debug = MainWindow.debugTb;
        //TextBlock learningTb = MainWindow.Instance.learningProcessTb;
        
        
        IEstimator<ITransformer> pipeline = mlContext.Transforms.LoadImages(outputColumnName: "input", imageFolder: _imagesFolder, inputColumnName: nameof(ImageData.ImagePath))
            // The image transforms transform the images into the model's expected format.
            .Append(mlContext.Transforms.ResizeImages(outputColumnName: "input", imageWidth: InceptionSettings.ImageWidth, imageHeight: InceptionSettings.ImageHeight, inputColumnName: "input"))
            .Append(mlContext.Transforms.ExtractPixels(outputColumnName: "input", interleavePixelColors: InceptionSettings.ChannelsLast, offsetImage: InceptionSettings.Mean))
            .Append(mlContext.Model.LoadTensorFlowModel("C:\\Users\\Vlad\\Documents\\ImageClassification\\ImageClassification.net\\assets\\tensorflow_inception_graph.pb").
                ScoreTensorFlowModel(outputColumnNames: new[] { "softmax2_pre_activation" }, inputColumnNames: new[] { "input" }, addBatchDimensionInput: true))
            .Append(mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "LabelKey", inputColumnName: "Label"))

            
            .Append(mlContext.MulticlassClassification.Trainers.LbfgsMaximumEntropy(labelColumnName: "LabelKey", featureColumnName: "softmax2_pre_activation"))
            .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabelValue", "PredictedLabel"))
            .AppendCacheCheckpoint(mlContext);

        trainingData = mlContext.Data.LoadFromEnumerable(DataLoader.Instance.ImageData);
        ITransformer model = pipeline.Fit(trainingData);

        IDataView testData = mlContext.Data.LoadFromEnumerable(DataLoader.Instance.TestImageData); 
        IDataView predictions = model.Transform(testData);
        
        
        MulticlassClassificationMetrics metrics =
            mlContext.MulticlassClassification.Evaluate(predictions,
                labelColumnName: "LabelKey",
                predictedLabelColumnName: "PredictedLabel"); 
        string logloss = ($"LogLoss is: {metrics.LogLoss}");
        string perclasslogloss =
            ($"PerClassLogLoss is: {String.Join(" , ", metrics.PerClassLogLoss.Select(c => c.ToString()))}");

        debug.Text += logloss + "\n" + perclasslogloss;
        
        //learningTb.Text = "Обучение завершено";
        
        return model;
    }
    
     /*
    void DisplayResults(IEnumerable<ImagePrediction> imagePredictionData)
    {
        string result = "";
        TextBlock debug = MainWindow.debugTb;
        foreach (ImagePrediction prediction in imagePredictionData)
        {
           
            result += $"Image: {Path.GetFileName(prediction.ImagePath)} predicted as: {prediction.PredictedLabelValue} with score: {prediction.Score?.Max()} ";
        }

        debug.Text = result;
    }
    */
    public string ClassifySingleImage(string path)
    {
        ImageData imageData = new ImageData()
        {
            ImagePath = path
        };
        
        PredictionEngine<ImageData, ImagePrediction> predictor = _mlContext.Model.CreatePredictionEngine<ImageData, ImagePrediction>(model);
        ImagePrediction prediction = predictor.Predict(imageData);
        TextBlock debug = MainWindow.debugTb;
        return $"Изображение: {Path.GetFileName(imageData.ImagePath)} является: {prediction.PredictedLabelValue} с результатом: {prediction.Score?.Max()} ";
    }

    public void SaveModel(string path)
    {
       // debug.Text = trainingData.Schema + "";
        _mlContext.Model.Save(model, trainingData.Schema, path);
    }

    public void LoadModel(string path)
    {
        DataViewSchema dvSchema;
        
        model = _mlContext.Model.Load(path, out DataViewSchema schema);
    }
}

