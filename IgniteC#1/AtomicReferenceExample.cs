    using System;
    using Apache.Ignite.Core;
    using Apache.Ignite.Core.DataStructures;
    using Apache.Ignite.ExamplesDll.DataStructures;

namespace Apache.Ignite.Examples.DataStructures
{
   

   
    public static class AtomicReferenceExample
    {
        [STAThread]
        public static void Main()
        {
            using (var ignite = Ignition.StartFromApplicationConfiguration())
            {
                Console.WriteLine();
                Console.WriteLine(">>> Atomic reference example started.");

              
                IAtomicReference<Guid> atomicRef = ignite.GetAtomicReference(
                    AtomicReferenceModifyAction.AtomicReferenceName, Guid.Empty, true);

            
                atomicRef.Write(Guid.Empty);

              
                ignite.GetCompute().Broadcast(new AtomicReferenceModifyAction());
                
              
                Console.WriteLine("\n>>> Current atomic reference value: " + atomicRef.Read());
            }

            Console.WriteLine("\n>>> Example finished, press any key to exit ...");
            Console.ReadKey();
        }
    }
}
