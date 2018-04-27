using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace IBT.Producer
{
    public class FileProcessor : IMessageProcessor
    {
        private static FileSystemWatcher _fileSystemWatcher;
        private const string MessagesFilePath = @"D:\Vontobel\IncomingMessages";

        public void ProcessMessages()
        {
            try
            {
                MonitorDirectory(MessagesFilePath);


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

        private static void MonitorDirectory(string path)
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