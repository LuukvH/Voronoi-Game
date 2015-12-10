namespace Voronoi
{
    public class HalfEdge
    {
        private Vertex _origin;

        public Face Face;
        public HalfEdge Next;
        public HalfEdge Prev;
        public HalfEdge Twin;

        public HalfEdge(Vertex v)
        {
            _origin = new Vertex(v.X, v.Y);

            Twin = null;
            Next = null;
            Prev = null;
        }

        public Vertex Origin
        {
            get { return _origin; }
            set { _origin = new Vertex(value.X, value.Y); }
        }
    }
}