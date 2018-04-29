using System;
using System.Linq;
using System.Messaging;
using System.Xml.Linq;
using System.Xml.XPath;
using IBT.Messaging;

namespace IBT.Consumer.PartnerB
{
    public class PartnerBMessageHandler : IMessageHandler
    {
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(PartnerBMessageHandler));
        private readonly IFileService _fileService;

        public PartnerBMessageHandler(IFileService fileService)
        {
            _fileService = fileService;
        }

        public void HandleMessages()
        {
            var partnerBQueueName = System.Configuration.ConfigurationManager.AppSettings["PartnerBQueueName"];
            if (string.IsNullOrWhiteSpace(partnerBQueueName)) _log.Info("Invalid Partner B Queue Name");

            using (var messageQueue = new MessageQueue(partnerBQueueName, true))
                while (true)
                {
                    _log.Info("Listening for partner B messages");
                    XDocument document;
                    using (var tx = new MessageQueueTransaction())
                    {
                        tx.Begin();
                        var message = messageQueue.Receive(tx);
                        message.Formatter = new XmlMessageFormatter(new[] { "System.String, mscorlib" });
                        document = XDocument.Parse(message.Body as string ?? throw new InvalidOperationException());
                        tx.Commit();
                    }

                    var instrumentNotification = ProcessDocument(document);
                    if (!string.IsNullOrWhiteSpace(instrumentNotification.Isin))
                        _fileService.PersistFile(instrumentNotification);
                }
        }

        private InstrumentNotification ProcessDocument(XNode document)
        {
            var isin = ExtractIsin(document);
            var timeStamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssffff");

            string ExtractIsin(XNode xDocument)
            {
                var instruments = xDocument.XPathSelectElement("IBTTermSheet/Instrument/InstrumentIds");
                if (null == instruments) return string.Empty;
                foreach (var instrumentId in instruments.Descendants())
                {
                    var idSchemeCode = instrumentId.XPathSelectElement("IdSchemeCode");
                    if (null == idSchemeCode) continue;
                    if (idSchemeCode.Value != "I-") continue;

                    var isinNode = instrumentId.DescendantNodes().Last();
                    return isinNode.ToString();
                }

                return string.Empty;
            }

            return new InstrumentNotification
            {
                Isin = isin,
                TimeStamp = timeStamp
            };
        }
    }
}