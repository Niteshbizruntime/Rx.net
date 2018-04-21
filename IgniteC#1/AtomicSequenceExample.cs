
    using System;
    using System.Threading;
    using Apache.Ignite.Core;
    using Apache.Ignite.Core.DataStructures;
    using Apache.Ignite.ExamplesDll.DataStructures;

namespace Apache.Ignite.Examples.DataStructures
{
    

    public static class AtomicSequenceExample
    {
        [STAThread]
        public static void Main()
        {
          

            using (var ignite = Ignition.StartFromApplicationConfiguration())
            {
                Console.WriteLine();
                Console.WriteLine(">>> Atomic sequence example started.");

                IAtomicSequence atomicSequence = 
                    ignite.GetAtomicSequence(AtomicSequenceIncrementAction.AtomicSequenceName, 0, true);

                Console.WriteLine(">>> Atomic sequence initial value: " + atomicSequence.Read());

              
                ignite.GetCompute().Broadcast(new AtomicSequenceIncrementAction());

                
                Console.WriteLine("\n>>> Atomic sequence current value: " + atomicSequence.Read());
            }

            Console.WriteLine("\n>>> Check output on all nodes.");
            Console.WriteLine("\n>>> Example finished, press any key to exit ...");
            Console.ReadKey();
        }
    }
}
