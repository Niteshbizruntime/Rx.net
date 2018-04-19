

namespace Apache.Ignite.Examples.Datagrid
{
    using System;
    using Apache.Ignite.Core;
    using Apache.Ignite.Core.Cache;
    using Apache.Ignite.Core.Cache.Configuration;
    using Apache.Ignite.Core.Cache.Eviction;

   
    public class NearCacheExample
    {
        private const string CacheName = "dotnet_near_cache_example";

        [STAThread]
        public static void Main()
        {
            // Make sure to start an Ignite server node before.
            Ignition.ClientMode = true;

            using (var ignite = Ignition.StartFromApplicationConfiguration())
            {
                Console.WriteLine(">>> Client node connected to the cluster");

                // Creating a distributed and near cache.
                var nearCacheCfg = new NearCacheConfiguration
                {
                    EvictionPolicy = new LruEvictionPolicy
                    {
                        // Near cache will store only 10 recently accessed/used entries.
                        MaxSize = 10
                    }
                };

                Console.WriteLine(">>> Populating the cache...");

                ICache<int, int> cache = ignite.GetOrCreateCache<int, int>(
                    new CacheConfiguration(CacheName), nearCacheCfg);

                // Adding data into the cache. 
                // Latest 10 entries will be stored in the near cache on the client node side.
                for (int i = 0; i < 1000; i++)
                    cache.Put(i, i * 10);

                Console.WriteLine(">>> Cache size: [Total={0}, Near={1}]", 
                    cache.GetSize(), cache.GetSize(CachePeekMode.Near));

                Console.WriteLine("\n>>> Reading from near cache...");

                foreach (var entry in cache.GetLocalEntries(CachePeekMode.Near))
                    Console.WriteLine(entry);

                Console.WriteLine("\n>>> Example finished, press any key to exit ...");
                Console.ReadKey();
            }
        }
    }
}
