namespace BallisticDB.Settings
{
    public class AppSettings
    {
        public string AppName { get; set; } = string.Empty;
        public string SQLitePrefix { get; set; } = string.Empty;
        public string DbLocation { get; set; } = string.Empty;
        public string DbName { get; set; } = string.Empty;
        public string CartFile { get; set; } = string.Empty;
        public string RifleFile { get; set; } = string.Empty;
        public string LastUsed { get; set; } = string.Empty;
    }
}
