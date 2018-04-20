

namespace Apache.Ignite.Examples.Sql
{
    using System;
    using Apache.Ignite.Core;
    using Apache.Ignite.Core.Cache;
    using Apache.Ignite.Core.Cache.Configuration;
    using Apache.Ignite.Core.Cache.Query;
    using Apache.Ignite.ExamplesDll.Binary;

   
    public class SqlDmlExample
    {
       
        private const string OrganizationCacheName = "dotnet_cache_query_dml_organization";

     
        private const string EmployeeCacheName = "dotnet_cache_query_dml_employee";

        [STAThread]
        public static void Main()
        {
            using (var ignite = Ignition.StartFromApplicationConfiguration())
            {
                Console.WriteLine();
                Console.WriteLine(">>> Cache query DML example started.");

                var employeeCache = ignite.GetOrCreateCache<int, Employee>(
                    new CacheConfiguration(EmployeeCacheName, new QueryEntity(typeof(int), typeof(Employee))));

                var organizationCache = ignite.GetOrCreateCache<int, Organization>(new CacheConfiguration(
                    OrganizationCacheName, new QueryEntity(typeof(int), typeof(Organization))));

                employeeCache.Clear();
                organizationCache.Clear();

                Insert(organizationCache, employeeCache);
                Select(employeeCache, "Inserted data");

                Update(employeeCache);
                Select(employeeCache, "Update salary for ASF employees");

                Delete(employeeCache);
                Select(employeeCache, "Delete non-ASF employees");

                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine(">>> Example finished, press any key to exit ...");
            Console.ReadKey();
        }

       
        private static void Select(ICache<int, Employee> employeeCache, string message)
        {
            Console.WriteLine("\n>>> {0}", message);

            var qry = new SqlFieldsQuery(string.Format(
                "select emp._key, emp.name, org.name, emp.salary " +
                "from Employee as emp, " +
                "\"{0}\".Organization as org " +
                "where emp.organizationId = org._key", OrganizationCacheName))
            {
                EnableDistributedJoins = true
            };

            using (var cursor = employeeCache.Query(qry))
            {
                foreach (var row in cursor)
                {
                    Console.WriteLine(">>> {0}: {1}, {2}, {3}", row[0], row[1], row[2], row[3]);
                }
            }
        }

      
        private static void Insert(ICache<int, Organization> organizationCache, ICache<int, Employee> employeeCache)
        {
           
            var qry = new SqlFieldsQuery("insert into Organization (_key, name) values (?, ?)", 1, "ASF");
            organizationCache.Query(qry);

            qry.Arguments = new object[] {2, "Eclipse"};
            organizationCache.Query(qry);

           
            qry = new SqlFieldsQuery("insert into Employee (_key, name, organizationId, salary) values (?, ?, ?, ?)");

            qry.Arguments = new object[] {1, "John Doe", 1, 4000};
            employeeCache.Query(qry);

            qry.Arguments = new object[] {2, "Jane Roe", 1, 5000};
            employeeCache.Query(qry);

            qry.Arguments = new object[] {3, "Mary Major", 2, 2000};
            employeeCache.Query(qry);

            qry.Arguments = new object[] {4, "Richard Miles", 2, 3000};
            employeeCache.Query(qry);
        }

        
        private static void Update(ICache<int, Employee> employeeCache)
        {
            var qry = new SqlFieldsQuery("update Employee set salary = salary * 1.1 where organizationId = ?", 1);

            employeeCache.Query(qry);
        }

       
        private static void Delete(ICache<int, Employee> employeeCache)
        {
            var qry = new SqlFieldsQuery("delete from Employee where organizationId != ?", 1);

            employeeCache.Query(qry);
        }
    }
}
