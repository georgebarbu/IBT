using System;
using System.Messaging;
using System.Xml.Linq;
using IBT.Messaging;

namespace IBT.Consumer.PartnerB
{
    public class PartnerBMessageHandler : IMessageHandler
    {
        private readonly IFileService _fileService;

        public PartnerBMessageHandler(IFileService fileService)
        {
            _fileService = fileService;
        }

        public void HandleMessages()
        {
            var partnerBQueueName = System.Configuration.ConfigurationManager.AppSettings["PartnerBQueueName"];
            if (string.IsNullOrWhiteSpace(partnerBQueueName)) Console.WriteLine("Invalid Partner B Queue Name");

            using (var messageQueue = new MessageQueue(partnerBQueueName, true))
                while (true)
                {
                    Console.WriteLine("Listening");
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
                    _fileService.PersistFile(instrumentNotification);
                }
        }

        private InstrumentNotification ProcessDocument(XDocument document)
        {
            throw new NotImplementedException();
        }
    }
}