namespace IBT.Producer
{
    public class MessageProcessor
    {
        private readonly IMessageProcessor _messageProcessor;

        public MessageProcessor(IMessageProcessor messageProcessor)
        {
            _messageProcessor = messageProcessor;
        }

        public void StartWork()
        {
            //var fileProcessor = container.Resolve<IMessageProcessor>("FileProcessor");
            //fileProcessor.ProcessMessages();

            _messageProcessor.ProcessMessages();
        }
    }
}