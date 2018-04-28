namespace IBT.Messaging
{
    public sealed class DbMessage
    {
        public DbMessage(int eventType, string timeStamp)
        {
            EventType = eventType;
            TimeStamp = timeStamp;
        }

        public int EventType { get; private set; }
        public string TimeStamp { get; private set; }
    }
}