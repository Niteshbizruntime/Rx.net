using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace Rx.net
{
    class Rx5
    {

        static void WriteSequenceToConsole(IObservable<string> sequence)
        {
            //The next two lines are equivalent.
            //sequence.Subscribe(value=>Console.WriteLine(value));
            sequence.Subscribe(Console.WriteLine);
        }

        static void BehaviorSubjectExample()
        {
            var subject = new BehaviorSubject<string>("a");
            subject.OnNext("b");
            WriteSequenceToConsole(subject);
            subject.OnNext("c");
            subject.OnNext("d");
        }
        static void Main(string[] args)
        {
            BehaviorSubjectExample();
            Console.ReadKey();
        }
    }
}
