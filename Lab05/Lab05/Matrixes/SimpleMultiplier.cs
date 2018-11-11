using System.Diagnostics;

namespace Lab05.Matrixes
{
    class SimpleMultiplier : MatrixMultiplier
    {
        private long _ticks;
        private int[,] _matrixBuffer;
        
        public override long Ticks => _ticks;

        public override Matrix Multiply(Matrix a, Matrix b)
        {
            if (_matrixBuffer == null)
                _matrixBuffer = new int[a.Cols, b.Rows];
            

            if(a.Cols != _matrixBuffer.GetLength(0) || b.Rows != _matrixBuffer.GetLength(1))
                _matrixBuffer = new int[a.Cols, b.Rows];

            Stopwatch timer = new Stopwatch();
            timer.Start();
            Matrix result = Process(a, b);
            timer.Stop();

            _ticks = timer.ElapsedTicks;

            return result;
        }

        private Matrix Process(Matrix a, Matrix b)
        {
            Matrix r = new Matrix(a.Cols, b.Rows, _matrixBuffer);

            for (int i = 0; i < r.Rows; i++)
            {
                for (int j = 0; j < r.Cols; j++)
                {
                    r[i, j] = 0;
                    for (int k = 0; k < a.Rows; k++)
                    {
                        r[i, j] = r[i,j] +  a[i, k] * b[k, j];
                    }
                }
            }

            return r;
        }
    }
}
