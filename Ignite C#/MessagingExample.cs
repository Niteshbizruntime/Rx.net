

namespace Apache.Ignite.Examples.Messaging
{
    using System;
    using System.Threading;
    using Apache.Ignite.Core;
    using Apache.Ignite.ExamplesDll.Messaging;

   
    public class MessagingExample
    {
        
        [STAThread]
        public static void Main()
        {
            using (var ignite = Ignition.StartFromApplicationConfiguration())
            {
                var remotes = ignite.GetCluster().ForRemotes();

                if (remotes.GetNodes().Count == 0)
                {
                    Console.WriteLine(">>> This example requires remote nodes to be started.");
                    Console.WriteLine(">>> Please start at least 1 remote node.");
                    Console.WriteLine(">>> Refer to example's documentation for details on configuration.");
                }
                else
                {
                    Console.WriteLine(">>> Messaging example started.");
                    Console.WriteLine();

                    
                    var localMessaging = ignite.GetCluster().ForLocal().GetMessaging();

                    var msgCount = remotes.GetNodes().Count * 10;

                    var orderedCounter = new CountdownEvent(msgCount);
                    var unorderedCounter = new CountdownEvent(msgCount);

                    localMessaging.LocalListen(new LocalListener(unorderedCounter), Topic.Unordered);

                    localMessaging.LocalListen(new LocalListener(orderedCounter), Topic.Ordered);

                   
                    var remoteMessaging = remotes.GetMessaging();

                    var idUnordered = remoteMessaging.RemoteListen(new RemoteUnorderedListener(), Topic.Unordered);
                    var idOrdered = remoteMessaging.RemoteListen(new RemoteOrderedListener(), Topic.Ordered);

                    
                    Console.WriteLine(">>> Sending unordered messages...");

                    for (var i = 0; i < 10; i++)
                        remoteMessaging.Send(i, Topic.Unordered);

                    Console.WriteLine(">>> Finished sending unordered messages.");

                   
                    Console.WriteLine(">>> Sending ordered messages...");

                    for (var i = 0; i < 10; i++)
                        remoteMessaging.SendOrdered(i, Topic.Ordered);

                    Console.WriteLine(">>> Finished sending ordered messages.");

                    Console.WriteLine(">>> Check output on all nodes for message printouts.");
                    Console.WriteLine(">>> Waiting for messages acknowledgements from all remote nodes...");

                    unorderedCounter.Wait();
                    orderedCounter.Wait();

                   
                    remoteMessaging.StopRemoteListen(idUnordered);
                    remoteMessaging.StopRemoteListen(idOrdered);
                }
            }

            Console.WriteLine();
            Console.WriteLine(">>> Example finished, press any key to exit ...");
            Console.ReadKey();
        }
    }
}
