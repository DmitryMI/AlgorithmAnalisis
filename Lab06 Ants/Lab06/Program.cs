using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab06.AcoAlgorithm;

namespace Lab06
{
    class Program
    {
        private const double AlphaMin = 0.1;
        private const double AlphaMax = 1;
        private const double C = 1;
        private const double RoMin = 0.1;
        private const double RoMax = 1;
        private const int TMin = 1;
        private const int TMax = 5;
        private const int Iterations = 10;


        static void Main(string[] args)
        {
            Experiment();
            Console.ReadKey();
        }

        private static void Test()
        {
            Graph graph = Graph.Builder.GetRandomPentagram();

            AcoProcessor processor = new AcoProcessor(graph);

            AcoProcessor.AcoParameters parameters = new AcoProcessor.AcoParameters();
            parameters.beta = 1;
            parameters.alpha = 1;
            parameters.pheromonMin = 0.2;
            parameters.q = 1;
            parameters.tMax = 10;
            parameters.p = 0.1;
            parameters.pheromonInitial = 1;

            int[] path;

            double result = processor.GetShortestPath(parameters, out path);

            Console.WriteLine("Graph matrix: ");
            Console.WriteLine(graph.ToString());

            Console.WriteLine("Result: " + result);

            Console.WriteLine("Result path: " + Utils.PathToString(path));

            Console.ReadKey();
        }

        private static void Experiment()
        {
            Bruteforcer bruteforcer = new Bruteforcer(AlphaMin, AlphaMax, C, RoMin, RoMax, TMin, TMax, Iterations);

            AcoProcessor.AcoParameters parameters = new AcoProcessor.AcoParameters();
            parameters.pheromonMin = 0.2;
            parameters.q = 1;
            parameters.pheromonInitial = 1;

            AcoProcessor.AcoParameters[] list = bruteforcer.Iterate(parameters);

            List<double> results = new List<double>();

            Graph graph = Graph.Builder.GetRandomPentagram();

            AcoProcessor processor = new AcoProcessor(Graph.Builder.GetRandomPentagram());

            double shortestPath = double.PositiveInfinity;
            AcoProcessor.AcoParameters optimal = new AcoProcessor.AcoParameters();

            foreach (var param in list)
            {
                int[] path;
                double result = processor.GetShortestPath(param, out path);
                results.Add(result);

                if(result < shortestPath)
                {
                    shortestPath = result;
                    optimal = param;
                }
            }

            string output = Utils.ParamsToTable(list, results);
            Console.WriteLine(output);

            Console.WriteLine("Graph: ");
            Console.WriteLine(graph);

            File.WriteAllLines("out.txt", output.Split('\n'));

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Optimal: " + optimal.ToString() + ", result: " + shortestPath);
        }
    }
}
