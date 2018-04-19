
namespace Apache.Ignite.Examples.Datagrid
{
    using System;
    using System.IO;
    using System.Threading;
    using Apache.Ignite.Core;
    using Apache.Ignite.Core.Cache;
    using Apache.Ignite.Core.Cache.Configuration;
    using Apache.Ignite.Core.Cache.Eviction;

    
    public class MultiTieredCacheExample
    {
        /// <summary>Example cache name.</summary>
        private const string CacheName = "dotnet_multi_tiered_example_cache";

        /// <summary>Cache entry size, in bytes..</summary>
        private const int EntrySize = 1024;

        [STAThread]
        public static void Main()
        {
            Console.WriteLine();
            Console.WriteLine(">>> Multi-tiered cache example started.");

            // Configure swap in the current bin directory (where our assembly is located).
            var binDir = Path.GetDirectoryName(typeof(MultiTieredCacheExample).Assembly.Location);
            var swapDir = Path.Combine(binDir, "ignite-swap");

            Console.WriteLine(">>> Swap space directory: " + swapDir);

            using (var ignite = Ignition.StartFromApplicationConfiguration())
            {
                var cacheCfg = new CacheConfiguration
                {
                    Name = CacheName,
                    Backups = 1,
                    EvictionPolicy = new LruEvictionPolicy
                    {
                        MaxSize = 10 // Maximum number of entries that will be stored in Java heap. 
                    },
                };

                ICache<int, byte[]> cache = ignite.GetOrCreateCache<int, byte[]>(cacheCfg);

                // Sample data.
                byte[] dataBytes = new byte[EntrySize];

                // Filling out cache and printing its metrics.
                PrintCacheMetrics(cache);

                for (int i = 0; i < 100; i++)
                {
                    cache.Put(i, dataBytes);

                    if (i%10 == 0)
                    {
                        Console.WriteLine(">>> Cache entries created: {0}", i + 1);

                        PrintCacheMetrics(cache);
                    }
                }

                Console.WriteLine(">>> Waiting for metrics final update...");

                Thread.Sleep(IgniteConfiguration.DefaultMetricsUpdateFrequency);

                PrintCacheMetrics(cache);

                Console.WriteLine();
                Console.WriteLine(">>> Example finished, press any key to exit ...");
                Console.ReadKey();
            }
        }

       
        private static void PrintCacheMetrics(ICache<int, byte[]> cache)
        {
            var metrics = cache.GetLocalMetrics();

            Console.WriteLine("\n>>> Cache entries layout: [Total={0}, Java heap={1}, Off-Heap={2}]",
                cache.GetSize(CachePeekMode.All), 
                metrics.Size, metrics.OffHeapEntriesCount);
        }
    }
}