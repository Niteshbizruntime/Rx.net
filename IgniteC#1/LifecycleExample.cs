    using System;
    using Apache.Ignite.Core;
    using Apache.Ignite.Core.Discovery.Tcp;
    using Apache.Ignite.Core.Discovery.Tcp.Static;
    using Apache.Ignite.Core.Lifecycle;
    using Apache.Ignite.Core.Resource;

namespace Apache.Ignite.Examples.Misc
{
    

   
    public class LifecycleExample
    {
        /// <summary>
        /// Runs the example.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Console.WriteLine();
            Console.WriteLine(">>> Lifecycle example started.");

          
            var lifecycleAwareExample = new LifecycleHandlerExample();

            var cfg = new IgniteConfiguration
            {
                DiscoverySpi = new TcpDiscoverySpi
                {
                    IpFinder = new TcpDiscoveryStaticIpFinder
                    {
                        Endpoints = new[] {"127.0.0.1:47500"}
                    }
                },
                LifecycleHandlers = new[] {lifecycleAwareExample}
            };

          
            using (Ignition.Start(cfg))
            {
               
                Console.WriteLine();
                Console.WriteLine(">>> Started (should be true): " + lifecycleAwareExample.Started);
            }

           
            Console.WriteLine();
            Console.WriteLine(">>> Started (should be false): " + lifecycleAwareExample.Started);

            Console.WriteLine();
            Console.WriteLine(">>> Example finished, press any key to exit ...");
            Console.ReadKey();
        }

       
        private class LifecycleHandlerExample : ILifecycleHandler
        {
            
            [InstanceResource]
#pragma warning disable 649
            private IIgnite _ignite;
#pragma warning restore 649

            
            public void OnLifecycleEvent(LifecycleEventType evt)
            {
                Console.WriteLine();
                Console.WriteLine(">>> Ignite lifecycle event occurred: " + evt);
                Console.WriteLine(">>> Ignite name: " + (_ignite != null ? _ignite.Name : "not available"));

                if (evt == LifecycleEventType.AfterNodeStart)
                    Started = true;
                else if (evt == LifecycleEventType.AfterNodeStop)
                    Started = false;
            }

           
            public bool Started
            {
                get;
                private set;
            }
        }
    }
}
