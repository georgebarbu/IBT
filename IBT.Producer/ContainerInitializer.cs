using Unity;
using Unity.Lifetime;

namespace IBT.Router
{
    public static class ContainerInitializer
    {
        public static void Initialize(IUnityContainer container)
        {
            container.RegisterType<IDatabaseProcessor, DatabaseProcessor>(new ContainerControlledLifetimeManager());
            container.RegisterType<IMessageProcessor, FileProcessor>("File");
            container.RegisterType<IMessageProcessor, MSMQProcessor>("Msmq");
        }
    }
}