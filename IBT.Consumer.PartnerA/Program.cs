using System;
using IBT.Messaging;
using Unity;

namespace IBT.Consumer.PartnerA
{
    class Program
    {
        private static readonly log4net.ILog Log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main()
        {
            Log.Info("PartnerA Handler started");

            var container = new UnityContainer();
            ContainerInitializer.Initialize(container);

            var messageHandler = container.Resolve<IMessageHandler>();
            messageHandler.HandleMessages();

            Log.Info("PartnerA Handler finished");
        }
    }
}
