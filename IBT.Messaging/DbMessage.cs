using System;

namespace IBT.Messaging
{
    [Serializable]
    public sealed class DbMessage
    {
        public int EventType { get; set; }
        public string TimeStamp { get; set; }
    }
}