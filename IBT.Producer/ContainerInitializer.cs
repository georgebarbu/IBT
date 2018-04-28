using Unity;

namespace IBT.Processor
{
    public static class ContainerInitializer
    {
        public static void Initialize(IUnityContainer container)
        {
            container.RegisterType<IMessageProcessor, FileProcessor>("File");
            container.RegisterType<IMessageProcessor, MSMQProcessor>("Msmq");
        }
    }
}