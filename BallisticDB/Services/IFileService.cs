using Avalonia.Platform.Storage;
using System.Threading.Tasks;

namespace BallisticDB.Services;

public interface IFilesService
{
    public Task<string?> OpenFolderPickerAsync();
}