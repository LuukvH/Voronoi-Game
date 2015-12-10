namespace Voronoi
{
    public class Triangle : Face
    {
        private bool _calculatedCircumcenter;
        private bool _calculatedCircumcenterRangeSquared;

        private Vertex _circumcenter;

        private float _circumcenterRangeSquared;

        public Triangle(HalfEdge halfEdge) : base(halfEdge)
        {
            Vertex v1 = HalfEdge.Origin;
            Vertex v2 = HalfEdge.Next.Origin;
            Vertex v3 = HalfEdge.Next.Next.Origin;
            Vertex v4 = HalfEdge.Next.Next.Next.Origin;

            if (v1 == v2 || v2 == v3 || v1 == v3)
                throw new IncorrectTriangleException("Triangle does not has a correct 3 vertex loop.");

            if (v1 != v4)
                throw new IncorrectTriangleException("Triangle does not has a correct 3 vertex loop.");

            // Fix halfedges to this face
            halfEdge.Face = this;
            halfEdge.Next.Face = this;
            halfEdge.Next.Next.Face = this;

            // Add vertices to the array
            Vertices.Add(v1);
            Vertices.Add(v2);
            Vertices.Add(v3);
        }

        public Vertex Circumcenter
        {
            get
            {
                if (_calculatedCircumcenter)
                    return _circumcenter;

                _circumcenter = CalculateCircumcenter();
                _calculatedCircumcenter = true;

                return _circumcenter;
            }
        }

        public float CircumcenterRangeSquared
        {
            get
            {
                if (_calculatedCircumcenterRangeSquared)
                    return _circumcenterRangeSquared;

                _circumcenterRangeSquared = Vertices[0].DeltaSquaredXy(Circumcenter);
                _calculatedCircumcenterRangeSquared = true;

                return _circumcenterRangeSquared;
            }
        }

        private Vertex CalculateCircumcenter()
        {
            Vertex v1 = Vertices[0];
            Vertex v2 = Vertices[1];
            Vertex v3 = Vertices[2];

            // Todo: Here we should add some x1-x2 > 0 logic to choose one of these, instead of trying
            Vertex c = CalculateCircumcenterT(v1, v2, v3);

            if (c.IsNan())
                c = CalculateCircumcenterT(v2, v3, v1);

            if (c.IsNan())
                c = CalculateCircumcenterT(v3, v1, v2);

            return c;
        }

        private Vertex CalculateCircumcenterT(Vertex a, Vertex b, Vertex c)
        {
            // determine midpoints (average of x & y coordinates)
            Vertex midAb = Utility.Midpoint(a, b);
            Vertex midBc = Utility.Midpoint(b, c);

            // determine slope
            // we need the negative reciprocal of the slope to get the slope of the perpendicular bisector
            float slopeAb = -1/Utility.Slope(a, b);
            float slopeBc = -1/Utility.Slope(b, c);

            // y = mx + b
            // solve for b
            float bAb = midAb.Y - slopeAb*midAb.X;
            float bBc = midBc.Y - slopeBc*midBc.X;

            // solve for x & y
            // x = (b1 - b2) / (m2 - m1)
            float x = (bAb - bBc)/(slopeBc - slopeAb);

            Vertex circumcenter = new Vertex(
                x,
                slopeAb*x + bAb
                );

            return circumcenter;
        }

        public bool InsideCircumcenter(Vertex vertex)
        {
            return Circumcenter.DeltaSquaredXy(vertex) < CircumcenterRangeSquared;
        }
    }
}