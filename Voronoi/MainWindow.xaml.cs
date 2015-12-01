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
        Graph graph = new Graph();

        bool drawcircles = true;
        bool drawfaces = true;
        bool drawlines = true;

        float marginleft = 100;
        float margintop = 300;

        public MainWindow()
        {
            InitializeComponent();
            InitBrushes();

            graph.Create();
            selected = graph.HalfEdges[0];

            Redraw();
        }

        private List<Brush> _brushes;
        private void InitBrushes()
        {
            _brushes = new List<Brush>();
            var props = typeof(Brushes).GetProperties(BindingFlags.Public | BindingFlags.Static);
            foreach (var propInfo in props)
            {
                _brushes.Add((Brush)propInfo.GetValue(null, null));
            }
        }

        private Random _rand = new Random();
        private Brush GetRandomBrush()
        {
            return _brushes[_rand.Next(_brushes.Count)];
        }

        public void DrawFaces()
        {
            foreach (Face face in graph.Faces)
            {
                //if (face != selected.Face)
                //  continue;

                Polygon p = new Polygon();
                p.Stroke = Brushes.Black;

                if (face.Brush == null)
                    face.Brush = GetRandomBrush();

                p.Fill = face.Brush;

                p.StrokeThickness = 1;
                p.HorizontalAlignment = HorizontalAlignment.Left;
                p.VerticalAlignment = VerticalAlignment.Center;

                Point p1 = new Point(marginleft + face.HalfEdge.Origin.x, margintop - face.HalfEdge.Origin.y);
                Point p2 = new Point(marginleft + face.HalfEdge.Next.Origin.x, margintop - face.HalfEdge.Next.Origin.y);
                Point p3 = new Point(marginleft + face.HalfEdge.Next.Next.Origin.x, margintop - face.HalfEdge.Next.Next.Origin.y);

                p.Points = new PointCollection() { p1, p2, p3 };

                canvas.Children.Add(p);
            }
        }

        public void DrawVertices()
        {
            foreach (Vertex vertex in graph.Vertices)
            {
                // Create a StackPanel to contain the shape.
                StackPanel myStackPanel = new StackPanel();

                // Create a red Ellipse.
                Ellipse myEllipse = new Ellipse();

                // Create a SolidColorBrush with a red color to fill the 
                // Ellipse with.
                SolidColorBrush mySolidColorBrush = new SolidColorBrush();

                // Describes the brush's color using RGB values. 
                // Each value has a range of 0-255.
                mySolidColorBrush.Color = Colors.Blue;
                myEllipse.Fill = mySolidColorBrush;

                // Set the width and height of the Ellipse.
                myEllipse.Width = 10;
                myEllipse.Height = 10;

                Canvas.SetLeft(myEllipse, vertex.x - 5 + marginleft);
                Canvas.SetTop(myEllipse, 300 - vertex.y - 5);
                Canvas.SetZIndex(myEllipse, 3);

                // Add the Ellipse to the StackPanel.
                canvas.Children.Add(myEllipse);
            }
        }


        public void Redraw()
        {

            canvas.Children.Clear();

            if (drawfaces)
                DrawFaces();

            if (drawlines)
                DrawLines();

            if (drawcircles)
                DrawCircumCenters();

            DrawVertices();

            DrawVoronoi();
        }

        public void DrawCircumCenters()
        {
            foreach (Face face in graph.Faces)
            {
                if (face is Triangle)
                {
                    // Create a StackPanel to contain the shape.
                    StackPanel myStackPanel = new StackPanel();

                    // Create a red Ellipse.
                    Ellipse myEllipse = new Ellipse();
                    Ellipse centre = new Ellipse();

                    // Create a SolidColorBrush with a red color to fill the 
                    // Ellipse with.
                    SolidColorBrush mySolidColorBrush = new SolidColorBrush();

                    // Describes the brush's color using RGB values. 
                    // Each value has a range of 0-255.
                    mySolidColorBrush.Color = Colors.LightGreen;
                    myEllipse.Stroke = mySolidColorBrush;
                    centre.Fill = mySolidColorBrush;

                    Vertex c = (face as Triangle).Circumcenter();
                    float d = (face as Triangle).Diameter();

                    // Set the width and height of the Ellipse.
                    myEllipse.Width = d * 2;
                    myEllipse.Height = d * 2;

                    centre.Width = 10;
                    centre.Height = 10;

                    Canvas.SetLeft(myEllipse, c.x - d + marginleft);
                    Canvas.SetTop(myEllipse, 300 - c.y - d);
                    Canvas.SetZIndex(myEllipse, 3);

                    Canvas.SetLeft(centre, c.x - 5 + marginleft);
                    Canvas.SetTop(centre, 300 - c.y - 5);
                    Canvas.SetZIndex(centre, 3);

                    // Add the Ellipse to the StackPanel.
                    canvas.Children.Add(myEllipse);
                    canvas.Children.Add(centre);
                }
            }
        }

        public void DrawLines()
        {
            foreach (HalfEdge halfEdge in graph.HalfEdges)
            {
                Line line = new Line();
                line.Visibility = System.Windows.Visibility.Visible;
                line.StrokeThickness = 2;
                line.Stroke = System.Windows.Media.Brushes.Black;
                line.X1 = halfEdge.Origin.x + marginleft;
                line.X2 = halfEdge.Next.Origin.x + marginleft;
                line.Y1 = 300 - halfEdge.Origin.y;
                line.Y2 = 300 - halfEdge.Next.Origin.y;

                if (selected == halfEdge)
                {
                    line.StrokeThickness = 4;
                    line.Stroke = System.Windows.Media.Brushes.Yellow;
                    Canvas.SetZIndex(line, 2);
                }

                //halfEdge.Line = line;
                canvas.Children.Add(line);
            }
        }

        // Testing only
        public void DrawVoronoi()
        {
            foreach (HalfEdge halfEdge in graph.HalfEdges)
            {

                Face f1 = halfEdge.Face;
                Face f2 = halfEdge.Twin.Face;

                if (f1 is Triangle && f2 is Triangle)
                {
                    Triangle t1 = f1 as Triangle;
                    Triangle t2 = f2 as Triangle;

                    Vertex v1 = t1.Circumcenter();
                    Vertex v2 = t2.Circumcenter();

                    Line line = new Line();
                    line.Visibility = System.Windows.Visibility.Visible;
                    line.StrokeThickness = 2;
                    line.Stroke = System.Windows.Media.Brushes.LightGreen;
                    line.X1 = v1.x + marginleft;
                    line.X2 = v2.x + marginleft;
                    line.Y1 = 300 - v1.y;
                    line.Y2 = 300 - v2.y;

                    //halfEdge.Line = line;
                    canvas.Children.Add(line);
                }
            }
        }

        Brush brush = Brushes.Red;
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            HalfEdge he = null;

            if (e.Key == Key.C)
            {
                drawcircles = !drawcircles;
                Redraw();
            }

            if (e.Key == Key.L)
            {
                drawlines = !drawlines;
                Redraw();
            }

            if (e.Key == Key.G)
            {
                drawfaces = !drawfaces;
                Redraw();
            }

            if (e.Key == Key.R)
            {
                Redraw();
            }

            if (e.Key == Key.N)
                he = selected.Next;

            if (e.Key == Key.P)
                he = selected.Prev;

            if (e.Key == Key.T)
            {
                he = selected.Twin;
            }

            if (e.Key == Key.F)
            {
                graph.Flip(selected);
            }

            if (he != null)
            {
                selected = he;
            }

            Redraw();
        }

        int t = 0;
        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Vertex vertex = new Vertex(Convert.ToInt32(e.GetPosition(canvas).X) - marginleft, 300 - Convert.ToInt32(e.GetPosition(canvas).Y));

            canvas.Children.Clear();

            //selected = graph.FindFace(vertex).HalfEdge;
            graph.AddVertex(vertex);

            Redraw();
        }
    }
}
