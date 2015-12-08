using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voronoi
{
    class Graph
    {
        private List<Face> faces = new List<Face>();
        private List<Vertex> vertices = new List<Vertex>();
        private List<HalfEdge> halfEdges = new List<HalfEdge>();
        private ObservableCollection<LogEntry> log = new ObservableCollection<LogEntry>();

        public Graph()
        {

        }

        public List<Face> Faces { get { return faces; } }
        public List<Vertex> Vertices { get { return vertices; } }
        public List<HalfEdge> HalfEdges { get { return halfEdges; } }
        public ObservableCollection<LogEntry> Log { get { return log; } }

        public void Create()
        {
            Vertex v1 = new Vertex(20, 0);
            Vertex v2 = new Vertex(200, 180);
            Vertex v3 = new Vertex(20, 180);
            Vertex v4 = new Vertex(200, 0);
            vertices.AddRange(new List<Vertex>() { v1, v2, v3, v4 });

            HalfEdge h1 = new HalfEdge(v1);
            HalfEdge h2 = new HalfEdge(v2);
            HalfEdge h3 = new HalfEdge(v3);
            halfEdges.AddRange(new List<HalfEdge>() { h1, h2, h3 });

            Face face = new Triangle(h1);
            h1.Face = face;
            h2.Face = face;
            h3.Face = face;
            faces.Add(face);

            h1.Next = h2;
            h2.Next = h3;
            h3.Next = h1;

            h2.Prev = h1;
            h3.Prev = h2;
            h1.Prev = h3;

            HalfEdge h4 = new HalfEdge(v2);
            HalfEdge h5 = new HalfEdge(v1);
            HalfEdge h6 = new HalfEdge(v4);
            halfEdges.AddRange(new List<HalfEdge>() { h4, h5, h6 });

            Face face1 = new Triangle(h4);
            h4.Face = face1;
            h5.Face = face1;
            h6.Face = face1;
            faces.Add(face1);

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
            halfEdges.AddRange(new List<HalfEdge>() { h7, h8, h9, h10 });

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
        }

        // Todo: tree implementation
        public Face FindFace(Vertex vertex)
        {
            foreach (Face face in faces)
            {
                if (face.inside(vertex))
                {
                    return face;
                }
            }

            return null;
        }

        public bool AddVertex(Vertex vertex)
        {
            Face face = FindFace(vertex);

            if (face == null)
                return false;

            List<Face> faces = AddVertex(face, vertex);
            this.faces.AddRange(faces);
            Delaunay(faces);

            return true;
        }

        private List<Face> AddVertex(Face face, Vertex vertex)
        {
            this.faces.Remove(face);

            LogEntry logEntry = new LogEntry("Adding vertex.");
            logEntry.objects.Add(vertex);
            log.Add(logEntry);

            vertices.Add(vertex);
            HalfEdge h1 = face.HalfEdge;
            HalfEdge h2 = h1.Next;
            HalfEdge h3 = h2.Next;

            HalfEdge h4 = new HalfEdge(h1.Origin);
            HalfEdge h5 = new HalfEdge(h2.Origin);
            HalfEdge h6 = new HalfEdge(h3.Origin);
            HalfEdge h7 = new HalfEdge(vertex);
            HalfEdge h8 = new HalfEdge(vertex);
            HalfEdge h9 = new HalfEdge(vertex);
            halfEdges.AddRange(new List<HalfEdge>() { h4, h5, h6, h7, h8, h9 });

            h4.Twin = h7;
            h7.Twin = h4;
            h5.Twin = h8;
            h8.Twin = h5;
            h6.Twin = h9;
            h9.Twin = h6;

            Face face1 = new Triangle(h1);
            h1.Face = face1;
            h5.Face = face1;
            h7.Face = face1;

            Face face2 = new Triangle(h2);
            h2.Face = face2;
            h6.Face = face2;
            h8.Face = face2;

            Face face3 = new Triangle(h3);
            h3.Face = face3;
            h4.Face = face3;
            h9.Face = face3;

            List<Face> faces = new List<Face>() { face1, face2, face3 };

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

            return faces;
        }

        private void Delaunay(List<Face> faces)
        {
            LogEntry logEntry;
            int limit = 20;
            for (int i = 0; i < faces.Count; i++)
            {
                if (!(faces[i] is Triangle))
                    continue;

                if (i > limit)
                {
                    logEntry = new LogEntry("length: " + faces.Count() + ", index: " + i + ", Limit reached.");
                    log.Add(logEntry);
                    break;
                }

                Triangle face = faces[i] as Triangle;

                // Get all surrounding points
                Vertex v = face.Circumcenter();
                float d = face.Diameter();

                // Which points to test
                List<Vertex> points = new List<Vertex>();
                points.Add(face.HalfEdge.Twin.Prev.Origin);
                points.Add(face.HalfEdge.Next.Twin.Next.Twin.Prev.Origin);
                points.Add(face.HalfEdge.Next.Next.Twin.Prev.Origin);

                points.Add(face.HalfEdge.Next.Twin.Prev.Origin);
                points.Add(face.HalfEdge.Next.Twin.Next.Twin.Prev.Origin);
                points.Add(face.HalfEdge.Next.Twin.Next.Next.Twin.Prev.Origin);

                // Logging for testing purposes
                logEntry = new LogEntry("length: " + faces.Count() + ", index: " + i + ", Testing points in circumcenter of a face.");
                logEntry.objects.Add(face);
                logEntry.objects.AddRange(points);
                log.Add(logEntry);

                // Test if flip is needed
                bool flip = false;
                foreach (Vertex p in points)
                {
                    if (Utility.Distance(p, v) < d)
                    {
                        flip = true;
                        break;
                    }
                }

                if (flip)
                {
                    Flip(face.HalfEdge);
                    logEntry = new LogEntry("Adding face for delaunay test.");

                    // Add surrounding faces
                    if (face.HalfEdge.Next.Twin.Face != null && !faces.Contains(face.HalfEdge.Next.Twin.Face))
                    {
                        faces.Add(face.HalfEdge.Next.Twin.Face);
                        logEntry.objects.Add(face.HalfEdge.Next.Twin.Face);
                    }

                    if (face.HalfEdge.Next.Next.Twin.Face != null && !faces.Contains(face.HalfEdge.Next.Next.Twin.Face))
                    {
                        faces.Add(face.HalfEdge.Next.Next.Twin.Face);
                        logEntry.objects.Add(face.HalfEdge.Next.Next.Twin.Face);
                    }

                    if (face.HalfEdge.Twin.Next.Twin.Face != null && !faces.Contains(face.HalfEdge.Next.Twin.Next.Face))
                    {
                        faces.Add(face.HalfEdge.Twin.Next.Twin.Face);
                        logEntry.objects.Add(face.HalfEdge.Twin.Next.Twin.Face);
                    }

                    if (face.HalfEdge.Twin.Next.Next.Twin.Face != null && !faces.Contains(face.HalfEdge.Twin.Next.Next.Twin.Face))
                    {
                        faces.Add(face.HalfEdge.Twin.Next.Next.Twin.Face);
                        logEntry.objects.Add(face.HalfEdge.Twin.Next.Next.Twin.Face);
                    }

                    log.Add(logEntry);
                }
            }
        }

        public void Flip(HalfEdge h)
        {
            LogEntry logEntry = new LogEntry("Flipping edge.");
            logEntry.objects.Add(h);
            log.Add(logEntry);

            HalfEdge h1 = h;
            HalfEdge h2 = h1.Next;
            HalfEdge h3 = h2.Next;
            HalfEdge h4 = h.Twin;
            HalfEdge h5 = h4.Next;
            HalfEdge h6 = h5.Next;

            if (h1.Face == null || h4.Face == null)
                return;

            // Set faces defined as h1 and h4
            h1.Face.HalfEdge = h1;
            h4.Face.HalfEdge = h4;

            h1.Next = h6;
            h6.Prev = h1;
            h6.Next = h2;
            h2.Prev = h6;
            h2.Next = h1;
            h1.Prev = h2;
            h1.Origin = h3.Origin;
            h6.Face = h1.Face;
            h2.Face = h1.Face;

            h4.Next = h3;
            h3.Prev = h4;
            h3.Next = h5;
            h5.Prev = h3;
            h5.Next = h4;
            h4.Prev = h5;
            h4.Origin = h6.Origin;
            h3.Face = h4.Face;
            h5.Face = h4.Face;
        }
    }
}
