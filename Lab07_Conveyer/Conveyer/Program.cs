using System;
using System.Collections.Generic;
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
            Conveyer.Job[] jobs = new Conveyer.Job[]
            {
                Incrementer, Incrementer, Incrementer
            };

            Conveyer conveyer = new Conveyer(jobs);

            conveyer.WriteNext(1);
            conveyer.WriteNext(2);
            conveyer.WriteNext(3);

            while (true)
            {
                if (conveyer.HasNextResult())
                {
                    int result = (int) conveyer.ReadNextResult();
                    Console.WriteLine("Result 1: " + result);
                    break;
                }
            }

            while (true)
            {
                if (conveyer.HasNextResult())
                {
                    int result = (int)conveyer.ReadNextResult();
                    Console.WriteLine("Result 2: " + result);
                    break;
                }
            }

            while (true)
            {
                if (conveyer.HasNextResult())
                {
                    int result = (int)conveyer.ReadNextResult();
                    Console.WriteLine("Result 3: " + result);
                    break;
                }
            }

            conveyer.StopConveyer();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static object Incrementer(object val)
        {
            Thread.Sleep(_rnd.Next(1, 5) * 1000);
            return (int) val + 1;
        }
    }
}
