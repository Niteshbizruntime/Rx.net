using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace Rx.net
{
    class Rx2
    {
        static void WriteSequenceToConsole(IObservable<int> sequence)
        {
            //The next two lines are equivalent.
            //sequence.Subscribe(value=>Console.WriteLine(value));
            sequence.Subscribe(Console.WriteLine);
        }
            static void Main(string[] args)
           {
           
                var subject = new Subject<int>();
                WriteSequenceToConsole(subject);
                subject.OnNext(1);
                subject.OnNext(2);
                subject.OnNext(3);
                Console.ReadKey();
            
            //Takes an IObservable<string> as its parameter. 
            //Subject<string> implements this interface.
          
            }
        
    }
}
