using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        //[ThreadStatic]
        static int count = 0;
        //[ThreadStatic]
        static int[] array = new int[Environment.ProcessorCount];

        static object o = new object();
        static int start = 1000_000_000;
        static int end = 2000_000_000;


        class MyClass
        {
            public int start { get; set; }
            public int stop { get; set; }
            public MyClass(int start, int stop)
            {
                this.start = start;
                this.stop = stop;
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("The number of processors on this computer is {0}.",Environment.ProcessorCount, Encoding.UTF8);


            Stopwatch sw = new Stopwatch();
            //sw.Start();
            //for (int i = start; i <= end; i++)
            //{
            //    if (IsSumMultipleLastNumber(i)) count++;
            //}
            //sw.Stop();
            //Console.WriteLine($"(SingleThtread)Количество чисел между {start} и {end}: {count}\nВремя:{sw.ElapsedMilliseconds}\n",Encoding.UTF8);


            //count = 0;
            //sw.Restart();

            //Parallel.For(start, end, (i) =>
            // {
            //     if (IsSumMultipleLastNumber(i))
            //     {
            //         lock (o)
            //         {
            //             count++;
            //         }
            //     }
            // });
            //sw.Stop();

            //Console.WriteLine($"(Parallel)Количество чисел между {start} и {end}: {count}\nВремя:{sw.ElapsedMilliseconds}\n", Encoding.UTF8);

            count = 0;
            sw.Restart();

            Parallel.For(0, Environment.ProcessorCount, (i) =>
            {
                
                array[i]=Task<int>.Factory.StartNew(Method, new MyClass(start + (i * (start/ Environment.ProcessorCount)), start + ((i+1) * (start/ Environment.ProcessorCount)))).Result;
                
            });
            foreach(var e in array)
            {
                count += e;
            }
            Console.WriteLine($"Task<int>.Factory.StartNew. Количество чисел между {start} и {end}: {count}\nВремя:{sw.ElapsedMilliseconds}\n",Encoding.UTF8);
            sw.Stop();


            Console.ReadKey();
        }
        static int Method(object m)
        {
            int start = (m as MyClass).start;
            int stop = (m as MyClass).stop;
            int temp = 0;
            for (int i = start; i <= stop; i++)
            {
                if (IsSumMultipleLastNumber(i))
                {
                    temp++;
                }
        }
            return temp;
        }

        static bool IsSumMultipleLastNumber(int x)
        {
            {
                int sum = 0;
                int lastElem = x % 10;
                
                switch (lastElem)
                {
                    case 0: return false;
                    case 1: return true;
                    default:
                        {
                            while (x != 0)
                            {
                                sum += x % 10;
                                x /= 10;
                            }
                            return sum % lastElem == 0;
                            
                        }   
                }
            }
        }
    }
}
