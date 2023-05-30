using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using DogsBreedClassification.Classification;

namespace DogsBreedClassification.Windows;

public partial class LoadModelWindow : Window
{
    public TextBox pathModelTextBox;
    
    public LoadModelWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif

        this.pathModelTextBox = this.FindControl<TextBox>("SaveModelTextBox");

    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void LoadModel(object? sender, RoutedEventArgs e)
    {
        string path = pathModelTextBox.Text;
        Model.LoadModel(path);
    }

    private void Close(object? sender, RoutedEventArgs e)
    {
       this.Hide();
    }
}