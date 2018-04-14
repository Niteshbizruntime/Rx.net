using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace RX.net
{
    class Program4
    {
        static void WriteSequenceToConsole(IObservable<string> sequence)
        {
            sequence.Subscribe(value => Console.WriteLine(value));
        }

        static void ReplaySubject(ReplaySubject<string> subject)
        {
            subject.OnNext("Hello");
            WriteSequenceToConsole(subject);
            subject.OnNext("To");
            subject.OnNext("All");
           
        }
        static void Main(string[] args)
        {

            var subject = new ReplaySubject<string>();
            ReplaySubject(subject);
            Console.ReadKey();
        }
       
    }
}