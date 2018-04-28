using IBT.Messaging;
using Unity;
using Unity.Lifetime;

namespace IBT.Consumer.PartnerB
{
    public static class ContainerInitializer
    {
        public static void Initialize(IUnityContainer container)
        {
            container.RegisterType<IFileService, FileService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IMessageHandler, PartnerBMessageHandler>();
        }
    }
}