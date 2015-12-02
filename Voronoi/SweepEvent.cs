using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voronoi
{
    public class SweepEvent
    {
        public SweepEvent(Vertex vertex, SweepEventTypes type)
        {
            this.Vertex = vertex;
            this.SweepEventType = type;
        }

        public Vertex Vertex;

        public SweepEventTypes SweepEventType;

        public Object obj;
    }
}
