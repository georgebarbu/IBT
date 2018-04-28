using System;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using IBT.Messaging;
using Newtonsoft.Json;

namespace IBT.Router
{
    public class FileProcessor : IMessageProcessor
    {
        private static FileSystemWatcher _fileSystemWatcher;

        private const string MessagesFilePath = @"D:\Vontobel\IncomingMessages";

        private readonly IDatabaseProcessor _databaseProcessor;

        public FileProcessor(IDatabaseProcessor databaseProcessor)
        {
            _databaseProcessor = databaseProcessor;
        }

        public void ProcessMessages()
        {
            try
            {
                StartDirectoryMonitoring(MessagesFilePath);

                // wait...
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                _fileSystemWatcher?.Dispose();
            }
        }

        private void ProcessSingleMessage(XNode document)
        {
            SendToDb(document);
            SendToPartnerAQueue(document);
            SendToPartnerBQueue(document);
        }

        private void SendToPartnerBQueue(XNode document)
        {
            // check for EventType
            var eventType = document.XPathSelectElement("IBTTermSheet/Events/Event/EventType")?.Value;
            if("1" != eventType) return;

            var partnerBQueueName = System.Configuration.ConfigurationManager.AppSettings["PartnerBQueueName"];
            if (string.IsNullOrWhiteSpace(partnerBQueueName)) Console.WriteLine("Invalid PartnerB Queue Name");

            if (!MessageQueue.Exists(partnerBQueueName ?? throw new InvalidOperationException()))
            {
                var messageQueue = MessageQueue.Create(partnerBQueueName, true);
                using (messageQueue)
                {
                    var tx = new MessageQueueTransaction();
                    tx.Begin();
                    messageQueue.Send(document, tx);
                    tx.Commit();
                }
            }
        }

        private void SendToPartnerAQueue(XNode document)
        {
            var partnerAQueueName = System.Configuration.ConfigurationManager.AppSettings["PartnerAQueueName"];
            if (string.IsNullOrWhiteSpace(partnerAQueueName)) Console.WriteLine("Invalid Partner A Queue Name");

            if (!MessageQueue.Exists(partnerAQueueName ?? throw new InvalidOperationException()))
            {
                var messageQueue = MessageQueue.Create(partnerAQueueName, true);
                using (messageQueue)
                {
                    var tx = new MessageQueueTransaction();
                    tx.Begin();
                    messageQueue.Send(document.ToString(), tx);
                    tx.Commit();
                }
            }
        }

        private void SendToDb(XNode document)
        {
            var eventTypeValue = document.XPathSelectElement("/IBTTermSheet/Events/Event/EventType")?.Value;
            if(string.IsNullOrWhiteSpace(eventTypeValue)) return;
            var eventType = Int32.Parse(eventTypeValue);
            var dbMessage = new DbMessage
            {
                EventType = eventType,
                TimeStamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssffff")
            };

            //_databaseProcessor.PersistToDatabase(dbMessage);
        }

        private void StartDirectoryMonitoring(string path)
        {
            _fileSystemWatcher = new FileSystemWatcher {Path = path};

            _fileSystemWatcher.Created += FileSystemWatcher_Created;
            _fileSystemWatcher.EnableRaisingEvents = true;
        }

        private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.FullPath)) return;
            var xDocument = XDocument.Load(e.FullPath);
            ProcessSingleMessage(xDocument);

            //Task.Factory.StartNew(() =>
            //{
            //    if (string.IsNullOrWhiteSpace(e.FullPath)) return;
            //    var xDocument = XDocument.Load(e.FullPath);
            //    ProcessSingleMessage(xDocument);
            //});
        }
    }
}