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

namespace lbMaze
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            int width = 0;
            int height = 0;
            int startx = 0;
            int starty = 0;
            int endx = 0;
            int endy = 0;
            
            try
            {
                width = int.Parse(textWidth.Text);
                height = int.Parse(textHeight.Text);
                startx = int.Parse(textStartX.Text);
                starty = int.Parse(textStartY.Text);
                endx = int.Parse(textEndX.Text);
                endy = int.Parse(textEndY.Text);

                
            }
            catch(Exception)
            {
                MessageBox.Show("Invalid parameters");
               
            }

            if (width >= 4 && width <= 50 && height >= 4 && height <= 50 && startx >= 0 && startx < width && starty >= 0 && starty < height && endx >= 0 && endx < width && endy >= 0 && endy < height && !(startx == endx && starty == endy)) 
            {
                MazeWindow mz = new MazeWindow();
                mz.Owner = this;
                
                mz.buildmaze(width, height, startx, starty, endx, endy);
                mz.Show();
            }

        }
    }
}
