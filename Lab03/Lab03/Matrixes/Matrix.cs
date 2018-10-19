
using System;
using System.Runtime.InteropServices;

namespace Lab03.Matrixes
{
    class Matrix
    {
        private readonly int _rows;
        private readonly int _cols;
        private readonly int[,] _data;

        private static Random _rnd = new Random();

        public int this[int i, int j]
        {
            get => _data[i, j];
            set => _data[i, j] = value;
        }

        public Matrix(int rows, int cols)
        {
            _rows = rows;
            _cols = cols;

            _data = new int[rows, cols];
        }

        public Matrix(int rows, int cols, int[,] buffer)
        {
            _rows = rows;
            _cols = cols;

            _data = buffer;
        }

        public int Rows => _rows;
        public int Cols => _cols;

        public bool IsSquare => _rows == _cols;


        public override string ToString()
        {
            string result = "";

            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    result += _data[i, j];
                    result += ' ';
                }

                if(i < _rows - 1)
                    result += '\n';
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            if (obj is Matrix other)
            {
                bool dimensionsCheck = _rows == other._rows && _cols == other._cols;
                if (!dimensionsCheck)
                    return false;

                for (int i = 0; i < _rows; i++)
                {
                    for (int j = 0; j < _cols; j++)
                    {
                        if (_data[i, j] != other[i, j])
                            return false;
                    }
                }

                return true;
            }

            return false;
        }

        protected bool Equals(Matrix other)
        {
            return _rows == other._rows && _cols == other._cols && Equals(_data, other._data);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _rows;
                hashCode = (hashCode * 397) ^ _cols;
                hashCode = (hashCode * 397) ^ (_data != null ? _data.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static void GenerateRandom(Matrix matrix, int min, int maxExclusive)
        {
            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Cols; j++)
                {
                    matrix[i, j] = _rnd.Next(min, maxExclusive);
                }
            }
        }
    }
}
