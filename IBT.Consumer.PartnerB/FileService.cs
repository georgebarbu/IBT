using System.Configuration;
using System.IO;
using IBT.Messaging;
using System.Xml.Serialization;

namespace IBT.Consumer.PartnerB
{
    public class FileService : IFileService
    {
        public void PersistFile(InstrumentNotification message)
        {
            var partnerBInbox = ConfigurationManager.AppSettings["PartnerBInbox"];
            var filePath = partnerBInbox + $"\\{message.TimeStamp}.xml";
            Save(message, filePath);
        }
    
        private void Save<T>(T file, string path)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, file);
            }
        }
    }
}