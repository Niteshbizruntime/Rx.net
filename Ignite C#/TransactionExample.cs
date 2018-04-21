

namespace Apache.Ignite.Examples.Datagrid
{
    using System;
    using System.Transactions;
    using Apache.Ignite.Core;
    using Apache.Ignite.Core.Cache;
    using Apache.Ignite.Core.Cache.Configuration;
    using Apache.Ignite.Core.Transactions;
    using Apache.Ignite.ExamplesDll.Binary;

 
    public class TransactionExample
    {
       
        private const string CacheName = "dotnet_cache_tx";

        [STAThread]
        public static void Main()
        {
            using (var ignite = Ignition.StartFromApplicationConfiguration())
            {
                Console.WriteLine();
                Console.WriteLine(">>> Transaction example started.");

                var cache = ignite.GetOrCreateCache<int, Account>(new CacheConfiguration
                {
                    Name = CacheName,
                    AtomicityMode = CacheAtomicityMode.Transactional
                });

                InitAccounts(cache);

                Console.WriteLine("\n>>> Transferring with Ignite transaction API...");

              
                using (var tx = cache.Ignite.GetTransactions().TxStart(TransactionConcurrency.Pessimistic,
                    TransactionIsolation.RepeatableRead))
                {
                    Account acc1 = cache.Get(1);
                    Account acc2 = cache.Get(2);

                    acc1.Balance += 100;
                    acc2.Balance -= 100;

                    cache.Put(1, acc1);
                    cache.Put(2, acc2);

                    tx.Commit();
                }

                DisplayAccounts(cache);

                InitAccounts(cache);

                Console.WriteLine("\n>>> Transferring with TransactionScope API...");

               
                using (var ts = new TransactionScope())
                {
                    Account acc1 = cache.Get(1);
                    Account acc2 = cache.Get(2);

                    acc1.Balance += 100;
                    acc2.Balance -= 100;

                    cache.Put(1, acc1);
                    cache.Put(2, acc2);

                    ts.Complete();
                }

                DisplayAccounts(cache);
            }

            Console.WriteLine();
            Console.WriteLine(">>> Example finished, press any key to exit ...");
            Console.ReadKey();
        }

    
        private static void DisplayAccounts(ICache<int, Account> cache)
        {
            Console.WriteLine(">>> Transfer finished.");

            Console.WriteLine();
            Console.WriteLine(">>> Accounts after transfer: ");
            Console.WriteLine(">>>     " + cache.Get(1));
            Console.WriteLine(">>>     " + cache.Get(2));
            Console.WriteLine();
        }

    
        private static void InitAccounts(ICache<int, Account> cache)
        {
           
            cache.Clear();

          
            cache.Put(1, new Account(1, 100));
            cache.Put(2, new Account(2, 200));

            Console.WriteLine();
            Console.WriteLine(">>> Accounts before transfer: ");
            Console.WriteLine(">>>     " + cache.Get(1));
            Console.WriteLine(">>>     " + cache.Get(2));
            Console.WriteLine();
        }
    }
}
