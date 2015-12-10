using System.Collections.Generic;
using System.Windows.Media;

namespace Voronoi
{
    public class Face
    {
        public Color Color;

        public Face(HalfEdge halfEdge)
        {
            HalfEdge = halfEdge;
            Color = Colors.Transparent;
        }

        public List<Vertex> Vertices { get; } = new List<Vertex>();

        public HalfEdge HalfEdge { get; private set; }

        public bool Contains(Vertex vertex)
        {
            return Vertices.Contains(vertex);
        }

        public bool Inside(Vertex p)
        {
            int i, j = Vertices.Count - 1;
            bool oddNodes = false;

            for (i = 0; i < Vertices.Count; i++)
            {
                if (((Vertices[i].Y < p.Y && Vertices[j].Y >= p.Y) || (Vertices[j].Y < p.Y && Vertices[i].Y >= p.Y)) &&
                    (Vertices[i].X <= p.X || Vertices[j].X <= p.X))
                    oddNodes ^= Vertices[i].X +
                                (p.Y - Vertices[i].Y)/(Vertices[j].Y - Vertices[i].Y)*(Vertices[j].X - Vertices[i].X) <
                                p.X;
                j = i;
            }

            return oddNodes;
        }
    }
}