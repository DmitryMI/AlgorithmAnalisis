using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab05.Matrixes.MatrixesParallel
{
    struct JobPart
    {
        private int _start, _end;
        private bool _occupied;

        public int Start => _start;
        public int End => _end;

        public bool Occupied
        {
            get => _occupied;
            set => _occupied = value;
        }

        public JobPart(int start, int end)
        {
            _start = start;
            _end = end;
            _occupied = false;
        }
    }
}
