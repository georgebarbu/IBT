using System;

namespace IBT.Messaging
{
    [Serializable]
    public sealed class InstrumentNotification
    {
        public string TimeStamp { get; set; }
        public string Isin { get; set; }
    }
}