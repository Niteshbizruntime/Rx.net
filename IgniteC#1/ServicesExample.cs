     using System;
     using Apache.Ignite.Core;
    using Apache.Ignite.ExamplesDll.Services;

namespace Apache.Ignite.Examples.Services
{
    

   
    public class ServicesExample
    {
       
        [STAThread]
        public static void Main()
        {
            using (var ignite = Ignition.StartFromApplicationConfiguration())
            {
                Console.WriteLine(">>> Services example started.");
                Console.WriteLine();

                var svc = new MapService<int, string>();
                Console.WriteLine(">>> Deploying service to all nodes...");
                ignite.GetServices().DeployNodeSingleton("service", svc);

                var prx = ignite.GetServices().GetServiceProxy<IMapService<int, string>>("service", true);

                for (var i = 0; i < 10; i++)
                    prx.Put(i, i.ToString());

                var mapSize = prx.Size;

                Console.WriteLine(">>> Map service size: " + mapSize);

                ignite.GetServices().CancelAll();
            }

            Console.WriteLine();
            Console.WriteLine(">>> Example finished, press any key to exit ...");
            Console.ReadKey();
        }
    }
}
