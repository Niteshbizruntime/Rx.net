

namespace Apache.Ignite.Examples.Compute
{
    using System;
    using Apache.Ignite.Core;
    using Apache.Ignite.Core.Compute;

    public class PeerAssemblyLoadingExample
    {
       
        [STAThread]
        public static void Main()
        {
            using (var ignite = Ignition.StartFromApplicationConfiguration())
            {
                Console.WriteLine();
                Console.WriteLine(">>> Peer loading example started.");

                var remotes = ignite.GetCluster().ForRemotes();

                if (remotes.GetNodes().Count == 0)
                {
                    throw new Exception("This example requires remote nodes to be started. " +
                                        "Please start at least 1 remote node. " +
                                        "Refer to example's documentation for details on configuration.");
                }

                Console.WriteLine(">>> Executing an action on all remote nodes...");

             
                remotes.GetCompute().Broadcast(new HelloAction());

                Console.WriteLine(">>> Action executed, check output on remote nodes.");
            }

            Console.WriteLine();
            Console.WriteLine(">>> Example finished, press any key to exit ...");
            Console.ReadKey();
        }

        private class HelloAction : IComputeAction
        {
          
            public void Invoke()
            {
                Console.WriteLine("Hello from automatically deployed assembly! Version is " +
                                  GetType().Assembly.GetName().Version);
            }
        }
    }
}
