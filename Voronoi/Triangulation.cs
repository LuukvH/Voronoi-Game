﻿using System.Collections.Generic;
using Objects;

namespace Voronoi
{
    public class Triangulation : Graph
    {
        public override bool AddVertex(Vertex vertex)
        {
            Face face = FindFace(vertex);

            if (face == null)
                return false;
            
            AddVertex(face, vertex);

            return true;
        }

        protected void AddVertex(Face face, Vertex vertex)
        {
            base.AddVertex(vertex);
            Faces.Remove(face);

            HalfEdge h1 = face.HalfEdge;
            HalfEdge h2 = h1.Next;
            HalfEdge h3 = h2.Next;

            HalfEdge h4 = new HalfEdge(h1.Origin);
            HalfEdge h5 = new HalfEdge(h2.Origin);
            HalfEdge h6 = new HalfEdge(h3.Origin);
            HalfEdge h7 = new HalfEdge(vertex);
            HalfEdge h8 = new HalfEdge(vertex);
            HalfEdge h9 = new HalfEdge(vertex);
            HalfEdges.AddRange(new List<HalfEdge> {h4, h5, h6, h7, h8, h9});

            h4.Twin = h7;
            h7.Twin = h4;
            h5.Twin = h8;
            h8.Twin = h5;
            h6.Twin = h9;
            h9.Twin = h6;

            // Set all next
            h1.Next = h5;
            h5.Prev = h1;
            h5.Next = h7;
            h7.Prev = h5;
            h7.Next = h1;
            h1.Prev = h7;

            h2.Next = h6;
            h6.Prev = h2;
            h6.Next = h8;
            h8.Prev = h6;
            h8.Next = h2;
            h2.Prev = h8;

            h3.Next = h4;
            h4.Prev = h3;
            h4.Next = h9;
            h9.Prev = h4;
            h9.Next = h3;
            h3.Prev = h9;

            Triangle t1 = new Triangle(h1);
            Triangle t2 = new Triangle(h2);
            Triangle t3 = new Triangle(h3);

            Faces.Add(t1);
            Faces.Add(t2);
            Faces.Add(t3);

            Tree.Add(vertex, t1, t2, t3);
            
            LogEntry logEntry = new LogEntry("Adding edges.", this);
            logEntry.Objects.Add(vertex);
            Log.Add(logEntry);
        }
    }
}