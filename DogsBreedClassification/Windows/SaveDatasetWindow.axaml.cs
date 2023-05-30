using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using DogsBreedClassification.Classification;

namespace DogsBreedClassification.Windows;

public partial class SaveDatasetWindow : Window
{
    private TextBox savePathTb;
    
    public SaveDatasetWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif

        savePathTb = this.FindControl<TextBox>("SaveModelTextBox");

    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void SaveModel(object? sender, RoutedEventArgs e)
    {
        string path = savePathTb.Text;
        Model.SaveModel(path);
    }

    private void Close(object? sender, RoutedEventArgs e)
    {
       this.Hide();
    }
}