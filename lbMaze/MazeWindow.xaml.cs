using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace lbMaze
{
    public class Nodes
    {
        public Rectangle rectangle { get; set; }        
        
        public int x { get; set; }
        public int y { get; set; }
        public static int mazeheight { get; set; }
        public static int mazewidth { get; set; }
        public bool isPath { get; set; }
        public Nodes parent { get; set; }
        public static Dictionary<string, Nodes> node { get; set; } = new Dictionary<string, Nodes>();
        public static Dictionary<string, Nodes> opennodes { get; set; } = new Dictionary<string, Nodes>();
        public static Dictionary<string, Nodes> closednodes { get; set; } = new Dictionary<string, Nodes>();
        public static int startx { get; set; }
        public static int starty { get; set; }
        public static int endx { get; set; }
        public static int endy { get; set; }
        public static Nodes start { get; set; }
        public int f { get; set; }
        public int g { get; set; }
        public int h { get; set; }


        public Nodes(int x, int y)
        {
            rectangle = new Rectangle();
            rectangle.Width = 10;
            rectangle.Height = 10;
            rectangle.Stroke = Brushes.Black;

            this.x = x;
            this.y = y;

            if (this.x == Nodes.startx && this.y == Nodes.starty)
                this.rectangle.Fill = Brushes.Green;

            else
                if (this.x == Nodes.endx && this.y == Nodes.endy)
                this.rectangle.Fill = Brushes.Red;

            else
                this.rectangle.Fill = Brushes.White;

            isPath = true;

            g = -1;
            h = (Math.Abs(this.x - endx) + Math.Abs(this.y - endy));
        }

        public Nodes() { }

        public static void move(Nodes current)
        {
            closednodes.Add(current.x.ToString("00") + current.y.ToString("00"), current);
            opennodes.Remove(current.x.ToString("00") + current.y.ToString("00"));
            getAllNearNodes(current);

            if (Nodes.opennodes.Count != 0)
            {
                Nodes nextnode = new Nodes();
                var curmin = Nodes.mazewidth * Nodes.mazeheight;
                foreach (Nodes n in opennodes.Values)
                {
                    if (n.g < curmin)
                    {
                        nextnode = n;
                        curmin = n.g;
                    }
                }

                if ((nextnode.x == endx || nextnode.y == endy) && (Math.Abs(nextnode.x - endx) == 1 || Math.Abs(nextnode.y - endy) == 1))
                    getPath(nextnode);

                else
                    move(nextnode);
            }
        }

        private static void getAllNearNodes(Nodes current)
        {
            if (current.x != 0)
            {
                getNearNode(current, -1, 0);
            }

            if (current.x != Nodes.mazewidth - 1)
            {
                getNearNode(current, 1, 0);
            }

            if (current.y != 0)
            {
                getNearNode(current, 0, -1);
            }

            if (current.y != Nodes.mazeheight - 1)
            {
                getNearNode(current, 0, 1);
            }
        }

        private static void getNearNode(Nodes current, int deltax, int deltay)
        {
            var tempkey = (current.x + deltax).ToString("00") + (current.y + deltay).ToString("00");
            if (!closednodes.ContainsKey(tempkey) && node[tempkey].isPath)
            {
                if (!opennodes.ContainsKey(tempkey))
                {
                    node[tempkey].g = current.g + 1;
                    node[tempkey].f = current.h + current.g;
                    node[tempkey].parent = current;
                    opennodes.Add(tempkey, node[tempkey]);
                }
                else
                {
                    if (node[tempkey].g < current.g + 1)
                        node[tempkey].parent = current;
                }
            }
        }

        public static void getPath(Nodes theNode)
        {
            theNode.rectangle.Fill = Brushes.DarkOliveGreen;
            if (theNode.parent.rectangle.Fill == Brushes.White)
                getPath(theNode.parent);
        }
    }

    public partial class MazeWindow : Window
    {
        public void HandleMousePreviewEvent(Nodes sender)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                if (sender.isPath)
                {
                    sender.isPath = false;
                    sender.rectangle.Fill = Brushes.Black;
                }
                else
                {
                    sender.isPath = true;
                    sender.rectangle.Fill = Brushes.White;
                }
            }

        }
        public void buildmaze(int x, int y, int startx, int starty, int endx, int endy)
        {
            
            Nodes.startx = startx;
            Nodes.starty = starty;
            Nodes.endx = endx;
            Nodes.endy = endy;
            Nodes.mazewidth = x;
            Nodes.mazeheight = y;
            Nodes.node.Clear();
            Nodes.opennodes.Clear();
            Nodes.closednodes.Clear();
            Application.Current.MainWindow = this;
            Application.Current.MainWindow.Height = 60 + y * 15;
            Application.Current.MainWindow.Width = 30 + x * 15;
            

            for (int i = 0; i < x; i++)
            {

                var z = new List<Nodes>();
                var r = new RowDefinition();
                r.Height = new GridLength(10);
                grid.RowDefinitions.Add(r);
                for (int j = 0; j < y; j++)
                {

                    if (i == 0)
                    {
                        var c = new ColumnDefinition();
                        c.Width = new GridLength(10);


                        grid.ColumnDefinitions.Add(c);
                    }
                    Nodes n = new Nodes(i, j);
                    z.Add(n);

                    n.rectangle.MouseEnter += (sender, e) => HandleMousePreviewEvent(n);
                    Nodes.node.Add(n.x.ToString("00") + n.y.ToString("00"), n);


                    grid.Children.Add(z[j].rectangle);
                    Grid.SetRow(z[j].rectangle, i);
                    Grid.SetColumn(z[j].rectangle, j);
                }
            }

            var starter = Nodes.node[Nodes.startx.ToString("00") + Nodes.starty.ToString("00")];
            Nodes.start = starter;
            
        }

        
        public MazeWindow()
        {
            InitializeComponent();
        } 
        
        private void SolveMaze(object sender, RoutedEventArgs e)
        {
            Nodes.opennodes.Clear();
            Nodes.closednodes.Clear();
            Nodes.opennodes.Add(Nodes.startx.ToString("00") + Nodes.starty.ToString("00"), Nodes.start);
            Nodes.move(Nodes.start);

        }

        

        private void CreateMazeButton(object sender, RoutedEventArgs e)
        {           
            foreach (Nodes n in Nodes.node.Values)
            {
                if (n.rectangle.Fill == Brushes.Black || n.rectangle.Fill == Brushes.DarkOliveGreen)
                {
                    n.rectangle.Fill = Brushes.White;
                    n.isPath = true;
                }
            }
        }
    }
}
