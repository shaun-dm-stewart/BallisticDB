using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;

using BallisticDB.ViewModels;
using BallisticDB.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System;
using BallisticDB.Settings;
using BallisticDB.Services;

namespace BallisticDB;

public partial class App : Application
{
    public IServiceProvider? ServiceProvider { get; private set; }
    public IConfiguration? Configuration { get; private set; }
    public new static App? Current => Application.Current as App;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Line below is needed to remove Avalonia data validation.
            // Without this line you will get duplicate validations from both Avalonia and CT
            BindingPlugins.DataValidators.RemoveAt(0);

            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: false);

            Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            
            ConfigureServices(serviceCollection);
            
            ServiceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();

            desktop.MainWindow = mainWindow;
        }
        base.OnFrameworkInitializationCompleted();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.Configure<AppSettings>(Configuration.GetSection(nameof(AppSettings)));
        services.AddSingleton<MainWindow>();
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<DatabaseService>();
        services.AddSingleton<ExportService>();
        services.AddSingleton<IFilesService, FilesService>();
    }
}
