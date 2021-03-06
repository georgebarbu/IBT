﻿using System;
using System.Linq;
using System.Messaging;
using System.Xml.Linq;
using System.Xml.XPath;
using IBT.Messaging;

namespace IBT.Consumer.PartnerA
{
    public class PartnerAMessageHandler : IMessageHandler
    {
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(PartnerAMessageHandler));

        private readonly IEmailService _emailService;

        public PartnerAMessageHandler(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public void HandleMessages()
        {
            var partnerAQueueName = System.Configuration.ConfigurationManager.AppSettings["PartnerAQueueName"];
            if (string.IsNullOrWhiteSpace(partnerAQueueName)) _log.Info("Invalid Partner A Queue Name");

            using (var messageQueue = new MessageQueue(partnerAQueueName, true))
                while (true)
                {
                    _log.Info("Listening for partner A messages");
                    XDocument document;
                    using (var tx = new MessageQueueTransaction())
                    {
                        tx.Begin();
                        var message = messageQueue.Receive(tx);
                        message.Formatter = new XmlMessageFormatter(new[] {"System.String, mscorlib"});
                        document = XDocument.Parse(message.Body as string ?? throw new InvalidOperationException());
                        tx.Commit();
                    }

                    var partnerAMessage = ProcessDocument(document);
                    _emailService.SendEmail(partnerAMessage);
                }
        }

        private PartnerAMessage ProcessDocument(XNode document)
        {
            var productNameFull = document.XPathSelectElement("IBTTermSheet/Instrument/ProductNameFull")?.Value;
            var ibtTypeCode = document.XPathSelectElement("IBTTermSheet/Instrument/IBTTypeCode")?.Value;
            var eventType = document.XPathSelectElement("IBTTermSheet/Events/Event/EventType")?.Value;
            var isin = ExtractIsin(document);

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

            return new PartnerAMessage
            {
                EventType = Convert.ToInt32(eventType),
                ProductNameFull = productNameFull,
                IbtTypeCode = Convert.ToInt32(ibtTypeCode),
                Isin = isin
            };
        }



    }
}