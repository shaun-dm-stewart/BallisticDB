using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using System;
using System.Threading.Tasks;

namespace BallisticDB.Services;

public class FilesService : IFilesService
{
    public async Task<string?> OpenFolderPickerAsync()
    {
        if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
            desktop.MainWindow?.StorageProvider is not { } provider)
            throw new NullReferenceException("Missing StorageProvider instance.");

        var folders = await provider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
        {
            Title = "Select Folder",
            AllowMultiple = false
        });

        return folders?.Count >= 1 ? folders[0].Path.AbsolutePath : null;
    }
}