using System.Collections.Generic;

namespace Voronoi
{
    public class LogEntry
    {
        public List<object> Objects = new List<object>();

        public Graph State;

        public LogEntry(string text, Graph graph)
        {
            Message = text;

            State = new Graph();
            State.Vertices.AddRange(graph.Vertices);
            State.Faces.AddRange(graph.Faces);
        }

        public string Message { get; private set; }
    }
}