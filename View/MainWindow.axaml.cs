// IncokerApp (c) 2025 MIT License <baltasarq@gmail.com>


namespace InvokerApp.View;

using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Dialogs;
using Avalonia.Platform.Storage;

using InvokerApp.Core;


public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var btLaunch = this.GetControl<Button>( "BtLaunch" );
        var btEdSaveInput = this.GetControl<Button>( "BtEdOpenInput" );
        var btEdSaveOutput = this.GetControl<Button>( "BtEdOpenOutput" );

        btLaunch.Click += (_, _) => this.Launch();
        btEdSaveInput.Click += (_, _) => this.SetInputFile();
        btEdSaveOutput.Click += (_, _) => this.SetOutputFile();
    }

    public string InputFilePath { get; private set; } = "";
    public string OutputFilePath { get; private set; } = "";

    private void Launch()
    {
        if ( !string.IsNullOrWhiteSpace( this.InputFilePath )) {
            var edExe = this.GetControl<TextBox>( "EdExe" );
            var edArgs = this.GetControl<TextBox>( "EdArgs" );
            var edInput = this.GetControl<TextBox>( "EdInput" );
            var edOutput = this.GetControl<SelectableTextBlock>( "EdOutput" );
            var tabs = this.GetControl<TabControl>( "Tabs" );
            var launcher = new CmdLauncher{
                                Exe = ( edExe.Text ?? "" ).Trim(),
                                Args = ( edArgs.Text ?? "" ).Split( ";" )  };

            File.WriteAllText( this.InputFilePath, edInput.Text );
            launcher.DoIt( this.InputFilePath, this.OutputFilePath );
            edOutput.Text = launcher.Output;
            tabs.SelectedIndex = 1;
        }
    }

    private void SetInputFile()
    {
        this.StoreSaveFilePath(
                "Save input as...",
                "EdInputFile",
                (path) => this.InputFilePath = path );
    }

    private void SetOutputFile()
    {
        this.StoreSaveFilePath(
                "Output will be saved as...",
                "EdOutputFile",
                (path) => this.OutputFilePath = path );
    }

    private async void StoreSaveFilePath(string title, string ctrlName, Action<string> saveIt)
    {
        var filePath = await this.StorageProvider.SaveFilePickerAsync(
            new FilePickerSaveOptions {
                Title = title,
                SuggestedFileName = "input.txt",
                FileTypeChoices = [ FilePickerFileTypes.TextPlain, FilePickerFileTypes.All ]
            });

        if ( filePath is not null ) {
            var ctrl = this.GetControl<SelectableTextBlock>( ctrlName );
            string path = filePath.TryGetLocalPath() ?? "";

            ctrl.Text = path;
            saveIt( path );
        }
    }
}
