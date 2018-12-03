using System.Diagnostics;

namespace Lab03.Matrixes
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

            // i = 0 - запись в память
            for (int i = 0; i < r.Rows; i++) // При каждой итерации: сравнение, инкремент (2)
            {
                // запись в память
                for (int j = 0; j < r.Cols; j++) // При каждой итерации: сравнение, инкремент (2)
                {
                    r[i, j] = 0; // Запись в память
                    // Запись в память
                    for (int k = 0; k < a.Rows; k++) // При каждой итерации: сравнение, инкремент (2)
                    {

                        r[i, j] = r[i, j] + a[i, k] * b[k, j]; // При каждой итерации: чтение, чтение, чтение, умножение, сложение, запись (6)

                    }
                }
            }

            return r; // Запись в память
        }
    }
}
