using System;
using System.Collections.Generic;

namespace Lab06.AcoAlgorithm
{
    class AcoProcessor
    {
        private Graph _graph;
        private AcoParameters _acoParameters;

        public struct AcoParameters
        {
            public double alpha, beta, q, tMax, p, pheromonMin, pheromonInitial;

            public override string ToString()
            {
                string result = "alpha: " + alpha + ", beta: " + beta + ", q: " + q + ", " + tMax + ", p: " + p;
                return result;
            }
        }

        public AcoProcessor(Graph graph)
        {
            _graph = graph;
        }

        public double GetShortestPath(AcoParameters parameters, out int[] path)
        {
            _acoParameters = parameters;

            PheromonMatrix pheromonMatrix = new PheromonMatrix(_graph.Size, parameters.pheromonInitial);

            double shortest = Double.PositiveInfinity;
            int[] shortestPath = null;

            for (int i = 0; i < parameters.tMax; i++)
            {
                Ant ant = new Ant(0, _graph, pheromonMatrix, parameters);
                while(ant.CanMove())
                    ant.Move();
                ant.DoPheromon();

                double antPathLen = ant.GetLength();
                if (antPathLen < shortest)
                {
                    shortest = antPathLen;
                    shortestPath = ant.Path;
                }
            }

            path = shortestPath;

            return shortest;
        }
        
    }
}
