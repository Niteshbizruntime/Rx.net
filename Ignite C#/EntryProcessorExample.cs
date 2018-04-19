

namespace Apache.Ignite.Examples.Datagrid
{
    using System;
    using System.Linq;
    using Apache.Ignite.Core;
    using Apache.Ignite.Core.Cache;
    using Apache.Ignite.ExamplesDll.Datagrid;

  
    public static class EntryProcessorExample
    {
        /// <summary>Cache name.</summary>
        private const string CacheName = "dotnet_cache_put_get";

        /// <summary>Entry count.</summary>
        private const int EntryCount = 20;

        /// <summary>
        /// Runs the example.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            using (var ignite = Ignition.StartFromApplicationConfiguration())
            {
                Console.WriteLine();
                Console.WriteLine(">>> Cache EntryProcessor example started.");

                ICache<int, int> cache = ignite.GetOrCreateCache<int, int>(CacheName);
                cache.Clear();

                // Populate cache with Invoke.
                int[] keys = Enumerable.Range(1, EntryCount).ToArray();

                foreach (var key in keys)
                    cache.Invoke(key, new CachePutEntryProcessor(), 10);

                PrintCacheEntries(cache);

                // Increment entries by 5 with InvokeAll.
                cache.InvokeAll(keys, new CacheIncrementEntryProcessor(), 5);

                PrintCacheEntries(cache);
            }

            Console.WriteLine();
            Console.WriteLine(">>> Example finished, press any key to exit ...");
            Console.ReadKey();
        }

      
        private static void PrintCacheEntries(ICache<int, int> cache)
        {
            Console.WriteLine("\n>>> Entries in cache:");

            foreach (var entry in cache)
                Console.WriteLine(entry);
        }
    }
}
