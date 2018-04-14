using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rx.net
{
   

    class Test2
    {
        static int stock=6;

        static void Call(Subject<string > src)
        {
            Thread t = Thread.CurrentThread;
            if ((stock--)>0)
            src.OnNext("User "+t.Name +" Booked");
            else
            src.OnNext("User " + t.Name + "Out of Stock");
        }
       
        static void Entry()
        {
            Thread[] t = new Thread[20];
            var subject = new Subject<string>();
            subject.Subscribe(value => Console.WriteLine(value));

            for (int i = 0; i < 20; i++)
            {
                t[i] = new Thread(() => Call(subject));
                t[i].Name = " " + (i + 1) + " ";
            }

            for (int i = 0; i < 20; i++)
            {   t[i].Start(); }
        }

        static void Main(string[] arg)
        {
            Entry();


            //t[4].Start();
            //t[0].Start();
            //t[3].Start();
            //t[2].Start();
            //t[5].Start();
            //t[7].Start();
            //t[8].Start();
            //t[1].Start();
            //t[9].Start();
            //t[6].Start();

            Console.ReadKey();
        }
    }
}
