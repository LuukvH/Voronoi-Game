using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Objects;
using TreeStructure;

namespace Voronoi
{
    public class Graph
    {
        public List<Face> Faces { get; } = new List<Face>();
        public List<Vertex> Vertices { get; } = new List<Vertex>();
        public List<HalfEdge> HalfEdges { get; } = new List<HalfEdge>();
        public ObservableCollection<LogEntry> Log { get; } = new ObservableCollection<LogEntry>();

        public Tree Tree;

        public void Create()
        {
            Vertex v1 = new Vertex(-2000, -2000);
            Vertex v2 = new Vertex(2000, 2000);
            Vertex v3 = new Vertex(-2000, 2000);
            Vertex v4 = new Vertex(2000, -2000);
            Vertices.AddRange(new List<Vertex> {v1, v2, v3, v4});

            HalfEdge h1 = new HalfEdge(v1);
            HalfEdge h2 = new HalfEdge(v2);
            HalfEdge h3 = new HalfEdge(v3);
            HalfEdges.AddRange(new List<HalfEdge> {h1, h2, h3});

            h1.Next = h2;
            h2.Next = h3;
            h3.Next = h1;

            h2.Prev = h1;
            h3.Prev = h2;
            h1.Prev = h3;

            HalfEdge h4 = new HalfEdge(v2);
            HalfEdge h5 = new HalfEdge(v1);
            HalfEdge h6 = new HalfEdge(v4);
            HalfEdges.AddRange(new List<HalfEdge> {h4, h5, h6});

            h4.Twin = h1;
            h1.Twin = h4;

            h4.Next = h5;
            h5.Next = h6;
            h6.Next = h4;

            h5.Prev = h4;
            h6.Prev = h5;
            h4.Prev = h6;

            HalfEdge h7 = new HalfEdge(v1);
            HalfEdge h8 = new HalfEdge(v2);
            HalfEdge h9 = new HalfEdge(v3);
            HalfEdge h10 = new HalfEdge(v4);
            HalfEdges.AddRange(new List<HalfEdge> {h7, h8, h9, h10});

            h10.Next = h7;
            h7.Prev = h10;
            h8.Next = h10;
            h10.Prev = h8;
            h9.Next = h8;
            h8.Prev = h9;
            h7.Next = h9;
            h9.Prev = h7;

            h3.Twin = h7;
            h7.Twin = h3;
            h8.Twin = h6;
            h6.Twin = h8;
            h2.Twin = h9;
            h9.Twin = h2;

            h10.Twin = h5;
            h5.Twin = h10;

            Triangle t1 = new Triangle(h1);
            Faces.Add(t1);
            Tree = new Tree(t1);
            //Faces.Add(new Triangle(h4));
        }

        protected Face FindFace(Vertex vertex)
        {
            return Faces.FirstOrDefault(face => face.Inside(vertex));
            //return Tree.Search(vertex);
        }

        public virtual bool AddVertex(Vertex vertex)
        {
            Vertices.Add(vertex);

            LogEntry logEntry = new LogEntry("Adding vertex.", this);
            logEntry.Objects.Add(vertex);
            Log.Add(logEntry);

            return true;
        }
    }
}