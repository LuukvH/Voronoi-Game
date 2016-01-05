using System;

namespace Objects
{
    public class Vertex
    {
        public float X;
        public float Y;

        public Vertex()
        {
            X = float.NaN;
            Y = float.NaN;
        }

        public Vertex(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Vertex(Vertex v)
        {
            X = v.X;
            Y = v.Y;
        }

        public bool IsNan()
        {
            return float.IsNaN(X) || float.IsNaN(Y);
        }


        public bool Equals(Vertex v)
        {
            return this.X == v.X && this.Y == v.Y;
        }

        public float Distance(Vertex vertex) => Convert.ToSingle(Math.Sqrt(DistanceSquared(vertex)));

        public float ManhattanDistance(Vertex vertex) => Convert.ToSingle(Math.Abs(X - vertex.X) + Math.Abs(Y - vertex.Y));

        public float DistanceSquared(Vertex vertex)
        {
            float dx = X - vertex.X;
            float dy = Y - vertex.Y;
            return dx*dx + dy*dy;
        }

        #region Operators

        public static Vertex operator -(Vertex v, Vertex w)
        {
            return new Vertex(v.X - w.X, v.Y - w.Y);
        }

        public static Vertex operator +(Vertex v, Vertex w)
        {
            return new Vertex(v.X + w.X, v.Y + w.Y);
        }

        public static float operator *(Vertex v, Vertex w)
        {
            return v.X*w.X + v.Y*w.Y;
        }

        public static Vertex operator *(Vertex v, float mult)
        {
            return new Vertex(v.X*mult, v.Y*mult);
        }

        public static Vertex operator *(float mult, Vertex v)
        {
            return new Vertex(v.X*mult, v.Y*mult);
        }

        public float Cross(Vertex v)
        {
            return X*v.Y - Y*v.X;
        }

        #endregion
    }
}