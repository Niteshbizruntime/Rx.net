

namespace Apache.Ignite.Examples.Datagrid
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Apache.Ignite.Core;
    using Apache.Ignite.Core.Cache;
    using Apache.Ignite.Core.Cache.Configuration;
    using Apache.Ignite.Core.Transactions;

   
    public class TransactionDeadlockDetectionExample
    {
       
        private const string CacheName = "dotnet_cache_tx_deadlock";

     
        [STAThread]
        public static void Main()
        {
            using (var ignite = Ignition.StartFromApplicationConfiguration())
            {
                Console.WriteLine();
                Console.WriteLine(">>> Transaction deadlock detection example started.");

                var cache = ignite.GetOrCreateCache<int, int>(new CacheConfiguration
                {
                    Name = CacheName,
                    AtomicityMode = CacheAtomicityMode.Transactional
                });

               
                cache.Clear();

                var keys = Enumerable.Range(1, 100).ToArray();

               
                var task1 = Task.Factory.StartNew(() => UpdateKeys(cache, keys, 1));
                var task2 = Task.Factory.StartNew(() => UpdateKeys(cache, keys.Reverse(), 2));

                Task.WaitAll(task1, task2);

                Console.WriteLine("\n>>> Example finished, press any key to exit ...");
                Console.ReadKey();
            }
        }

      
        private static void UpdateKeys(ICache<int, int> cache, IEnumerable<int> keys, int threadId)
        {
            var txs = cache.Ignite.GetTransactions();

            try
            {
                using (var tx = txs.TxStart(TransactionConcurrency.Pessimistic, TransactionIsolation.ReadCommitted,
                    TimeSpan.FromSeconds(2), 0))
                {
                    foreach (var key in keys)
                    {
                        cache[key] = threadId;
                    }

                  
                    Thread.Sleep(TimeSpan.FromSeconds(3));

                    tx.Commit();
                }
            }
            catch (TransactionDeadlockException e)
            {
                
                Console.WriteLine("\n>>> Transaction deadlock in thread {0}: {1}", threadId, e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("\n>>> Update failed in thread {0}: {1}", threadId, e);
            }
        }
    }
}
