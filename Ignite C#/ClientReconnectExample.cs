

namespace Apache.Ignite.Examples.Misc
{
    using System;
    using System.Threading;
    using Apache.Ignite.Core;
    using Apache.Ignite.Core.Cache;
    using Apache.Ignite.Core.Cache.Configuration;
    using Apache.Ignite.Core.Common;
    using Apache.Ignite.Core.Discovery.Tcp;
    using Apache.Ignite.Core.Discovery.Tcp.Static;
    using Apache.Ignite.Core.Events;

    public class ClientReconnectExample
    {
        private const string CacheName = "dotnet_client_reconnect_cache";

        [STAThread]
        public static void Main()
        {
            Console.WriteLine();
            Console.WriteLine(">>> Client reconnect example started.");

            var evt = new ManualResetEvent(false);
            ThreadPool.QueueUserWorkItem(_ => RunServer(evt));

           
            Thread.Sleep(200);

            var cfg = new IgniteConfiguration(GetIgniteConfiguration())
            {
                ClientMode = true
            };

            using (var ignite = Ignition.Start(cfg))
            {
                Console.WriteLine(">>> Client node connected to the cluster.");

                if (ignite.GetCluster().GetNodes().Count > 2)
                    throw new Exception("Extra nodes detected. " +
                                        "ClientReconnectExample should be run without external nodes.");

                var cache = ignite.GetCache<int, string>(CacheName);

                for (var i = 0; i < 10; i++)
                {
                    try
                    {
                        Console.WriteLine(">>> Put value with key: " + i);
                        cache.Put(i, "val" + i);

                        Thread.Sleep(500);
                    }
                    catch (CacheException e)
                    {
                        var disconnectedException = e.InnerException as ClientDisconnectedException;

                        if (disconnectedException != null)
                        {
                            Console.WriteLine(
                                "\n>>> Client disconnected from the cluster. Failed to put value with key: " + i);

                            disconnectedException.ClientReconnectTask.Wait();

                            Console.WriteLine("\n>>> Client reconnected to the cluster.");

                            // Updating the reference to the cache. The client reconnected to the new cluster.
                            cache = ignite.GetCache<int, string>(CacheName);
                        }
                        else
                        {
                            throw;
                        }
                    }

                }

                // Stop the server node.
                evt.Set();

                Console.WriteLine();
                Console.WriteLine(">>> Example finished, press any key to exit ...");
                Console.ReadKey();
            }
        }

      
        private static void RunServer(WaitHandle evt)
        {
            var cfg = new IgniteConfiguration(GetIgniteConfiguration())
            {
                // Nodes within a single process are distinguished by GridName property.
                IgniteInstanceName = "serverNode",

                CacheConfiguration = new[] {new CacheConfiguration(CacheName)},

                IncludedEventTypes = new[] {EventType.NodeJoined}
            };

           
            using (var ignite = Ignition.Start(cfg))
            {
                Console.WriteLine("\n>>> Server node started.");

            
                if (ignite.GetCluster().GetNodes().Count == 1)
                    ignite.GetEvents().WaitForLocal(EventType.NodeJoined);

               
                Thread.Sleep(2000);
            }

            Console.WriteLine("\n>>> Server node stopped.");

            
            Thread.Sleep(15000);

            Console.WriteLine("\n>>> Restarting server node...");

           
            using (Ignition.Start(cfg))
            {
                evt.WaitOne();
            }
        }

       
        private static IgniteConfiguration GetIgniteConfiguration()
        {
            return new IgniteConfiguration
            {
                DiscoverySpi = new TcpDiscoverySpi
                {
                    IpFinder = new TcpDiscoveryStaticIpFinder
                    {
                        Endpoints = new[] { "127.0.0.1:47500" }
                    }
                }
            };
        }
    }
}
