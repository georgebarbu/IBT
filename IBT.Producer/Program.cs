﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace IBT.Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new UnityContainer();
            ContainerInitializer.Initialize(container);

            var fileProcessor = container.Resolve<IMessageProcessor>("FileProcessor");
            fileProcessor.ProcessMessages();


        }
    }
}
