

namespace Apache.Ignite.Examples.Datagrid
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Apache.Ignite.Core;
    using Apache.Ignite.Core.Cache.Event;
    using Apache.Ignite.Core.Cache.Query.Continuous;
    using Apache.Ignite.ExamplesDll.Datagrid;

   
    public class ContinuousQueryExample
    {
        /// <summary>Cache name.</summary>
        private const string CacheName = "dotnet_cache_continuous_query";

        /// <summary>
        /// Runs the example.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            using (var ignite = Ignition.StartFromApplicationConfiguration())
            {
                Console.WriteLine();
                Console.WriteLine(">>> Cache continuous query example started.");

                var cache = ignite.GetOrCreateCache<int, string>(CacheName);

                // Clean up caches on all nodes before run.
                cache.Clear();

                const int keyCnt = 20;

                for (int i = 0; i < keyCnt; i++)
                    cache.Put(i, i.ToString());

                var qry = new ContinuousQuery<int, string>(new Listener<string>(), new ContinuousQueryFilter(15));


                // Create new continuous query.
                using (cache.QueryContinuous(qry))
                {
                    // Add a few more keys and watch more query notifications.
                    for (var i = keyCnt; i < keyCnt + 5; i++)
                        cache.Put(i, i.ToString());

                    // Wait for a while while callback is notified about remaining puts.
                    Thread.Sleep(2000);
                }
            }

            Console.WriteLine();
            Console.WriteLine(">>> Example finished, press any key to exit ...");
            Console.ReadKey();
        }

        /// <summary>
        /// Callback for continuous query example.
        /// </summary>
        private class Listener<T> : ICacheEntryEventListener<int, T>
        {
            public void OnEvent(IEnumerable<ICacheEntryEvent<int, T>> events)
            {
                foreach (var e in events)
                    Console.WriteLine("Queried entry [key=" + e.Key + ", val=" + e.Value + ']');
            }
        }
    }
}
