using BallisticDB.Settings;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace BallisticDB.Services
{
    public class ExportService
    {
        private const int RIFLERECORDLENGTH = 151;
        private const int CARTRECORDLENGTH = 101;
        private string _rifleFileName;
        private string _riflePath;
        private string _cartridgePath;
        private readonly string _connectionString;
        private List<RifleData>? _rifles;
        private List<CartridgeData>? _cartridges;
        private DatabaseService _dbService;
        private IFilesService _filesService;

        public ExportService(IOptions<AppSettings> appSettings, DatabaseService databaseService, IFilesService filesService)
        {
            _rifleFileName = appSettings.Value.RifleFile;
            _riflePath = Path.Join(appSettings.Value.DbLocation, appSettings.Value.RifleFile);
            _cartridgePath = appSettings.Value.DbLocation;
            _connectionString = appSettings.Value.SQLitePrefix + appSettings.Value.DbLocation
                + "\\" + appSettings.Value.DbName
                + ";";
            _cartridges = new List<CartridgeData>();
            _rifles = new List<RifleData>();
            _dbService = databaseService;
            _filesService = filesService;
        }

        public async Task<bool> ExportDatabaseAsync()
        {
            if (_dbService != null)
            {
                var folder = await _filesService.OpenFolderPickerAsync();
                if (folder != null)
                {
                    _riflePath = Path.Join(folder, _rifleFileName);
                    _cartridgePath = folder;
                    _rifles = _dbService.LoadRifleData();
                    _cartridges = _dbService.LoadCartridgeData();
                    SerialiseRifles();
                    SerialiseCartridges();
                }
            }
            return true;
        }

        public void SerialiseRifles()
        {
            string rfl = string.Empty;
            foreach (var row in _rifles)
            {
                rfl += JsonSerializer.Serialize(row).PadRight(RIFLERECORDLENGTH, '\0');
            }

            using (StreamWriter sw = new StreamWriter(_riflePath))
            {
                sw.Write(rfl);
            }
        }

        public void SerialiseCartridges()
        {
            var rifles = _cartridges.Select(c=>c.rifleid).Distinct().ToList();
            foreach(var rifle in rifles)
            {
                var ct = string.Empty;
                var fileName = string.Format("{0}.dbb", rifle);
                var cPath = Path.Join(_cartridgePath, fileName);
                var ctgs = (from c in _cartridges where c.rifleid == rifle select c).ToList();

                foreach (var row in ctgs)
                {
                    ct += JsonSerializer.Serialize(row).PadRight(CARTRECORDLENGTH, '\0');
                }

                using (StreamWriter sw = new StreamWriter(cPath))
                {
                    sw.Write(ct);
                }
            }
        }
    }
}
