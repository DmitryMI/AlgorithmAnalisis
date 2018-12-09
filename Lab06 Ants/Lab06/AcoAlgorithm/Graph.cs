using System;
using System.Collections.Generic;

namespace Lab06.AcoAlgorithm
{
    class Graph
    {
        double[,] _distances;

        public Graph(int citiesCount)
        {
            _distances = new double[citiesCount, citiesCount];

            for(int i = 0; i < citiesCount; i++)
                for(int j = 0; j < citiesCount; j++)
                {
                    _distances[i, j] = Double.PositiveInfinity;
                }
        }

        private void SetDistance(int cityA, int cityB, double distance)
        {
            _distances[cityA, cityB] = distance;
            _distances[cityB, cityA] = distance;
        }

        private double GetDistance(int cityA, int cityB)
        {
            return _distances[cityA, cityB];
        }

        public double this[int i, int j]
        {
            get => GetDistance(i, j);
            set => SetDistance(i, j, value);
        }

        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < _distances.GetLength(0); i++)
            {
                for (int j = 0; j < _distances.GetLength(1); j++)
                {
                    //result += _distances[i, j] + " ";
                    string dist = _distances[i, j].ToString("F");
                    result += dist + " ";
                }
                result += '\n';
            }

            return result;
        }

        public int Size => _distances.GetLength(0);

        public bool HasConnection(int i, int j)
        {
            double distance = this[i, j];
            return !Double.IsInfinity(distance);
        }

        public List<int> GetConnections(int city)
        {
            List<int> result = new List<int>();

            for (int i = 0; i < Size; i++)
            {
                if(HasConnection(city, i) && city != i)
                    result.Add(i);
            }

            return result;
        }

        public static class Builder
        {
            private static Random _rnd = new Random();

            public static Graph GetRandomPentagram()
            {
                Graph graph = new Graph(5);
                for (int i = 0; i < 5; i++)
                {
                    for (int j = i; j < 5; j++)
                    {
                        graph.SetDistance(i, j, _rnd.NextDouble() * 9 + 1);
                    }
                }

                return graph;
            }
        }

    }
}
