using IBT.Messaging;

namespace IBT.Consumer.PartnerB
{
    public interface IFileService
    {
        void PersistFile(InstrumentNotification message);
    }
}