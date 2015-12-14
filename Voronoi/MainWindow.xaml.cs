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
        private HalfEdge _selected;
        private Graph _graph = new Delaunay();

        private bool _drawcircles = false;
        private bool _drawfaces = false;
        private bool _drawedges = false;
        private bool _drawvoronoi = true;
        
        public MainWindow()
        {
            InitializeComponent();
            InitBrushes();

            _graph.Create();

            DrawGraph(_graph);

            // Add items to log
            DataGrid.ItemsSource = _graph.Log;
        }

        private List<Color> _colors;
        private void InitBrushes()
        {
            _colors = new List<Color>();
            PropertyInfo[] props = typeof(Colors).GetProperties(BindingFlags.Public | BindingFlags.Static);
            foreach (PropertyInfo propInfo in props)
            {
                _colors.Add((Color)propInfo.GetValue(null, null));
            }
        }

        private readonly Random _rand = new Random();
        private Color GetRandomColor()
        {
            return _colors[_rand.Next(_colors.Count)];
        }

        // Testing only
        public void DrawVoronoi()
        {
            List<Edge> edges = (from halfEdge in _graph.HalfEdges where halfEdge.Twin != null let f1 = halfEdge.Face let f2 = halfEdge.Twin.Face where f1 is Triangle && f2 is Triangle let t1 = f1 as Triangle let t2 = f2 as Triangle let v1 = t1.Circumcenter let v2 = t2.Circumcenter select new Edge(v1, v2)).ToList();

            foreach (Edge edge in edges)
            {
                //Draw lines
                Line line = new Line();
                line.Visibility = System.Windows.Visibility.Visible;
                line.StrokeThickness = 2;
                line.Stroke = System.Windows.Media.Brushes.LightGreen;
                try
                {
                    line.X1 = edge.V1.X;
                    line.X2 = edge.V2.X;
                    line.Y1 = edge.V1.Y;
                    line.Y2 = edge.V2.Y;

                    //halfEdge.Line = line;
                    canvas.Children.Add(line);
                }
                catch { }
            }
        }

        private Brush _brush = Brushes.Red;
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            HalfEdge he = null;

            if (e.Key == Key.V)
            {
                _drawvoronoi = !_drawvoronoi;
                DrawGraph(_graph);
            }

            if (e.Key == Key.C)
            {
                _drawcircles = !_drawcircles;
                DrawGraph(_graph);
            }

            if (e.Key == Key.L)
            {
                _drawedges = !_drawedges;
                DrawGraph(_graph);
            }

            if (e.Key == Key.F)
            {
                _drawfaces = !_drawfaces;
                DrawGraph(_graph);
            }

            if (e.Key == Key.C)
            {
                _graph = new Delaunay();
                DrawGraph(_graph);
            }

            if (e.Key == Key.R)
            {
                DrawGraph(_graph);
            }

            if (e.Key == Key.G)
            {
                Random r = new Random();

                for (int i = 0; i < 500; i++)
                {
                    int x = r.Next(Convert.ToInt32(canvas.ActualWidth));
                    int y = r.Next(Convert.ToInt32(canvas.ActualHeight));
                    _graph.AddVertex(new Vertex(x, y));
                }
                DrawGraph(_graph);
            }

                if (e.Key == Key.N)
            {
                if (_selected == null)
                {
                    _selected = _graph.HalfEdges.First();
                }
                else
                {
                    he = _selected.Next;
                }
            }

            if (e.Key == Key.P)
                he = _selected.Prev;

            if (e.Key == Key.T)
            {
                he = _selected.Twin;
            }

            if (he != null)
            {
                _selected = he;
            }

            DrawGraph(_graph);
        }

        private int _t = 0;
        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Vertex vertex = new Vertex(Convert.ToInt32(e.GetPosition(canvas).X), Convert.ToInt32(e.GetPosition(canvas).Y));

            canvas.Children.Clear();

            //_selected = _graph.FindFace(vertex).HalfEdge;
            _graph.AddVertex(vertex);

            DrawGraph(_graph);
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // On event selection

            _selected = null;
            LogEntry entry = e.AddedItems[0] as LogEntry;

            canvas.Children.Clear();
            DrawGraph(entry.State);

            foreach (object obj in entry.Objects)
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
                    _selected = obj as HalfEdge;
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

            if (_drawfaces)
            {
                foreach (Face face in graph.Faces)
                {
                    if (face.Color == Colors.Transparent)
                        face.Color = GetRandomColor();

                    DrawFace(face, face.Color);
                }

                foreach (Face face in graph.Faces)
                {
                    if (face is Triangle)
                    {
                        DrawLabel((face as Triangle).Center, String.Format("V{0:d}", face.Id));
                    }
                }
            }

            if (_drawedges)
            {
                foreach (HalfEdge halfEdge in graph.HalfEdges)
                {
                    DrawEdge(halfEdge, Colors.Black, 2);
                }
            }

            // Draw _selected
            if (_selected != null)
            {
                DrawFace(_selected.Face, Colors.LightYellow);
                DrawEdge(_selected, Colors.YellowGreen, 3);
            }

            if (_drawcircles)
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

            if (_drawvoronoi)
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
            DrawEdge(edge.V1, edge.V2, color, thickness);
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

            Polygon p = new Polygon
            {
                Stroke = Brushes.Black,
                Fill = solidColorBrush,
                StrokeThickness = 1,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Points = new PointCollection()
            };

            foreach (Vertex vertex in face.Vertices)
            {
                p.Points.Add(new Point(vertex.X, vertex.Y));
            }

            canvas.Children.Add(p);
        }

        public void DrawLabel(Vertex vertex, String text)
        {
            // Create a red Ellipse.
            float radius = 1;
            SolidColorBrush solidBrush = new SolidColorBrush { Color = Colors.Black };
            Ellipse elipse = new Ellipse
            {
                Stroke = solidBrush,
                Fill = solidBrush,
                Width = radius * 2,
                Height = radius * 2
            };

            TextBlock textBlock = new TextBlock
            {
                Text = text,
                Foreground = new SolidColorBrush(solidBrush.Color)
            };

            Canvas.SetLeft(textBlock, vertex.X);
            Canvas.SetTop(textBlock, vertex.Y);
            canvas.Children.Add(textBlock);

            Canvas.SetLeft(elipse, vertex.X - radius);
            Canvas.SetTop(elipse, vertex.Y - radius);
            canvas.Children.Add(elipse);
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
