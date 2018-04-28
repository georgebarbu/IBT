using System;

namespace IBT.Messaging
{
    [Serializable]
    public sealed class PartnerAMessage
    {
        public string ProductNameFull { get; set; }
        public int IbtTypeCode { get; set; }
        public int EventType { get; set; }
        public string Isin { get; set; }
    }
}
