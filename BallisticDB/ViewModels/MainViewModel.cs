using BallisticDB.Messages;
using BallisticDB.Services;
using BallisticDB.Settings;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
namespace BallisticDB.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly AppSettings? _settings;
    private readonly DatabaseService? _dbService;
    private readonly ExportService? _exportService;

    [ObservableProperty]
    private string? _title;
    [ObservableProperty]
    public RifleViewModel? _selectedRifle;
    [ObservableProperty]
    public CartridgeViewModel? _selectedCartridge;
    [ObservableProperty]
    public ObservableCollection<RifleViewModel>? _rifles;
    [ObservableProperty]
    private ObservableCollection<CartridgeViewModel>? _cartridges;

    public MainViewModel() 
    {
    }

    public MainViewModel(IOptions<AppSettings> settings, DatabaseService dbService, ExportService exportService)
    {
        _settings = settings.Value;
        _dbService = dbService;
        _exportService = exportService;
        Title = _settings.AppName;
        InitialiseDatabase();
    }

    private void InitialiseDatabase()
    {
        _dbService.OpenDatabase();
        Rifles = new ObservableCollection<RifleViewModel>(LoadRifles());
        Cartridges = new ObservableCollection<CartridgeViewModel>();
        _dbService.CloseDatabase();
    }

    private List<RifleViewModel> LoadRifles()
    {
        return _dbService.LoadRifleRecords();
    }

    private List<CartridgeViewModel>LoadCartridges()
    {
        return _dbService.GetCartridgesByRifleId(SelectedRifle);
    }

    partial void OnSelectedRifleChanged(RifleViewModel? value)
    {
        Cartridges = new ObservableCollection<CartridgeViewModel>(_dbService.GetCartridgesByRifleId(value));
    }

    [RelayCommand]
    internal async Task ExportBallisticsData()
    {
        _dbService.OpenDatabase();
        await _exportService.ExportDatabaseAsync();
        _dbService.CloseDatabase();
    }

    [RelayCommand]
    internal void AddRifle()
    {
        var r = new RifleViewModel();
        r.RifleId = 0;
        r.RifleName = "New";
        r = _dbService.AddRifle(r);
        Rifles.Add(r);
    }

    [RelayCommand]
    internal void RemoveRifle()
    {
        _dbService.DeleteRifle(SelectedRifle);
        Rifles.Remove(SelectedRifle);
    }

    [RelayCommand]
    internal void Save()
    {
        _dbService.OpenDatabase();
        _dbService.Save();
        _dbService.CloseDatabase();
    }

    [RelayCommand]
    internal void AddCartridge()
    {
        var c = new CartridgeViewModel();
        c.RifleId = SelectedRifle.RifleId;
        c.CartridgeName = "New";
        _dbService.AddCartridge(c);
        Cartridges.Add(c);
    }

    [RelayCommand]
    internal void RemoveCartridge()
    {
        _dbService.DeleteCartridge(SelectedCartridge);
        Cartridges.Remove(SelectedCartridge);
    }

    bool FilterCartridges(object obj)
    {
        if (obj is CartridgeViewModel ct)
        {
            return ct.RifleId == _selectedRifle.RifleId && ct.RowState != RowStatus.DELETED;
        }
        return false;
    }

    bool FilterRifles(object obj)
    {
        if (obj is RifleViewModel rfl)
        {
            return rfl.RowState != RowStatus.DELETED;
        }
        return false;
    }
}
