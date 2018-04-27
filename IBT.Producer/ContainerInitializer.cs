using Unity;

namespace IBT.Producer
{
    public static class ContainerInitializer
    {
        public static void Initialize(IUnityContainer container)
        {
            container.RegisterType<IMessageProcessor, FileProcessor>("FileProcessor");
        }
    }
}