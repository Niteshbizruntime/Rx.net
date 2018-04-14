using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace Rx.net
{
    class AdvanceErrorControl
    {
        static void Dump<T>(IObservable<T> source, string name)
        {
            source.Subscribe(
          i => Console.WriteLine("{0}-->{1}", name, i),
          ex => Console.WriteLine("{0} failed-->{1}", name, ex.Message),
          () => Console.WriteLine("{0} completed", name));
        }

        static void Catch()
        {
            var source = new Subject<int>();
            var result = source.Catch(Observable.Empty<int>());
             Dump(result,"Catch");
            source.OnNext(1);
            source.OnNext(2);
            source.OnError(new Exception("Fail!"));
        }

        static void Finally()
        {
            var source = new Subject<int>();
            var result = source.Finally(() => Console.WriteLine("Finally action ran"));
            Dump(result,"Finally");
            source.OnNext(1);
            source.OnNext(2);
            source.OnNext(3);
            source.OnCompleted();
        }
    }
}
