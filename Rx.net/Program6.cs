using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace RX.net
{
    class Program6
    {
        static void Subject(Subject<string> subject)
        {
            WriteSequenceToConsole(subject);
            subject.OnNext("Hello");
            subject.OnNext("To");
            subject.OnNext("All");
        }

        static void WriteSequenceToConsole(IObservable<string> sequence)
        {
            //The next two lines are equivalent.
            sequence.Subscribe(value => Console.WriteLine(value));
           
        }

        static void Main(string[] args)
        {
            var subject = new Subject<string>();
            Subject(subject);
            Console.ReadKey();
        }

        
    }
}