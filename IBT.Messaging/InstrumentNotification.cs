using System;

namespace IBT.Messaging
{
    [Serializable]
    public sealed class InstrumentNotification
    {
        public double TimeSpan { get; set; }
        public string Isin { get; set; }
    }
}