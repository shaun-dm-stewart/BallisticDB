using Avalonia.Controls;
using BallisticDB.Messages;
using BallisticDB.ViewModels;
using CommunityToolkit.Mvvm.Messaging;

namespace BallisticDB.Views;

public partial class Dialog : Window
{
    readonly MainViewModel? _mvm;
    private bool _unsavedData = false;

    public Dialog()
    {
        InitializeComponent();
    }

    public Dialog(MainViewModel mvm)
    {
        InitializeComponent();
        dialog.DataContext = _mvm;
    }


    protected override void OnClosing(WindowClosingEventArgs e)
    {
        base.OnClosing(e);
    }
}
