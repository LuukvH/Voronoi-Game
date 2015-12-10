using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voronoi
{
    public class IncorrectTriangleException : Exception
    {
        public IncorrectTriangleException()
        {
        }

        public IncorrectTriangleException(string message)
            : base(message)
        {
        }

        public IncorrectTriangleException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
