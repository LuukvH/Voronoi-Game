using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voronoi
{
    public static class Extensions
    {
        private const float Epsilon = 1e-10f;

        public static bool IsZero(this float d)
        {
            return Math.Abs(d) < Epsilon;
        }
    }
}
