using IBT.Messaging;

namespace IBT.Consumer.PartnerB
{
    public class FileService : IFileService
    {
        public void PersistFile(InstrumentNotification message)
        {
            throw new System.NotImplementedException();
        }
    }

    public interface IFileService
    {
        void PersistFile(InstrumentNotification message);
    }
}