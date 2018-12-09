using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab06.AcoAlgorithm
{
    class PheromonMatrix
    {
        double[,] _pheromons;

        public PheromonMatrix(int citiesCount, double initial)
        {
            _pheromons = new double[citiesCount, citiesCount];

            for (int i = 0; i < citiesCount; i++)
            for (int j = 0; j < citiesCount; j++)
            {
                _pheromons[i, j] = initial;
            }
        }

        private void SetPheromon(int cityA, int cityB, double pheromon)
        {
            _pheromons[cityA, cityB] = pheromon;
            _pheromons[cityB, cityA] = pheromon;
        }

        private double GetPheromon(int cityA, int cityB)
        {
            return _pheromons[cityA, cityB];
        }

        public double this[int i, int j]
        {
            get => GetPheromon(i, j);
            set => SetPheromon(i, j, value);
        }

        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < _pheromons.GetLength(0); i++)
            {
                for (int j = 0; j < _pheromons.GetLength(1); j++)
                {
                    result += _pheromons[i, j] + " ";
                }
                result += '\n';
            }

            return result;
        }

        public int Size => _pheromons.GetLength(0);
    }
}
