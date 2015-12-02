using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voronoi
{
    public class Vertex
    {

        public float x;
        public float y;

        public Vertex()
        {
            x = float.NaN;
            y = float.NaN;
        }

        public Vertex (float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public Vertex(Vertex v)
        {
            x = v.x;
            y = v.y;
        }

        public bool isNan()
        {
            return float.IsNaN(x) || float.IsNaN(y);
        }

        #region Operators
        public static Vertex operator -(Vertex v, Vertex w)
        {
            return new Vertex(v.x - w.x, v.y - w.y);
        }

        public static Vertex operator +(Vertex v, Vertex w)
        {
            return new Vertex(v.x + w.x, v.y + w.y);
        }

        public static float operator *(Vertex v, Vertex w)
        {
            return v.x * w.x + v.y * w.y;
        }

        public static Vertex operator *(Vertex v, float mult)
        {
            return new Vertex(v.x * mult, v.y * mult);
        }

        public static Vertex operator *(float mult, Vertex v)
        {
            return new Vertex(v.x * mult, v.y * mult);
        }

        public float Cross(Vertex v)
        {
            return x * v.y - y * v.x;
        }
        #endregion

    }
}
