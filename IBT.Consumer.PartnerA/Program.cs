using System;
using IBT.Messaging;
using Unity;

namespace IBT.Consumer.PartnerA
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("PartnerA Handler started");

            var container = new UnityContainer();
            ContainerInitializer.Initialize(container);

            var messageHandler = container.Resolve<IMessageHandler>();
            messageHandler.HandleMessages();

            Console.WriteLine("PartnerA Handler finished");
        }
    }
}
