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

namespace Traffic_Simulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Random rnd = new Random();

            Rect r1 = new Rect();
            r1.X = rnd.Next(500);
            r1.Y = rnd.Next(500);
            r1.Width = rnd.Next(50, 100);
            r1.Height = rnd.Next(50, 100);
            R1 = r1;

            Rect r2 = new Rect();
            r2.X = rnd.Next(500);
            r2.Y = rnd.Next(500);
            r2.Width = rnd.Next(50, 100);
            r2.Height = rnd.Next(50, 100);
            R2 = r2;


            DataContext = this;
        }

        public Rect R1 { get; set; }
        public Rect R2 { get; set; }
    }
}
