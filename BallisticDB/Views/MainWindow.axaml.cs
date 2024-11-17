using Avalonia.Controls;
using BallisticDB.Messages;
using BallisticDB.ViewModels;
using CommunityToolkit.Mvvm.Messaging;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using System.Threading.Tasks;

namespace BallisticDB.Views;

public partial class MainWindow : Window
{
    readonly MainViewModel? _mvm;
    private bool _unsavedData = false;
    private bool _forceClosing = false;

    public MainWindow()
    {
        InitializeComponent();
    }

    public MainWindow(MainViewModel mvm)
    {
        InitializeComponent();
        WeakReferenceMessenger.Default.Register<DataChangedMessage>(this, (r, m) =>
        {
            _unsavedData = m.Value.DataChanged;
        });
        _mvm = mvm;
        main.DataContext = _mvm;
    }

    protected override async void OnClosing(WindowClosingEventArgs e)
    {
        if (_unsavedData)
        {
            if (!_forceClosing)
            {
                e.Cancel = true;
                var box = MessageBoxManager
                          .GetMessageBoxStandard("BallisticDB", "You have unsaved Changes.  Do you wish to save them?",
                              ButtonEnum.YesNoCancel);

                await box.ShowAsync().ContinueWith(t =>
                {
                    switch(t.Result)
                    {
                        case ButtonResult.Yes:
                            _mvm.Save();
                            _forceClosing = true;
                            this.Close(); 
                            break;
                        case ButtonResult.No:
                            _forceClosing = true;
                            this.Close();
                            break;
                        case ButtonResult.Cancel:
                            e.Cancel = true;
                            break;
                        default:
                            break;
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }
    }
}

