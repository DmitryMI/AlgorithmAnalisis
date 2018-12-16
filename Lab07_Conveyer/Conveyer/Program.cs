using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConveyerLab.ConveyerProcessing;

namespace ConveyerLab
{
    class Program
    {
        private static Random _rnd = new Random();

        static void Main(string[] args)
        {
            Experiment();
        }

        static object Incrementer(object val)
        {
            Thread.Sleep(_rnd.Next(1, 5) * 1000);
            return (int) val + 1;
        }

        static object FastIncrementer(object val)
        {
            Thread.Sleep(1000);
            return (int)val + 1;
        }

        static object MediumIncrementer(object val)
        {
            Thread.Sleep(3000);
            return (int)val + 1;
        }

        static object SlowIncrementer(object val)
        {
            Thread.Sleep(5000);
            return (int)val + 1;
        }

        static void Experiment()
        {
            DateTime start = DateTime.Now;
            DoInConveyer();

            TimeSpan span = DateTime.Now - start;

            Console.WriteLine("Conveyer time: " + span);

            start = DateTime.Now;
            DoLinearly();
            span = DateTime.Now - start;

            Console.WriteLine("Linear time: " + span);

            Console.ReadKey();
        }

        static void DoLinearly()
        {
            int[] init = new int[]{1, 2, 3,4, 5, 6, 7, 8, 9};
            for (int i = 0; i < init.Length; i++)
            {
                int result = (int)FastIncrementer(MediumIncrementer(SlowIncrementer(init[i])));

                Console.WriteLine("Result " + (i + 1) + ": " + result);
            }
        }

        static void DoInConveyer()
        {
            Conveyer.Job[] jobs = new Conveyer.Job[]
            {
                FastIncrementer, MediumIncrementer, SlowIncrementer
            };

            Conveyer conveyer = new Conveyer(jobs);

            int[] init = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            for (int i = 0; i < init.Length; i++)
            {
                conveyer.WriteNext(init[i]);
            }

            for (int i = 0; i < init.Length; i++)
            {
                while (true)
                {
                    if (conveyer.HasNextResult())
                    {
                        int result = (int) conveyer.ReadNextResult();
                        Console.WriteLine("Result " + (i + 1) + ": " + result);
                        break;
                    }
                }
            }

            conveyer.StopConveyer();
        }
    }
}
