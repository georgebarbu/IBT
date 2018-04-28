using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using IBT.Messaging;

namespace IBT.Processor
{
    public class FileProcessor : IMessageProcessor
    {
        private static FileSystemWatcher _fileSystemWatcher;

        private const string MessagesFilePath = @"D:\Vontobel\IncomingMessages";
        private const string PartnerAQueue = "";
        private const string PartnerBQueue = "";
        private const string DBQueue = "";

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

        private void ProcessSingleMessage(XDocument document)
        {
            SendToDB(document);
            SendToPartnerAQueue(document);
            SendToPartnerBQueue(document);
        }

        private void SendToPartnerBQueue(XDocument document)
        {
            throw new NotImplementedException();
        }

        private void SendToPartnerAQueue(XDocument document)
        {
            
        }

        private void SendToDB(XDocument document)
        {
            var value = (IEnumerable) document.XPathEvaluate("/IBTTermSheet/Events/Event/EventType");
            foreach (XElement element in value) // should be just one element
            {
                var eventType = Int32.Parse(element.Value);
                var dbMessage = new DbMessage(eventType, DateTime.UtcNow.ToString("yyyyMMddHHmmssffff"));
                _databaseProcessor.PersistToDatabase(dbMessage);
                break;
            }
        }

        private void StartDirectoryMonitoring(string path)
        {
            _fileSystemWatcher = new FileSystemWatcher {Path = path};

            _fileSystemWatcher.Created += FileSystemWatcher_Created;
            _fileSystemWatcher.EnableRaisingEvents = true;
        }

        private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                if (string.IsNullOrWhiteSpace(e.FullPath)) return;
                var xDocument = XDocument.Load(e.FullPath);
                ProcessSingleMessage(xDocument);
            });
        }
    }
}