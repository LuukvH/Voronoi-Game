﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voronoi
{
    public class Delaunay : Triangulation
    {
        public override bool AddVertex(Vertex vertex)
        {
            Triangle face = FindFace(vertex) as Triangle;

            if (face == null)
                return false;

            AddVertex(face, vertex);

            // Find halfedges of triangle
            HalfEdge h1 = face.HalfEdge;
            HalfEdge h2 = face.HalfEdge.Next.Twin.Next;
            HalfEdge h3 = face.HalfEdge.Next.Twin.Next.Next.Twin.Next;

            // Log registration for playback
            LogEntry logEntry = new LogEntry("Select edges for delauney check", this);
            logEntry.objects.Add(vertex);
            logEntry.objects.Add(new Edge(h1.Origin, h1.Next.Origin));
            logEntry.objects.Add(new Edge(h2.Origin, h2.Next.Origin));
            logEntry.objects.Add(new Edge(h3.Origin, h3.Next.Origin));
            log.Add(logEntry);

            // Flip if needed
            LegalizeEdge(vertex, h1, h1.Face as Triangle);
            LegalizeEdge(vertex, h2, h2.Face as Triangle);
            LegalizeEdge(vertex, h3, h3.Face as Triangle);

            return true;
        }

        private void LegalizeEdge(Vertex vertex, HalfEdge halfEdge, Triangle triangle)
        {
            if (triangle == null)
                return;

            LogEntry logEntry = new LogEntry("Checking edge", this);
            //logEntry.objects.Add(vertex);
            logEntry.objects.Add(halfEdge.Face);
            logEntry.objects.Add(new Edge(halfEdge.Origin, halfEdge.Next.Origin));
            log.Add(logEntry);

            // Points to test
            Vertex v1 = halfEdge.Twin.Next.Next.Origin;
            Vertex v2 = halfEdge.Next.Twin.Next.Next.Origin;
            Vertex v3 = halfEdge.Next.Next.Twin.Next.Next.Origin;

            logEntry.objects.Add(v1);
            logEntry.objects.Add(v2);
            logEntry.objects.Add(v3);

            if (triangle.InsideCircumcenter(v1))
            {
                logEntry = new LogEntry("Found point inside circumcenter", this);
                logEntry.objects.Add(v1);
                logEntry.objects.Add(halfEdge.Face);
                log.Add(logEntry);
            }

            if (triangle.InsideCircumcenter(v2))
            {
                logEntry = new LogEntry("Found point inside circumcenter", this);
                logEntry.objects.Add(v2);
                logEntry.objects.Add(halfEdge.Face);
                log.Add(logEntry);
            }

            if (triangle.InsideCircumcenter(v3))
            {
                logEntry = new LogEntry("Found point inside circumcenter", this);
                logEntry.objects.Add(v3);
                logEntry.objects.Add(halfEdge.Face);
                log.Add(logEntry);
            }

            if (triangle.InsideCircumcenter(v1) || triangle.InsideCircumcenter(v2) || triangle.InsideCircumcenter(v3))
            {
                HalfEdge h1 = halfEdge.Twin.Next.Twin;
                HalfEdge h2 = halfEdge.Twin.Prev.Twin;

                Flip(halfEdge);

                logEntry = new LogEntry("Select recursive flip edges", this);
                logEntry.objects.Add(new Edge(h1.Origin, h1.Next.Origin));
                logEntry.objects.Add(new Edge(h2.Origin, h2.Next.Origin));
                log.Add(logEntry);

                LegalizeEdge(vertex, h1.Twin, h1.Twin.Face as Triangle);
                LegalizeEdge(vertex, h2.Twin, h2.Twin.Face as Triangle);
            }
        }
        
        private void Flip(HalfEdge h)
        {
            HalfEdge h1 = h;
            HalfEdge h2 = h1.Next;
            HalfEdge h3 = h2.Next;
            HalfEdge h4 = h.Twin;
            HalfEdge h5 = h4.Next;
            HalfEdge h6 = h5.Next;

            if (h1.Face == null || h4.Face == null)
                return;

            LogEntry logEntry = new LogEntry("Flipping edge", this);
            logEntry.objects.Add(new Edge(h.Origin, h.Next.Origin));
            log.Add(logEntry);

            // Remove old faces
            faces.Remove(h.Face);
            faces.Remove(h.Twin.Face);

            h1.Next = h6;
            h6.Prev = h1;
            h6.Next = h2;
            h2.Prev = h6;
            h2.Next = h1;
            h1.Prev = h2;
            h1.Origin = h3.Origin;

            h4.Next = h3;
            h3.Prev = h4;
            h3.Next = h5;
            h5.Prev = h3;
            h5.Next = h4;
            h4.Prev = h5;
            h4.Origin = h6.Origin;

            faces.Add(new Triangle(h1));
            faces.Add(new Triangle(h1.Twin));
        }
    }
}
