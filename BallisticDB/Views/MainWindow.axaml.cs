using Avalonia.Controls;
using BallisticDB.Messages;
using BallisticDB.ViewModels;
using CommunityToolkit.Mvvm.Messaging;

namespace BallisticDB.Views;

public partial class MainWindow : Window
{
    readonly MainViewModel? _mvm;
    private bool _unsavedData = false;
    Dialog? _dialog;

    public MainWindow()
    {
        _dialog = null;
        InitializeComponent();
    }

    public MainWindow(MainViewModel mvm, Dialog dialog)
    {
        InitializeComponent();
        WeakReferenceMessenger.Default.Register<DataChangedMessage>(this, (r, m) =>
        {
            _unsavedData = m.Value.DataChanged;
        });
        _dialog = dialog;
        _dialog.DataContext = mvm;
        _mvm = mvm;
        main.DataContext = _mvm;
    }

    protected override void OnClosing(WindowClosingEventArgs e)
    {
        e.Cancel = true;
        _ = _dialog.ShowDialog(this);
        base.OnClosing(e);
    }
}
