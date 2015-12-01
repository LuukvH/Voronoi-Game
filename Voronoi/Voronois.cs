using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voronoi
{
    public class Voronois
    {
        public Voronois(List<HalfEdge> edges)
        {
            foreach (HalfEdge halfEdge in edges)
            {
                Face f1 = halfEdge.Face;
                Face f2 = halfEdge.Twin.Face;

                if (f1 is Triangle && f2 is Triangle)
                {
                    Triangle t1 = f1 as Triangle;
                    Triangle t2 = f2 as Triangle;

                    Vertex v1 = t1.Circumcenter();
                    Vertex v2 = t2.Circumcenter();


                }
            }
        }
    }
}
