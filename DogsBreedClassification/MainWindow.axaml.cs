using Avalonia.Controls;
using Avalonia.Interactivity;
using DogsBreedClassification.Classification;
using Microsoft.ML;
namespace DogsBreedClassification;

public partial class MainWindow : Window
{
    private TextBlock debugTb;
    private DataLoader dLoader = DataLoader.getInstance();

    private TextBox pathTb;
    public MainWindow()
    {
       
        InitializeComponent();
        debugTb = this.Find<TextBlock>("DebugTextBlock");
        debugTb.Text = "aasd";
        pathTb = this.FindControl<TextBox>("Path");
    }

    private void LoadTrainingImages(object? sender, RoutedEventArgs e)
    {
        dLoader.LoadImages(pathTb.Text);
    }
}