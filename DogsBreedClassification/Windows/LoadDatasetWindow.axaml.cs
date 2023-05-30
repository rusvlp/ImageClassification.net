using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using DogsBreedClassification.Classification;

namespace DogsBreedClassification.Windows;

public partial class LoadDatasetWindow : Window
{
    private TextBox TrainPathTextBox;
    private TextBox TestPathTextBox;
    
    public LoadDatasetWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif

        this.TrainPathTextBox = this.FindControl<TextBox>("TrainDataPath");
        this.TestPathTextBox = this.FindControl<TextBox>("TestDataPath");
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    protected override void OnClosing(CancelEventArgs e)
    {
       // MainWindow.debugTb.Text = "OnClosing is called";
    
    }

    private void Close(object? sender, RoutedEventArgs e)
    {
        this.Hide();
    }

    private void LoadTestData(object? sender, RoutedEventArgs e)
    {
        string path = this.TestPathTextBox.Text;
        DataLoader.Instance.LoadTestData(path);
        Storage.isTestLoaded = true;

        if (Storage.isTestLoaded && Storage.isTrainLoaded)
        {
            MainWindow.Instance.learnButton.IsEnabled = true;
        }
    }

    private void LoadTrainData(object? sender, RoutedEventArgs e)
    {
        string path = this.TrainPathTextBox.Text;
        DataLoader.Instance.LoadImages(path);
        Storage.isTrainLoaded = true;
        
        if (Storage.isTestLoaded && Storage.isTrainLoaded)
        {
            MainWindow.Instance.learnButton.IsEnabled = true;
        }
    }
}