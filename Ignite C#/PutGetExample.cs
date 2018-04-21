

namespace Apache.Ignite.Examples.Datagrid
{
    using System;
    using System.Collections.Generic;
    using Apache.Ignite.Core;
    using Apache.Ignite.Core.Binary;
    using Apache.Ignite.Core.Cache;
    using Apache.Ignite.ExamplesDll.Binary;

  
    public class PutGetExample
    {
       
        private const string CacheName = "dotnet_cache_put_get";

        [STAThread]
        public static void Main()
        {
            using (var ignite = Ignition.StartFromApplicationConfiguration())
            {
                Console.WriteLine();
                Console.WriteLine(">>> Cache put-get example started.");

                // Clean up caches on all nodes before run.
                ignite.GetOrCreateCache<object, object>(CacheName).Clear();

                PutGet(ignite);
                PutGetBinary(ignite);
                PutAllGetAll(ignite);
                PutAllGetAllBinary(ignite);

                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine(">>> Example finished, press any key to exit ...");
            Console.ReadKey();
        }

      
        private static void PutGet(IIgnite ignite)
        {
            var cache = ignite.GetCache<int, Organization>(CacheName);

           
            Organization org = new Organization(
                "Microsoft",
                new Address("1096 Eddy Street, San Francisco, CA", 94109),
                OrganizationType.Private,
                DateTime.Now
            );

         
            cache.Put(1, org);

        
            Organization orgFromCache = cache.Get(1);

            Console.WriteLine();
            Console.WriteLine(">>> Retrieved organization instance from cache: " + orgFromCache);
        }

      
        private static void PutGetBinary(IIgnite ignite)
        {
            var cache = ignite.GetCache<int, Organization>(CacheName);

         
            Organization org = new Organization(
                "Microsoft",
                new Address("1096 Eddy Street, San Francisco, CA", 94109),
                OrganizationType.Private,
                DateTime.Now
            );

          
            cache.Put(1, org);

           
            var binaryCache = cache.WithKeepBinary<int, IBinaryObject>();

          
            var binaryOrg = binaryCache.Get(1);

          
            string name = binaryOrg.GetField<string>("name");

            Console.WriteLine();
            Console.WriteLine(">>> Retrieved organization name from binary object: " + name);
        }

      
        private static void PutAllGetAll(IIgnite ignite)
        {
            var cache = ignite.GetCache<int, Organization>(CacheName);

         
            Organization org1 = new Organization(
                "Microsoft",
                new Address("1096 Eddy Street, San Francisco, CA", 94109),
                OrganizationType.Private,
                DateTime.Now
            );

            Organization org2 = new Organization(
                "Red Cross",
                new Address("184 Fidler Drive, San Antonio, TX", 78205),
                OrganizationType.NonProfit,
                DateTime.Now
            );

            var map = new Dictionary<int, Organization> { { 1, org1 }, { 2, org2 } };

          
            cache.PutAll(map);

          
            ICollection<ICacheEntry<int, Organization>> mapFromCache = cache.GetAll(new List<int> { 1, 2 });

            Console.WriteLine();
            Console.WriteLine(">>> Retrieved organization instances from cache:");

            foreach (ICacheEntry<int, Organization> org in mapFromCache)
                Console.WriteLine(">>>     " + org.Value);
        }

  
        private static void PutAllGetAllBinary(IIgnite ignite)
        {
            var cache = ignite.GetCache<int, Organization>(CacheName);

           
            Organization org1 = new Organization(
                "Microsoft",
                new Address("1096 Eddy Street, San Francisco, CA", 94109),
                OrganizationType.Private,
                DateTime.Now
            );

            Organization org2 = new Organization(
                "Red Cross",
                new Address("184 Fidler Drive, San Antonio, TX", 78205),
                OrganizationType.NonProfit,
                DateTime.Now
            );

            var map = new Dictionary<int, Organization> { { 1, org1 }, { 2, org2 } };

          
            cache.PutAll(map);

            
            var binaryCache = cache.WithKeepBinary<int, IBinaryObject>();

         
            ICollection<ICacheEntry<int, IBinaryObject>> binaryMap = binaryCache.GetAll(new List<int> { 1, 2 });

            Console.WriteLine();
            Console.WriteLine(">>> Retrieved organization names from binary objects:");

            foreach (var pair in binaryMap)
                Console.WriteLine(">>>     " + pair.Value.GetField<string>("name"));
        }
    }
}
