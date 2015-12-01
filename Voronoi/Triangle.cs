using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voronoi
{
    public class Triangle : Face
    {
        public Triangle(HalfEdge halfEdge) : base(halfEdge)
        {

        }

        public Vertex Circumcenter()
        {
            Vertex v1 = HalfEdge.Origin;
            Vertex v2 = HalfEdge.Next.Origin;
            Vertex v3 = HalfEdge.Next.Next.Origin;

            // Todo: Here we should add some x1-x2 > 0 logic to choose one of these, instead of trying
            Vertex c = Circumcenter(v1, v2, v3);

            if (c.isNan())
                c = Circumcenter(v2, v3, v1);

            if (c.isNan())
                c = Circumcenter(v3, v1, v2);

            return c;
        }

        private Vertex Circumcenter(Vertex a, Vertex b, Vertex c)
        {
            // determine midpoints (average of x & y coordinates)
            Vertex midAB = Utility.Midpoint(a, b);
            Vertex midBC = Utility.Midpoint(b, c);

            // determine slope
            // we need the negative reciprocal of the slope to get the slope of the perpendicular bisector
            float slopeAB = -1 / Utility.Slope(a, b);
            float slopeBC = -1 / Utility.Slope(b, c);

            // y = mx + b
            // solve for b
            float bAB = midAB.y - slopeAB * midAB.x;
            float bBC = midBC.y - slopeBC * midBC.x;

            // solve for x & y
            // x = (b1 - b2) / (m2 - m1)
            float x = (bAB - bBC) / (slopeBC - slopeAB);

            Vertex circumcenter = new Vertex(
                x,
                (slopeAB * x) + bAB
            );

            return circumcenter;
        }

        public float Diameter()
        {
            Vertex c = Circumcenter();
            Vertex v1 = HalfEdge.Origin;

            return Convert.ToSingle(Math.Sqrt((c.x - v1.x) * (c.x - v1.x) + (c.y - v1.y) * (c.y - v1.y)));
        }
    }
}

