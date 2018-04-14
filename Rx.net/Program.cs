using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Threading;

namespace Rx.net
{
    class Observer : IObserver<int>
    {
        public void OnCompleted()
        {
            Console.WriteLine("Observation Completed");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine($"Error:{error.Message}");
        }

        public void OnNext(int value)
        {

            Console.WriteLine($"Value received :{value}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var observable = Observable.Range(5, 8).Where(i=>i%2==0);
            var subscription = observable.Subscribe(new Observer());
            Console.ReadKey();
            subscription.Dispose();
        }
    }
}
