using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace RX.net
{
    class Program3
    {
        static void WriteSequenceToConsole(IObservable<string> sequence)
        {
            sequence.Subscribe(value => Console.WriteLine(value));
        }

        static void AsyncSubject(AsyncSubject<string> subject)
        {
            //In the asyncSubject only one value will be printed then immediately complete
            subject.OnNext("hello");
            WriteSequenceToConsole(subject);
            subject.OnNext("to");
            subject.OnNext("All");
            subject.OnCompleted();
        }
        static void Main(string[] args)
        {
            var subject = new AsyncSubject<string>();
            AsyncSubject(subject);
            Console.ReadKey();
        }
       
    }
}