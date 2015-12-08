using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voronoi
{
    public class Edge
    {
        public Edge(Vertex v1, Vertex v2)
        {
            if (v1.y >= v2.y)
            {
                this.v1 = v1;
                this.v2 = v2;
            }
            else
            {
                this.v1 = v2;
                this.v2 = v1;
            }
        }

        public Vertex v1;
        public Vertex v2;

        public bool IsEqual(Edge e)
        {
            if (!Extensions.NearlyEqual(this.v1.x, e.v1.x))
                return false;

            if (!Extensions.NearlyEqual(this.v1.y, e.v1.y))
                return false;

            if (!Extensions.NearlyEqual(this.v2.x, e.v2.x))
                return false;

            if (!Extensions.NearlyEqual(this.v2.y, e.v2.y))
                return false;

            return true;
        }

        public bool Intersect(Edge e, out Vertex crossing)
        {
            crossing = new Vertex();

            var r = this.v2 - this.v1;
            var s = e.v2 - e.v1;
            var rxs = r.Cross(s);
            var qpxr = (e.v1 - this.v1).Cross(r);

            /*
            // If lines are on top of each other
            if (Extensions.NearlyEqual(e.v1.x, this.v1.x) && Extensions.NearlyEqual(e.v2.x, this.v2.x) &&
                Extensions.NearlyEqual(e.v1.y, this.v1.y) && Extensions.NearlyEqual(e.v2.y, this.v2.y))
                return false;
            */

            /*
            // Ignore begin and endpoints
            if ((Extensions.NearlyEqual(e.v1.x, this.v1.x) && Extensions.NearlyEqual(e.v1.y, this.v1.y)) ||
               (Extensions.NearlyEqual(e.v2.x, this.v2.x) && Extensions.NearlyEqual(e.v2.y, this.v2.y)) ||
               (Extensions.NearlyEqual(e.v2.x, this.v1.x) && Extensions.NearlyEqual(e.v2.y, this.v1.y)) ||
               (Extensions.NearlyEqual(e.v1.x, this.v2.x) && Extensions.NearlyEqual(e.v1.y, this.v2.y)))
                return false;
            */

            // If r x s = 0 and (q - p) x r = 0, then the two lines are collinear.
            if (rxs.IsZero() && qpxr.IsZero())
                return false;

            // 3. If r x s = 0 and (q - p) x r != 0, then the two lines are parallel and non-intersecting.
            if (rxs.IsZero() && !qpxr.IsZero())
                return false;

            // t = (q - p) x s / (r x s)
            float t = (e.v1 - this.v1).Cross(s) / rxs;

            // u = (q - p) x r / (r x s)
            var u = (e.v1 - this.v1).Cross(r) / rxs;

            // 4. If r x s != 0 and 0 <= t <= 1 and 0 <= u <= 1
            // the two line segments meet at the point p + t r = q + u s.
            if (!rxs.IsZero() && (0 <= t && t <= 1) && (0 <= u && u <= 1))
            {
                // We can calculate the intersection point using either t or u.
                crossing = this.v1 + t * r;

                // An intersection was found.
                return true;
            }

            // 5. Otherwise, the two line segments are not parallel but do not intersect.
            return false;
        }

    }
}
