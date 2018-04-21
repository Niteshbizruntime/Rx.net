
namespace Apache.Ignite.Examples.Datagrid
{
    using System;
    using Apache.Ignite.Core;
    using Apache.Ignite.Core.Cache;
    using Apache.Ignite.Core.Cache.Configuration;
    using Apache.Ignite.Core.Cache.Query;
    using Apache.Ignite.ExamplesDll.Binary;
    using Apache.Ignite.ExamplesDll.Datagrid;

    public class QueryExample
    {
       
        private const string EmployeeCacheName = "dotnet_cache_query_employee";

        [STAThread]
        public static void Main()
        {
            using (var ignite = Ignition.StartFromApplicationConfiguration())
            {
                Console.WriteLine();
                Console.WriteLine(">>> Cache query example started.");

                var employeeCache = ignite.GetOrCreateCache<int, Employee>(
                    new CacheConfiguration(EmployeeCacheName, typeof(Employee)));

                // Populate cache with sample data entries.
                PopulateCache(employeeCache);

                // Run scan query example.
                ScanQueryExample(employeeCache);

                // Run full text query example.
                FullTextQueryExample(employeeCache);

                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine(">>> Example finished, press any key to exit ...");
            Console.ReadKey();
        }

        private static void ScanQueryExample(ICache<int, Employee> cache)
        {
            const int zip = 94109;

            var qry = cache.Query(new ScanQuery<int, Employee>(new ScanQueryFilter(zip)));

            Console.WriteLine();
            Console.WriteLine(">>> Employees with zipcode {0} (scan):", zip);

            foreach (var entry in qry)
                Console.WriteLine(">>>    " + entry.Value);
        }

     
        private static void FullTextQueryExample(ICache<int, Employee> cache)
        {
            var qry = cache.Query(new TextQuery("Employee", "TX"));

            Console.WriteLine();
            Console.WriteLine(">>> Employees living in Texas:");

            foreach (var entry in qry)
                Console.WriteLine(">>> " + entry.Value);
        }

       
        private static void PopulateCache(ICache<int, Employee> cache)
        {
            cache.Put(1, new Employee(
                "James Wilson",
                12500,
                new Address("1096 Eddy Street, San Francisco, CA", 94109),
                new[] {"Human Resources", "Customer Service"},
                1));

            cache.Put(2, new Employee(
                "Daniel Adams",
                11000,
                new Address("184 Fidler Drive, San Antonio, TX", 78130),
                new[] {"Development", "QA"},
                1));

            cache.Put(3, new Employee(
                "Cristian Moss",
                12500,
                new Address("667 Jerry Dove Drive, Florence, SC", 29501),
                new[] {"Logistics"},
                1));

            cache.Put(4, new Employee(
                "Allison Mathis",
                25300,
                new Address("2702 Freedom Lane, San Francisco, CA", 94109),
                new[] {"Development"},
                2));

            cache.Put(5, new Employee(
                "Breana Robbin",
                6500,
                new Address("3960 Sundown Lane, Austin, TX", 78130),
                new[] {"Sales"},
                2));

            cache.Put(6, new Employee(
                "Philip Horsley",
                19800,
                new Address("2803 Elsie Drive, Sioux Falls, SD", 57104),
                new[] {"Sales"},
                2));

            cache.Put(7, new Employee(
                "Brian Peters",
                10600,
                new Address("1407 Pearlman Avenue, Boston, MA", 12110),
                new[] {"Development", "QA"},
                2));
        }
    }
}
