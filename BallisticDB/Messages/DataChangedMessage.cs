using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BallisticDB.Messages
{
    public class DataChangedMessage : ValueChangedMessage<string>
    {
        public DataChangedMessage(string msg) : base(msg)
        {
        }
    }
}
