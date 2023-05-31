using System;
using System.IO;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using DogsBreedClassification.Classification;
using DogsBreedClassification.Windows;
using Microsoft.ML;




namespace DogsBreedClassification;
/*
string _assetsPath = Path.Combine(Environment.CurrentDirectory, "assets");
string _imagesFolder = Path.Combine(_assetsPath, "images");
string _trainTagsTsv = Path.Combine(_imagesFolder, "tags.tsv");
string _testTagsTsv = Path.Combine(_imagesFolder, "test-tags.tsv");
string _predictSingleImage = Path.Combine(_imagesFolder, "toaster3.jpg");
string _inceptionTensorFlowModel = Path.Combine(_assetsPath, "inception", "tensorflow_inception_graph.pb");
*/

public partial class MainWindow : Window
{
    public static TextBlock debugTb;
    public TextBlock learningProcessTb;
    public static TextBlock resultTb;
    private DataLoader dLoader = DataLoader.getInstance();

    public Window LoadDatasetWindow;
    public Window SaveDatasetWindow;
    public Window LoadModelWindow;

    public TextBox classifyPathTb;
    public TextBox testPath;

    public Grid imgGrid;
    public Image loadedImage;
    
    public Button classifyButton;
    public Button learnButton;

    public string imagePath;

    private TextBox pathTb;

    public static MainWindow Instance;

    public MainWindow()
    {
        LoadDatasetWindow = new LoadDatasetWindow();
        InitializeComponent();
        debugTb = this.Find<TextBlock>("DebugTextBlock");
        //debugTb.Text = "aasd";
        pathTb = this.FindControl<TextBox>("Path");

        this.testPath = this.FindControl<TextBox>("TestPath");

        classifyPathTb = this.FindControl<TextBox>("ClassifyPath");
        classifyButton = this.FindControl<Button>("ClassifyButton");
        // classifyButton.IsEnabled = false;
        learnButton = this.FindControl<Button>("LearnBtn");
        learnButton.IsEnabled = false;
        Instance = this;

        learningProcessTb = this.FindControl<TextBlock>("IsLearning");

        SaveDatasetWindow = new SaveDatasetWindow();
        LoadModelWindow = new LoadModelWindow();

        loadedImage = this.FindControl<Image>("LoadedImages");
        
        imgGrid = this.FindControl<Grid>("ImageGrid");
        imgGrid.IsVisible = false;

        resultTb = this.FindControl<TextBlock>("ClassificationResult");
    }

    private void LoadTrainingImages(object? sender, RoutedEventArgs e)
    {
        dLoader.LoadImages(pathTb.Text);
    }

    private void LoadTestImages(object? sender, RoutedEventArgs e)
    {
        dLoader.LoadTestData(testPath.Text);
    }

    private void Learn(object? sender, RoutedEventArgs e)
    {
        learningProcessTb.Text = "Идет процесс обучения...";
        int milliseconds = 500;
        Thread.Sleep(milliseconds);

        Model.getInstance().Condfigure();
        learningProcessTb.Text = "Обучение завершено...";
    }

    private void LoadImage(object? sender, RoutedEventArgs e)
    {
        string path = classifyPathTb.Text;
        imagePath = path;
        Bitmap bitmap = new Bitmap(path);
        
        
        loadedImage.Source = bitmap;
        imgGrid.IsVisible = true;
    }

    private void Classify(object? sender, RoutedEventArgs e)
    {
        resultTb.Text = Model.getInstance().ClassifySingleImage(classifyPathTb.Text);
    }

    private void OpenSelectDatasetWindow(object? sender, RoutedEventArgs e)
    {
        LoadDatasetWindow.Show();
    }

    private void SaveModel(object? sender, RoutedEventArgs e)
    {
        SaveDatasetWindow.Show();
    }

    private void LoadModel(object? sender, RoutedEventArgs e)
    {
        LoadModelWindow.Show();
    }
    
    
}