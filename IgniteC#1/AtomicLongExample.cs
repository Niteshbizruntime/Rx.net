
    using System;
    using System.Threading;
    using Apache.Ignite.Core;
    using Apache.Ignite.Core.DataStructures;
    using Apache.Ignite.ExamplesDll.DataStructures;

namespace Apache.Ignite.Examples.DataStructures
{
  

  
    public static class AtomicLongExample
    {
        [STAThread]
        public static void Main()
        {
            using (var ignite = Ignition.StartFromApplicationConfiguration())
            {
                Console.WriteLine();
                Console.WriteLine(">>> Atomic long example started.");

                IAtomicLong atomicLong = ignite.GetAtomicLong(AtomicLongIncrementAction.AtomicLongName, 0, true);

                Console.WriteLine(">>> Atomic long initial value: " + atomicLong.Read());

               
                ignite.GetCompute().Broadcast(new AtomicLongIncrementAction());

               
                Console.WriteLine("\n>>> Atomic long current value: " + atomicLong.Read());
            }

            Console.WriteLine("\n>>> Check output on all nodes.");
            Console.WriteLine("\n>>> Example finished, press any key to exit ...");
            Console.ReadKey();
        }
    }
}
