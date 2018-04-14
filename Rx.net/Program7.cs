using System;
using System.Reactive.Subjects;
using System.Threading;

namespace RX.net
{
    class Program7
    {
       
        public static void ReplaySubjectBuffer()
        {
            var window = TimeSpan.FromMilliseconds(50);
            var subject = new ReplaySubject<string>(window);
            subject.OnNext("a");
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            subject.OnNext("b");
            Thread.Sleep(TimeSpan.FromMilliseconds(50));
            subject.OnNext("c");
            subject.Subscribe(Console.WriteLine);
            subject.OnNext("d");
        }
        static void WriteSequenceToConsole(IObservable<string> sequence)
        {
            sequence.Subscribe(value => Console.WriteLine(value));
        }

        static void Main(string[] args)
        {
            ReplaySubjectBuffer();
            Console.Read();
        }
    }
}