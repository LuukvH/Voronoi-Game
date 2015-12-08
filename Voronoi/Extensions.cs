using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voronoi
{
    public static class Extensions
    {
        private const float Epsilon = 0.01f;

        public static bool IsZero(this float d)
        {
            return Math.Abs(d) < Epsilon;
        }

        public static bool NearlyEqual(float a, float b)
        {
            return NearlyEqual(a, b, Epsilon);
        }

        public static bool NearlyEqual(float a, float b, float epsilon)
        {
            // Compare the values
            // The output to the console indicates that the two values are equal
            float v = Math.Abs(a - b);

            if (Math.Abs(a - b) <= epsilon)
                return true;
            else
                return false;
        }
    }
}
