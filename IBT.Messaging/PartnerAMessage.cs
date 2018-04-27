using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBT.Messaging
{
    public sealed class PartnerAMessage
    {
        public string ProductNameFull { get; set; }
        public int IBTTypeCode { get; set; }
        public int EventType { get; set; }
        public string ISIN { get; set; }
    }
}
