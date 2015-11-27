using System;
using System.Collections.Generic;
using System.Linq;
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
        float marginleft = 100;

        public MainWindow()
        {
            InitializeComponent();

            graph.Create();
            selected = graph.HalfEdges[0];

            Redraw();
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

            DrawLines();

            DrawCircumCenters();

            DrawVertices();

        }

        public void DrawCircumCenters()
        {
            foreach(Face face in graph.Faces)
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

                Vertex c = face.Circumcenter();
                float d = face.Diameter();

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
        
        Brush brush = Brushes.Red;
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            HalfEdge he = null;
            
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
