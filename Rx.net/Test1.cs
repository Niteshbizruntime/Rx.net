using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reactive;

namespace Rx.net
{
    class Employee
    {
        public string Name { get; set; }
        public string Age { get; set; }
    }
    class Test1
    {
        static List<Employee> list = new List<Employee>();
        static void Add()
        {
            Console.Write("How Many Record U Want To Add In List : ");
            int n = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i < n; i++)
            {
                Employee e = new Employee();
                Console.Write("Enter Name : ");
                e.Name = Console.ReadLine();
                Console.Write("Enter Age : ");
                e.Age = Console.ReadLine();
                list.Add(e);
            }

        }

        static void ConvertIntoJson()
        {

            StreamWriter sw = new StreamWriter(new FileStream("New.json", FileMode.Create));
            var s = JsonConvert.SerializeObject(list);
            sw.Write(s);
            sw.Close();
        }

        static void ConverIntoString()
        {
            string json = File.ReadAllText("New.json");
            Employee[] s1 = JsonConvert.DeserializeObject<Employee[]>(json);
            int i = 1;
            var subject = new Subject<Employee>();
            subject.Subscribe(
            obj  => Console.WriteLine(obj.Name+" "+obj.Age),
            () => Console.WriteLine("Done"));
            foreach(var s in s1)
            subject.OnNext(s);

        }
        static void Main(string[] args)
        {
            Add();
            ConvertIntoJson();
            ConverIntoString();
            Console.ReadKey();
        }
    }
   
   
}
