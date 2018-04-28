using System;
using Unity;

namespace IBT.Consumer.PartnerA
{
    class Program
    {
        static void Main()
        {
            var container = new UnityContainer();
            ContainerInitializer.Initialize(container);

            var messageHandler = container.Resolve<IMessageHandler>();
            messageHandler.HandleMessages();

            Console.WriteLine("PartnerA Handler finished");
        }
    }
}
