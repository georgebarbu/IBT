using System;
using System.Configuration;
using System.Messaging;
using Unity;

namespace IBT.Router
{
    class Program
    {
        static void Main()
        {
            var container = new UnityContainer();
            ContainerInitializer.Initialize(container);

            InitializeQueues();

            var messageProcessorType = ConfigurationManager.AppSettings["MessageProcessorType"];
            if (Enum.TryParse(messageProcessorType, true, out ProcessorType processorType))
            {
                var processor = container.Resolve<IMessageProcessor>(processorType.ToString());

                processor?.ProcessMessages();
            }


        }

        private static void InitializeQueues()
        {
            var partnerAQueue = ConfigurationManager.AppSettings["PartnerAQueueName"];
            if (!MessageQueue.Exists(partnerAQueue))
                MessageQueue.Create(partnerAQueue, true);

            var partnerBQueue = ConfigurationManager.AppSettings["PartnerBQueueName"];
            if (!MessageQueue.Exists(partnerBQueue))
                MessageQueue.Create(partnerBQueue, true);

        }
    }
}
