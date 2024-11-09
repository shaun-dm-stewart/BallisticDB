using Avalonia.Controls;
using BallisticDB.ViewModels;

namespace BallisticDB.Views;

public partial class MainWindow : Window
{
    readonly MainViewModel? _mvm;

    public MainWindow()
    {
        InitializeComponent();
    }
    public MainWindow(MainViewModel mvm)
    {
        InitializeComponent();
        _mvm = mvm;
        tit.DataContext = _mvm;
    }
}
