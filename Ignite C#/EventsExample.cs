

namespace Apache.Ignite.Examples.Events
{
    using System;
    using System.Linq;
    using Apache.Ignite.Core;
    using Apache.Ignite.Core.Events;
    using Apache.Ignite.ExamplesDll.Binary;
    using Apache.Ignite.ExamplesDll.Compute;
    using Apache.Ignite.ExamplesDll.Events;

   
    public class EventsExample
    {
      
        [STAThread]
        public static void Main()
        {
            using (var ignite = Ignition.StartFromApplicationConfiguration())
            {
                Console.WriteLine(">>> Events example started.");
                Console.WriteLine();

               
                Console.WriteLine(">>> Listening for a local event...");

                ignite.GetEvents().EnableLocal(EventType.TaskExecutionAll);

                var listener = new LocalListener();
                ignite.GetEvents().LocalListen(listener, EventType.TaskExecutionAll);

                ExecuteTask(ignite);

                ignite.GetEvents().StopLocalListen(listener);

                Console.WriteLine(">>> Received events count: " + listener.EventsReceived);
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine(">>> Example finished, press any key to exit ...");
            Console.ReadKey();
        }

       
        private static void ExecuteTask(IIgnite ignite)
        {
            var employees = Enumerable.Range(1, 10).SelectMany(x => new[]
            {
                new Employee("Allison Mathis",
                    25300,
                    new Address("2702 Freedom Lane, San Francisco, CA", 94109),
                    new[] {"Development"}),

                new Employee("Breana Robbin",
                    6500,
                    new Address("3960 Sundown Lane, Austin, TX", 78130),
                    new[] {"Sales"})
            }).ToArray();

            ignite.GetCompute().Execute(new AverageSalaryTask(), employees);
        }
    }
}
