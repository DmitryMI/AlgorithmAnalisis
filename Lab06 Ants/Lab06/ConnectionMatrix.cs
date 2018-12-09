using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab06
{
    class ConnectionMatrix
    {
        double[,] _distances;

        public ConnectionMatrix(int citiesCount)
        {
            _distances = new double[citiesCount, citiesCount];

            for(int i = 0; i < citiesCount; i++)
                for(int j = 0; j < citiesCount; j++)
                {
                    _distances[i, j] = Double.PositiveInfinity;
                }
        }

        public void SetDistance(int cityA, int cityB, double distance)
        {
            _distances[cityA, cityB] = distance;
            _distances[cityB, cityA] = distance;
        }

        public double GetDistance(int cityA, int cityB)
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
                    result += _distances[i, j] + " ";
                }
                result += '\n';
            }

            return result;
        }

        public int Size => _distances.GetLength(0);

    }
}
