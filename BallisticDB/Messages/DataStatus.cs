namespace BallisticDB.Messages
{
    public class DataStatus
    {
        public DataStatus()
        { 
        }
        public DataStatus(bool dataChanged, string msg) 
        { 
            DataChanged = dataChanged;
            Message = msg;
        }
        public bool DataChanged { get; }
        public string Message { get; } = string.Empty;
    }
}
