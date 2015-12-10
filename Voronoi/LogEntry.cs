using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voronoi
{
    public class LogEntry
    {
        public LogEntry(String text, Graph graph)
        {
            this.Message = text;

            State = new Graph();
            State.Vertices.AddRange(graph.Vertices);
            State.Faces.AddRange(graph.Faces);
        }

        public Graph State;
        public String Message { get; private set; }
        public List<Object> objects = new List<Object>();
    }
}
