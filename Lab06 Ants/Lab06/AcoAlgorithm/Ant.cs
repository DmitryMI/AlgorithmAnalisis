using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab06.AcoAlgorithm
{
    class Ant
    {
        private int _currentCity;
        private List<int> _path = new List<int>();
        private Graph _graph;
        private PheromonMatrix _pheromonMatrix;
        private AcoProcessor.AcoParameters _parameters;
        private Random _rnd = new Random();
        private bool _isElite = false;

        public Ant(int initialCity, Graph graph, PheromonMatrix pheromonMatrix, AcoProcessor.AcoParameters parameters)
        {
            _currentCity = initialCity;
            _graph = graph;
            _pheromonMatrix = pheromonMatrix;
            _parameters = parameters;
        }

        public bool IsElite
        {
            get => _isElite;
            set => _isElite = value;
        }

        private double GetProbability(int destCity, List<int> available)
        {
            double inversedDistance = _parameters.q / _graph[_currentCity, destCity];
            double pheromon = _pheromonMatrix[_currentCity, destCity];

            inversedDistance = Math.Pow(inversedDistance, _parameters.beta);
            
            pheromon = Math.Pow(pheromon, _parameters.alpha);

            double summ = 0;

            foreach (var city in available)
            {
                double id = _parameters.q / _graph[_currentCity, city];
                double ph = _pheromonMatrix[_currentCity, city];

                id = Math.Pow(id, _parameters.beta);
                ph = Math.Pow(ph, _parameters.alpha);

                summ += id * ph;
            }

            return inversedDistance * pheromon / summ;
        }

        private List<KeyValuePair<int, double>> GetProbabilities(List<int> available)
        {
            List < KeyValuePair<int, double> > result = new List<KeyValuePair<int, double>>();

            foreach (var city in available)
            {
                double prob = GetProbability(city, available);
                KeyValuePair<int, double> pair = new KeyValuePair<int, double>(city, prob);
                result.Add(pair);
            }

            return result;
        }

        private int ProbabilisticChoise(List<KeyValuePair<int, double>> citiesProbs)
        {
            double prevPoint = 0;
            double rnd = _rnd.NextDouble();
            foreach (var cityProb in citiesProbs)
            {
                if (prevPoint <= rnd && rnd < prevPoint + cityProb.Value)
                    return cityProb.Key;
                prevPoint += cityProb.Value;
            }

            throw new ArgumentException();
        }

        private int EliteChoise(List<KeyValuePair<int, double>> citiesProbs)
        {
            double max = citiesProbs[0].Value;
            int maxIndex = 0;
            foreach (var cityProb in citiesProbs)
            {
                if (cityProb.Value > max)
                {
                    max = cityProb.Value;
                    maxIndex = cityProb.Key;
                }
            }

            return maxIndex;
        }

        public int[] Path => _path.ToArray();

        public double GetLength()
        {
            double length = 0;
            for (int i = 1; i < _path.Count; i++)
            {
                int start = _path[i - 1];
                int end = _path[i];
                length += _graph[start, end];
            }

            return length;
        }

        public void DoPheromon()
        {
            double length = GetLength();

            double dt = _parameters.q / length;

            for (int i = 1; i < _path.Count; i++)
            {
                int start = _path[i - 1];
                int end = _path[i];
                length += _graph[start, end];
                _pheromonMatrix[start, end] *= (1 - _parameters.p);
                _pheromonMatrix[start, end] += dt;

                if (_pheromonMatrix[start, end] < _parameters.pheromonMin)
                    _pheromonMatrix[start, end] = _parameters.pheromonMin;
            }
        }

        public bool CanMove()
        {
            List<int> available = _graph.GetConnections(_currentCity);
            available = Utils.Intersect(available, _path);

            return available.Count != 0;
        }

        public void Move()
        {
            List<int> available = _graph.GetConnections(_currentCity);
            available = Utils.Intersect(available, _path);

            List<KeyValuePair<int, double>> citiesProbs = GetProbabilities(available);

            int nexCity;

            if (_isElite)
            {
                nexCity = EliteChoise(citiesProbs);
            }
            else
            {
                nexCity = ProbabilisticChoise(citiesProbs);
            }

            _path.Add(nexCity);
            _currentCity = nexCity;
        }
    }
}
