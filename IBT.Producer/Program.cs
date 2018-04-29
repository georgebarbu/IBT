using System;
using System.Configuration;
using System.Messaging;
using Unity;

namespace IBT.Router
{
    class Program
    {
        private static readonly log4net.ILog Log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main()
        {
            var container = new UnityContainer();
            ContainerInitializer.Initialize(container);

            Log.Info("Checking queues");
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
            {
                MessageQueue.Create(partnerAQueue, true);
                Log.InfoFormat("Queue {0} created", partnerAQueue);
            }

            var partnerBQueue = ConfigurationManager.AppSettings["PartnerBQueueName"];
            if (!MessageQueue.Exists(partnerBQueue))
            {
                MessageQueue.Create(partnerBQueue, true);
                Log.InfoFormat("Queue {0} created", partnerBQueue);
            }
        }
    }
}
