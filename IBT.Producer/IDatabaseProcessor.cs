using IBT.Messaging;

namespace IBT.Router
{
    public interface IDatabaseProcessor
    {
        void PersistToDatabase(DbMessage message);
    }
}