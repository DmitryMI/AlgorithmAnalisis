using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab04.Sorting;

namespace Lab04
{
    class Program
    {
        static Random _rnd = new Random();

        static string ArrToString(int[] arr)
        {
            string res = "";
            foreach (var el in arr)
            {
                res += el.ToString();
                res += ' ';
            }

            return res;
        }

        static int[] GenerateArray(int len, int min, int max)
        {
            int[] arr = new int[len];
            for (int i = 0; i < len; i++)
            {
                arr[i] = _rnd.Next(min, max);
            }
            return arr;
        }

        static int[] GetInvertedArray(int len)
        {
            int[] arr = new int[len];
            for (int i = 0; i < len; i++)
            {
                arr[i] = len - i;
            }
            return arr;
        }

        static void Main(string[] args)
        {
            //TestMergeSort();
            //TestTreeSort();
            //TestRadixSort();
            Analise();

            Console.ReadKey();
        }

        static void Analise()
        {
            // Measuring merge sorter
            Sorter merge = new MergeSorter();
            Sorter tree = new TreeSorter();
            Sorter radix = new RadixSorter();

            var mergeTimes = AnaliseSorterRandom(merge);
            var treeTimes = AnaliseSorterRandom(tree);
            var radixTimes = AnaliseSorterRandom(radix);

            File.WriteAllLines("mergeRandom.txt", ToStringArray(mergeTimes));
            File.WriteAllLines("treeRandom.txt", ToStringArray(treeTimes));
            File.WriteAllLines("radixRandom.txt", ToStringArray(radixTimes));

            mergeTimes = AnaliseSorterReverted(merge);
            treeTimes = AnaliseSorterReverted(tree);
            radixTimes = AnaliseSorterReverted(radix);

            File.WriteAllLines("mergeReverted.txt", ToStringArray(mergeTimes));
            File.WriteAllLines("treeReverted.txt", ToStringArray(treeTimes));
            File.WriteAllLines("radixReverted.txt", ToStringArray(radixTimes));
        }

        static string[] ToStringArray<T>(T[] ints)
        {
            string[] res = new string[ints.Length];
            for (int i = 0; i < ints.Length; i++)
            {
                res[i] = ints[i].ToString();
            }

            return res;
        }

        static long[] AnaliseSorterReverted(Sorter sorter)
        {
            long[] times = new long[10];
            int count = 0;
            for (int i = 100; i <= 1000; i += 100)
            {
                int[] randomArray = GetInvertedArray(i);
                long time = 0;
                for (int iter = 0; iter < 100; iter++)
                {
                    sorter.Sort(randomArray);
                    time += sorter.LastOperationTicks;
                }
                time /= 100;
                times[count] = time;
                count++;
                Console.WriteLine(sorter.GetType().Name + ", " + i + ": " + time);
            }

            return times;
        }

        static long[] AnaliseSorterRandom(Sorter sorter)
        {
            long[] times = new long[10];
            int count = 0;
            for (int i = 100; i <= 1000; i += 100)
            {
                int[] randomArray = GenerateArray(i, 0, i);
                long time = 0;
                for (int iter = 0; iter < 100; iter++)
                {
                    sorter.Sort(randomArray);
                    time += sorter.LastOperationTicks;
                }
                time /= 100;
                times[count] = time;
                count++;
                Console.WriteLine(sorter.GetType().Name + ", " + i + ": " + time);
            }

            return times;
        }

        static void TestMergeSort()
        {
            try
            {
                Console.WriteLine("Testing merge sort: ");
                Sorter merge = new MergeSorter();
                int[] arr = GenerateArray(10, 0, 10);
                Console.WriteLine("Source array: " + ArrToString(arr));
                merge.Sort(arr);
                Console.WriteLine("Resulting array: " + ArrToString(arr));
                Console.WriteLine("OK!");
            }
            catch (Sorter.InvalidRealizationException ex)
            {
                Console.WriteLine(ex.Info);
            }
        }

        static void TestTreeSort()
        {
            try
            {
                Sorter sorter = new TreeSorter();
                Console.WriteLine("Testing sort: " + sorter.GetType().Name);

                int[] arr = GenerateArray(10, 0, 10);
                Console.WriteLine("Source array: " + ArrToString(arr));
                sorter.Sort(arr);
                Console.WriteLine("Resulting array: " + ArrToString(arr));
                Console.WriteLine("OK!");
                
            }
            catch (Sorter.InvalidRealizationException ex)
            {
                Console.WriteLine(ex.Info);
            }
        }

        static void TestRadixSort()
        {
            try
            {
                Sorter sorter = new RadixSorter();
                Console.WriteLine("Testing sort: " + sorter.GetType().Name);
                
                int[] arr = GenerateArray(10, 0, 10);
                Console.WriteLine("Source array: " + ArrToString(arr));
                sorter.Sort(arr);
                Console.WriteLine("Resulting array: " + ArrToString(arr));
                Console.WriteLine("OK!");

            }
            catch (Sorter.InvalidRealizationException ex)
            {
                Console.WriteLine(ex.Info);
            }
        }
    }
}
