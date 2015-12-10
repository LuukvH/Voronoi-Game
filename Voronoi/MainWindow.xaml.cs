using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Voronoi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HalfEdge selected;
        Graph graph = new Delaunay();

        bool drawcircles = false;
        bool drawfaces = false;
        bool drawedges = false;
        bool drawvoronoi = true;
        
        public MainWindow()
        {
            InitializeComponent();
            InitBrushes();

            graph.Create();

            DrawGraph(graph);

            // Add items to log
            dataGrid.ItemsSource = graph.Log;
        }

        private List<Color> _colors;
        private void InitBrushes()
        {
            _colors = new List<Color>();
            var props = typeof(Colors).GetProperties(BindingFlags.Public | BindingFlags.Static);
            foreach (var propInfo in props)
            {
                _colors.Add((Color)propInfo.GetValue(null, null));
            }
        }

        private Random _rand = new Random();
        private Color GetRandomColor()
        {
            _rand.Next();
            return _colors[_rand.Next(_colors.Count)];
        }

        

        // Testing only
        public void DrawVoronoi()
        {
            List<Edge> edges = new List<Edge>();

            foreach (HalfEdge halfEdge in graph.HalfEdges)
            {
                if (halfEdge.Twin == null)
                    continue;

                Face f1 = halfEdge.Face;
                Face f2 = halfEdge.Twin.Face;

                if (f1 is Triangle && f2 is Triangle)
                {
                    Triangle t1 = f1 as Triangle;
                    Triangle t2 = f2 as Triangle;

                    Vertex v1 = t1.Circumcenter;
                    Vertex v2 = t2.Circumcenter;

                    Edge edge = new Edge(v1, v2);
                    edges.Add(edge);
                }
            }

            foreach (Edge edge in edges)
            {
                //Draw lines
                Line line = new Line();
                line.Visibility = System.Windows.Visibility.Visible;
                line.StrokeThickness = 2;
                line.Stroke = System.Windows.Media.Brushes.LightGreen;
                try
                {
                    line.X1 = edge.v1.X;
                    line.X2 = edge.v2.X;
                    line.Y1 = edge.v1.Y;
                    line.Y2 = edge.v2.Y;

                    //halfEdge.Line = line;
                    canvas.Children.Add(line);
                }
                catch { }
            }
        }

        Brush brush = Brushes.Red;
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            HalfEdge he = null;

            if (e.Key == Key.V)
            {
                drawvoronoi = !drawvoronoi;
                DrawGraph(graph);
            }

            if (e.Key == Key.C)
            {
                drawcircles = !drawcircles;
                DrawGraph(graph);
            }

            if (e.Key == Key.L)
            {
                drawedges = !drawedges;
                DrawGraph(graph);
            }

            if (e.Key == Key.F)
            {
                drawfaces = !drawfaces;
                DrawGraph(graph);
            }

            if (e.Key == Key.R)
            {
                DrawGraph(graph);
            }

            if (e.Key == Key.G)
            {
                Random r = new Random();

                for (int i = 0; i < 500; i++)
                {
                    int x = r.Next(Convert.ToInt32(canvas.ActualWidth));
                    int y = r.Next(Convert.ToInt32(canvas.ActualHeight));
                    graph.AddVertex(new Vertex(x, y));
                }
                DrawGraph(graph);
            }

                if (e.Key == Key.N)
            {
                if (selected == null)
                {
                    selected = graph.HalfEdges.First();
                }
                else
                {
                    he = selected.Next;
                }
            }

            if (e.Key == Key.P)
                he = selected.Prev;

            if (e.Key == Key.T)
            {
                he = selected.Twin;
            }

            if (he != null)
            {
                selected = he;
            }

            DrawGraph(graph);
        }

        int t = 0;
        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Vertex vertex = new Vertex(Convert.ToInt32(e.GetPosition(canvas).X), Convert.ToInt32(e.GetPosition(canvas).Y));

            canvas.Children.Clear();

            //selected = graph.FindFace(vertex).HalfEdge;
            graph.AddVertex(vertex);

            DrawGraph(graph);
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // On event selection

            selected = null;
            LogEntry entry = e.AddedItems[0] as LogEntry;

            canvas.Children.Clear();
            DrawGraph(entry.State);

            foreach (Object obj in entry.objects)
            {
                if (obj is Face)
                {
                    Face face = obj as Face;
                    DrawFace(face, Colors.LightYellow);
                }

                if (obj is Triangle)
                {
                    DrawCircumcenter(obj as Triangle);
                }

                if (obj is HalfEdge)
                {
                    DrawEdge(obj as HalfEdge, Colors.Yellow, 5);
                    selected = obj as HalfEdge;
                }

                if (obj is Edge)
                {
                    DrawEdge(obj as Edge, Colors.Yellow, 5);
                }

                if (obj is Vertex)
                {
                    DrawVertex(obj as Vertex, Colors.Red);
                }
            }
        }

        #region DrawFunctions

        private void DrawGraph(Graph graph)
        {
            canvas.Children.Clear();

            if (drawfaces)
            {
                foreach (Face face in graph.Faces)
                {
                    if (face.Color == Colors.Transparent)
                        face.Color = GetRandomColor();

                    DrawFace(face, face.Color);
                }
            }

            if (drawedges)
            {
                foreach (HalfEdge halfEdge in graph.HalfEdges)
                {
                    DrawEdge(halfEdge, Colors.Black, 2);
                }
            }

            // Draw selected
            if (selected != null)
            {
                DrawFace(selected.Face, Colors.LightYellow);
                DrawEdge(selected, Colors.YellowGreen, 3);
            }

            if (drawcircles)
            {
                foreach (Face face in graph.Faces)
                {
                    if (face is Triangle)
                    {
                        Triangle triangle = face as Triangle;
                        DrawCircumcenter(triangle);
                    }
                }
            }

            if (drawvoronoi)
            {
                DrawVoronoi();
            }

            foreach (Vertex vertex in graph.Vertices)
            {
                DrawVertex(vertex, Colors.Blue);
            }
        }

        public void DrawVertex(Vertex v, Color color)
        {
            // Create a red Ellipse.
            Ellipse myEllipse = new Ellipse();

            // Create a SolidColorBrush with a red color to fill the 
            // Ellipse with.
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();

            // Describes the brush's color using RGB values. 
            // Each value has a range of 0-255.
            mySolidColorBrush.Color = color;
            myEllipse.Fill = mySolidColorBrush;

            // Set the width and height of the Ellipse.
            myEllipse.Width = 10;
            myEllipse.Height = 10;

            Canvas.SetLeft(myEllipse, v.X - 5);
            Canvas.SetTop(myEllipse, v.Y - 5);
            Canvas.SetZIndex(myEllipse, 3);

            // Add the Ellipse to the StackPanel.
            canvas.Children.Add(myEllipse);
        }

        private void DrawEdge(HalfEdge halfEdge, Color color, double thickness)
        {
            DrawEdge(halfEdge.Origin, halfEdge.Next.Origin, color, thickness);
        }

        private void DrawEdge(Edge edge, Color color, double thickness)
        {
            DrawEdge(edge.v1, edge.v2, color, thickness);
        }

        private void DrawEdge(Vertex v1, Vertex v2, Color color, double thickness)
        {
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            mySolidColorBrush.Color = color;

            Line line = new Line();
            line.Visibility = System.Windows.Visibility.Visible;
            line.StrokeThickness = thickness;
            line.Stroke = mySolidColorBrush;
            line.X1 = v1.X;
            line.X2 = v2.X;
            line.Y1 = v1.Y;
            line.Y2 = v2.Y;

            //halfEdge.Line = line;
            canvas.Children.Add(line);
        }

        private void DrawFace(Face face, Color color)
        {
            if (face == null)
                return;

            SolidColorBrush solidColorBrush = new SolidColorBrush();
            solidColorBrush.Color = color;

            Polygon p = new Polygon();
            p.Stroke = Brushes.Black;

            p.Fill = solidColorBrush;

            p.StrokeThickness = 1;
            p.HorizontalAlignment = HorizontalAlignment.Left;
            p.VerticalAlignment = VerticalAlignment.Center;

            p.Points = new PointCollection();
            foreach (Vertex vertex in face.Vertices)
            {
                p.Points.Add(new Point(vertex.X, vertex.Y));
            }

            canvas.Children.Add(p);
        }

        public void DrawCircumcenter(Triangle triangle)
        {
            // Create a red Ellipse.
            Ellipse myEllipse = new Ellipse();
            Ellipse centre = new Ellipse();

            // Create a SolidColorBrush with a red color to fill the 
            // Ellipse with.
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();

            // Describes the brush's color using RGB values. 
            // Each value has a range of 0-255.
            mySolidColorBrush.Color = Colors.LightPink;
            myEllipse.Stroke = mySolidColorBrush;
            centre.Fill = mySolidColorBrush;

            Vertex c = triangle.Circumcenter;
            float d = Convert.ToSingle(Math.Sqrt(triangle.CircumcenterRangeSquared));

            // Set the width and height of the Ellipse.
            myEllipse.Width = d * 2;
            myEllipse.Height = d * 2;

            centre.Width = 10;
            centre.Height = 10;

            Canvas.SetLeft(myEllipse, c.X - d);
            Canvas.SetTop(myEllipse, c.Y - d);
            Canvas.SetZIndex(myEllipse, 3);

            Canvas.SetLeft(centre, c.X - 5);
            Canvas.SetTop(centre, c.Y - 5);
            Canvas.SetZIndex(centre, 3);

            // Add the Ellipse to the StackPanel.
            canvas.Children.Add(myEllipse);
            canvas.Children.Add(centre);
        }

        #endregion
    }
}
