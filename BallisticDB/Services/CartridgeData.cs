namespace BallisticDB.Services
{
    public class CartridgeData
    {
        public long id { get; set; }
        public long rifleid { get; set; }
        public string desc { get; set; } = string.Empty;
        public double wt { get; set; }
        public double mv { get; set; }
        public double bc { get; set; }
        public double bl { get; set; }
        public double clbr { get; set; }
    }
}