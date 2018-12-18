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

        static void DoLog(int threadNumber, int taskNumber, bool stage)
        {
            if(stage)
            {
                Console.WriteLine("Поток {0}\t принял задачу \t{1}\t в \t{2}", threadNumber, taskNumber, DateTime.Now);
            }
            else
            {
                Console.WriteLine("Поток {0}\t завер. задачу \t{1}\t в \t{2}", threadNumber, taskNumber, DateTime.Now);
            }
        }

        static object FastIncrementer(object val)
        {
            object[] argArray = (object[])val;
            int data = (int)argArray[0];
            DoLog(1, (int)argArray[1], true);           
            Thread.Sleep(1000);
            DoLog(1, (int)argArray[1], false);
            data++;
            //return (int)data + 1;
            return new object[] { data, (int)argArray[1] };
        }

        static object MediumIncrementer(object val)
        {
            object[] argArray = (object[])val;
            int data = (int)argArray[0];
            DoLog(2, (int)argArray[1], true);
            Thread.Sleep(3000);
            DoLog(2, (int)argArray[1], false);
            //return (int)data + 1;
            data++;
            return new object[] { data, (int)argArray[1] };
        }

        static object SlowIncrementer(object val)
        {
            object[] argArray = (object[])val;
            int data = (int)argArray[0];
            DoLog(3, (int)argArray[1], true);
            Thread.Sleep(5000);
            DoLog(3, (int)argArray[1], false);
            data++;
            return new object[] { data, (int)argArray[1] };
        }

        static void Experiment()
        {
            DateTime start = DateTime.Now;
            DoInConveyer();

            TimeSpan span = DateTime.Now - start;

            Console.WriteLine("Conveyer time: " + span);

            return;

            start = DateTime.Now;
            DoLinearly();
            span = DateTime.Now - start;

            Console.WriteLine("Linear time: " + span);

            Console.ReadKey();
        }

        static void DoLinearly()
        {
            throw new NotImplementedException(); // New Arguments

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

            //int[] init = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            object[][] init = new object[][] {
                new object[] {1, 1 },
                new object[] {1, 2},
                new object[] {1, 3},
                new object[] {1, 4},
                new object[] {1, 5},
                new object[] {1, 6},
                new object[] {1, 7}
            };

            for (int i = 0; i < init.Length; i++)
            {
                conveyer.WriteNext(init[i]);
                Console.WriteLine("Основной поток: поставил задачу на конвейер");
            }

            for (int i = 0; i < init.Length; i++)
            {
                while (true)
                {
                    if (conveyer.HasNextResult())
                    {
                        object[] resultArr = (object[])conveyer.ReadNextResult();
                        int result = (int) resultArr[0];
                        int takNum = (int)resultArr[1];
                        Console.WriteLine("Основной поток: данные получены о задаче " + takNum + " в " + DateTime.Now);
                        Console.WriteLine("Result " + (i + 1) + ": " + result);
                        break;
                    }
                }
            }

            conveyer.StopConveyer();
        }
    }
}
