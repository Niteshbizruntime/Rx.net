using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace Rx.net
{
    class ReducingSequence
    {
        static void  WhereExample()
        {
            var oddNumbers = Observable.Range(0, 10)
            .Where(i => i % 2 == 0)
            .Subscribe(Console.WriteLine,() => Console.WriteLine("Completed"));
        }

        static void DistinctExample()
        {
            var subject = new Subject<int>();
            var distinct = subject.Distinct();
            subject.Subscribe(
            i => Console.WriteLine("{0}", i),
            () => Console.WriteLine("subject.OnCompleted()"));
            distinct.Subscribe(
            i => Console.WriteLine("distinct.OnNext({0})", i),
            () => Console.WriteLine("distinct.OnCompleted()"));
            subject.OnNext(1);
            subject.OnNext(2);
            subject.OnNext(3);
            subject.OnNext(1);
            subject.OnNext(1);
            subject.OnNext(4);
            subject.OnCompleted();
        }

        static void DistinctUntilChangeExample()
        {

            var subject = new Subject<int>();
            var distinct = subject.DistinctUntilChanged();
            subject.Subscribe(
            i => Console.WriteLine("{0}", i),
            () => Console.WriteLine("subject.OnCompleted()"));
            distinct.Subscribe(
            i => Console.WriteLine("distinct.OnNext({0})", i),
            () => Console.WriteLine("distinct.OnCompleted()"));
            subject.OnNext(1);
            subject.OnNext(2);
            subject.OnNext(3);
            subject.OnNext(1);
            subject.OnNext(1);
            subject.OnNext(4);
            subject.OnCompleted();

        }

        
        static void IgnoreElementsExample()
        {
            var subject = new Subject<int>();
            //Could use subject.Where(_=>false);
            var noElements = subject.IgnoreElements();
            subject.Subscribe(
            i => Console.WriteLine("subject.OnNext({0})", i),
            () => Console.WriteLine("subject.OnCompleted()"));
            noElements.Subscribe(
            i => Console.WriteLine("noElements.OnNext({0})", i),
            () => Console.WriteLine("noElements.OnCompleted()"));
            subject.OnNext(1);
            subject.OnNext(2);
            subject.OnNext(3);
            subject.OnCompleted();
        }

        static void SkipElements()
        {
            Observable.Range(0, 10)
            .Skip(3)
            .Subscribe(Console.WriteLine, () => Console.WriteLine("Completed"));
        }

        static void TakeElements()
        {
            Observable.Range(0, 10)
            .Take(3)
            .Subscribe(Console.WriteLine, () => Console.WriteLine("Completed"));
        }

        static void SkipWhileElements()
        {
            var subject = new Subject<int>();
            subject
            .SkipWhile(i => i < 4)
            .Subscribe(Console.WriteLine, () => Console.WriteLine("Completed"));
            subject.OnNext(1);
            subject.OnNext(2);
            subject.OnNext(3);
            subject.OnNext(4);
            subject.OnNext(3);
            subject.OnNext(2);
            subject.OnNext(1);
            subject.OnNext(0);
            subject.OnCompleted();
        }
        static void TakeWhileElements()
        {
            var subject = new Subject<int>();
            subject
            .TakeWhile(i => i < 4)
            .Subscribe(Console.WriteLine, () => Console.WriteLine("Completed"));
            subject.OnNext(1);
            subject.OnNext(2);
            subject.OnNext(3);
            subject.OnNext(4);
            subject.OnNext(3);
            subject.OnNext(2);
            subject.OnNext(1);
            subject.OnNext(0);
            subject.OnCompleted();
        }

        static void SkipLastElements()
        {
            var subject = new Subject<int>();
            subject
            .SkipLast(2)
            .Subscribe(Console.WriteLine, () => Console.WriteLine("Completed"));
            Console.WriteLine("Pushing 1");
            subject.OnNext(1);
            Console.WriteLine("Pushing 2");
            subject.OnNext(2);
            Console.WriteLine("Pushing 3");
            subject.OnNext(3);
            Console.WriteLine("Pushing 4");
            subject.OnNext(4);
            subject.OnCompleted();
        }

        static void TakeLastElements()
        {
            var subject = new Subject<int>();
            subject
            .SkipLast(2)
            .Subscribe(Console.WriteLine, () => Console.WriteLine("Completed"));
            Console.WriteLine("Pushing 1");
            subject.OnNext(1);
            Console.WriteLine("Pushing 2");
            subject.OnNext(2);
            Console.WriteLine("Pushing 3");
            subject.OnNext(3);
            Console.WriteLine("Pushing 4");
            subject.OnNext(4);
            Console.WriteLine("Completing");
            subject.OnCompleted();
        }

        static void SkipUntillElements()
        {
            var subject = new Subject<int>();
            var otherSubject = new Subject<Unit>();
            subject
            .SkipUntil(otherSubject)
            .Subscribe(Console.WriteLine, () => Console.WriteLine("Completed"));
            subject.OnNext(1);
            subject.OnNext(2);
            subject.OnNext(3);
            otherSubject.OnNext(Unit.Default);
            subject.OnNext(4);
            subject.OnNext(5);
            subject.OnNext(6);
            subject.OnNext(7);
            subject.OnNext(8);
            subject.OnCompleted();
        }

        static void TakeUntillElements()
        {
            var subject = new Subject<int>();
            var otherSubject = new Subject<Unit>();
            subject
            .TakeUntil(otherSubject)
            .Subscribe(Console.WriteLine, () => Console.WriteLine("Completed"));
            subject.OnNext(1);
            subject.OnNext(2);
            subject.OnNext(3);
            otherSubject.OnNext(Unit.Default);
            subject.OnNext(4);
            subject.OnNext(5);
            subject.OnNext(6);
            subject.OnNext(7);
            subject.OnNext(8);
            subject.OnCompleted();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("\n----------------Where--------------");
            WhereExample();
            Console.WriteLine("\n-------------Distinct--------------");
            DistinctExample();
            Console.WriteLine("\n-------DistinctUntillChange--------");
            DistinctUntilChangeExample();
            Console.WriteLine("\n--------IgnoreElements-------------");
            IgnoreElementsExample();
            Console.WriteLine("\n--------- SkipElements-------------");
            SkipElements();
            Console.WriteLine("\n----------TakeElements-------------");
            TakeElements();
            Console.WriteLine("\n---------SkipWhileElements---------");
            SkipWhileElements();
            Console.WriteLine("\n---------TakeWhileElements---------");
            TakeWhileElements();
            Console.WriteLine("\n---------SkipLastElements----------");
            SkipLastElements();
            Console.WriteLine("\n---------TakeLastElements----------");
            TakeLastElements();
            Console.WriteLine("\n---------SkipUntillElements--------");
            SkipUntillElements();
            Console.WriteLine("\n---------TakeUntillElements--------");
            TakeUntillElements();
            Console.ReadKey();
        }

    }
}
