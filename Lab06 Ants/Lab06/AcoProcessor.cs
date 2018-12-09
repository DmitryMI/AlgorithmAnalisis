using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab06
{
    class AcoProcessor
    {

        public struct AcoParameters
        {
            public double alpha, beta, q, tMax;
        }

        private class PheromonMatrix
        {
            private double[,] _matrix;

            public double this[int i, int j]
            {
                get => _matrix[i, j];
                set => _matrix[i, j] = value;
            }

            public override string ToString()
            {
                string result = "";
                for (int i = 0; i < _matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < _matrix.GetLength(1); j++)
                    {
                        result += _matrix[i, j] + " ";
                    }
                    result += '\n';
                }

                return result;
            }

            public PheromonMatrix(int size)
            {
                _matrix = new double[size, size];
            }

            public void NextTimeStep()
            {

            }
        }

        private class PathContainer
        {
            private List<int>[] _paths;
            
            public PathContainer(int count)
            {
                _paths = new List<int>[count];
                for (int i = 0; i < count; i++)
                    _paths[i] = new List<int>();
            }

            public double GetPath(int ant, int time)
            {
                return _paths[ant][time];
            }

            public void AddPath(int ant, int city)
            {
                _paths[ant].Add(city);
            }
        }

        public class LengthMatrix
        {
            private double[,] _matrix;

            public LengthMatrix(int size)
            {
                _matrix = new double[size, size];

                for(int i = 0; i < size; i++)
                {
                    for(int j = 0; j < size; j++)
                    {
                        _matrix[i, j] = Double.PositiveInfinity;
                    }
                }
            }

            public double this[int i, int j]
            {
                get => _matrix[i, j];
                set => _matrix[i, j] = value;
            }

            public override string ToString()
            {
                string result = "";
                for (int i = 0; i < _matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < _matrix.GetLength(1); j++)
                    {
                        result += _matrix[i, j] + " ";
                    }
                    result += '\n';
                }

                return result;
            }
        }




        ConnectionMatrix _connectionMatrix;
        AcoParameters _params;
        PheromonMatrix _pheromonMatrix;
        LengthMatrix _lengthMatrix;
        Random _rnd = new Random();
        List<int>[] _antsTargets;

        int[] _antsCities;

        public AcoProcessor(ConnectionMatrix matrix)
        {
            _connectionMatrix = matrix;            
        }

        private void RandomizeAnts()
        {
            int count = _connectionMatrix.Size;
            _antsTargets = new List<int>[count];
            for(int i = 0; i < count; i++)
            {
                _antsCities[i] = i;
                _antsTargets[i] = new List<int>();
                for(int j = 0; j < count; j++)
                {
                    if (i != j)
                        _antsTargets[i].Add(j);
                }
            }
        }

        public LengthMatrix Process(AcoParameters parameters)
        {
            _params = parameters;

            _pheromonMatrix = new PheromonMatrix(_connectionMatrix.Size);
            _lengthMatrix = new LengthMatrix(_connectionMatrix.Size);

            RandomizeAnts();

            DoJob();

            return _lengthMatrix;
        }

        private double GetProbability(int ant, int cityI, int cityJ)
        {
            double p;
            if(_antsTargets[ant].Contains(cityJ))
            {
                double tau = _pheromonMatrix[cityI, cityJ];
                double n = 1.0d / _connectionMatrix[cityI, cityJ];
                double tauAlpha = Math.Pow(tau, _params.alpha);
                double nBeta = Math.Pow(n, _params.beta);
                double upPart = tauAlpha * nBeta;

                double lowerPart = 0;
                foreach(int q in _antsTargets[ant])
                {
                    double tauQ = _pheromonMatrix[cityI, q];
                    double nQ = _pheromonMatrix[cityI, q];
                    double tauQAlpha = Math.Pow(tauQ, _params.alpha);
                    double nQBeta = Math.Pow(nQ, _params.beta);
                    lowerPart += tauQAlpha * nQBeta;
                }

                p = upPart / lowerPart;
            }
            else
            {
                p = 0;
            }

            return p;
        }

        private double PheromonDelta(int ant, int i, int j)
        {
            return _params.q / _connectionMatrix[i, j];
        }

        private int GetRandomIndex(double[] probabilities)
        {
            double r = _rnd.NextDouble();

            double currentStart = 0;

            for(int i = 0; i < probabilities.Length; i++)
            {
                if (currentStart <= r && r < currentStart + probabilities[i])
                    return i;
            }

            throw new ArgumentException();
        }

        private int RandomizePath(int ant)
        {
            int count = _antsTargets[ant].Count;
            int currentCity = _antsCities[ant];
            double[] probabilities = new double[count];

            for(int i = 0; i < count; i++)
            {
                probabilities[i] = GetProbability(ant, currentCity, i);
            }

            int city = GetRandomIndex(probabilities);

            return city;
        }

        private void DoJob()
        {
            for(int time = 0; time < _params.tMax; time++)
            {
                for(int ant = 0; ant < _antsCities.Length; ant++)
                {
                    // Building path
                    int nextCity = RandomizePath(ant);
                    double length = _connectionMatrix[_antsCities[ant], nextCity];

                }
            }
        }

        
    }
}
