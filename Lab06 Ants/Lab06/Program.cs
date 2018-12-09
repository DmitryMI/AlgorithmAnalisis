﻿using System;
using System.Collections.Generic;
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
        private const int TMin = 5;
        private const int TMax = 10;
        private const int Iterations = 5;


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

            AcoProcessor processor = new AcoProcessor(Graph.Builder.GetRandomPentagram());

            foreach (var param in list)
            {
                int[] path;
                double result = processor.GetShortestPath(param, out path);
                results.Add(result);
            }

            string output = Utils.ParamsToTable(list, results);
            Console.WriteLine(output);
        }
    }
}
