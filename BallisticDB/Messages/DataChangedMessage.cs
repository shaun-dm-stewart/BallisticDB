using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BallisticDB.Messages
{
    public class DataChangedMessage : ValueChangedMessage<DataStatus>
    {
        public DataChangedMessage(DataStatus msg) : base(msg)
        {
        }
    }
}
