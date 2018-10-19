using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab03.Matrixes
{
    unsafe class FastMatrix
    {
        private IntPtr _buffer;

        private int _rows, _cols;

        private int _lineByteLength;

        public FastMatrix(int rows, int cols, IntPtr _linearBuffer)
        {
            _rows = rows;
            _cols = cols;
            _lineByteLength = _cols * sizeof(int);
            _buffer = _linearBuffer;
        }

        public IntPtr GetLinePointer(int rowIndex)
        {
            return _buffer + _lineByteLength * rowIndex;
        }
    }
}
