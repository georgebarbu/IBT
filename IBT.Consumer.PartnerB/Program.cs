using System;
using IBT.Messaging;
using Unity;

namespace IBT.Consumer.PartnerB
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("PartnerB Handler started");

            var container = new UnityContainer();
            ContainerInitializer.Initialize(container);

            var messageHandler = container.Resolve<IMessageHandler>();
            messageHandler.HandleMessages();

            Console.WriteLine("PartnerB Handler finished");
        }
    }
}
