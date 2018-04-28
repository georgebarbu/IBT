using IBT.Messaging;
using Unity;
using Unity.Lifetime;

namespace IBT.Consumer.PartnerA
{
    public static class ContainerInitializer
    {
        public static void Initialize(IUnityContainer container)
        {
            container.RegisterType<IEmailService, EmailService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IMessageHandler, PartnerAMessageHandler>();
        }
    }
}