using System;
using Unity;

namespace IBT.Processor
{
    class Program
    {
        static void Main()
        {
            var container = new UnityContainer();
            ContainerInitializer.Initialize(container);

            var messageProcessorType = System.Configuration.ConfigurationManager.AppSettings["MessageProcessorType"];
            if (Enum.TryParse(messageProcessorType, true, out ProcessorType processorType))
            {
                var processor = container.Resolve<IMessageProcessor>(processorType.ToString());
                processor?.ProcessMessages();
            }


        }
    }
}
