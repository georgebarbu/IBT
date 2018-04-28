using System;
using System.IO;
using System.Messaging;

namespace IBT.Producer
{
    public class FileProcessor : IMessageProcessor
    {
        private static FileSystemWatcher _fileSystemWatcher;

        private const string MessagesFilePath = @"D:\Vontobel\IncomingMessages";
        private const string PartnerAQueueName = "";
        private const string PartnerBQueueName = "";

        public void ProcessMessages()
        {
            try
            {
                InitializeQueues();

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

        /// <summary>
        /// Setup queues for the two partners
        /// </summary>
        private void InitializeQueues()
        {
            
        }

        private static void StartDirectoryMonitoring(string path)
        {
            _fileSystemWatcher = new FileSystemWatcher {Path = path};

            _fileSystemWatcher.Created += FileSystemWatcher_Created;
            _fileSystemWatcher.EnableRaisingEvents = true;
        }

        private static void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"File {e.Name} received");
            

        }
    }
}