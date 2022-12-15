using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Traffic_Simulator.ViewModel
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly MainWindow _mainWindow;
        private Rect _r1;
        private Rect _r2;
        private bool _isRunning;

        public MainViewModel(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;

            _isRunning = true;

            _mainWindow.Loaded += MainWindowOnLoaded;
            _mainWindow.Closing += MainWindowOnClosing;
        }

        public Rect R1
        {
            get => _r1;
            set
            {
                _r1 = value;
                OnPropertyChanged();
            }
        }

        public Rect R2
        {
            get => _r2;
            set
            {
                _r2 = value;
                OnPropertyChanged();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SetRandomPositionOfFixedRec()
        {

            while (_isRunning)
            {
                Thread.Sleep(2000);

                Random rnd = new Random();

                _r1.X = rnd.Next(500);
                _r1.Y = rnd.Next(500);
                _r1.Width = rnd.Next(50, 100);
                _r1.Height = rnd.Next(50, 100);
                R1 = _r1;
            }
        }

        private void MainWindowOnClosing(object sender, CancelEventArgs e)
        {
            _isRunning = false;
        }

        private void MainWindowOnLoaded(object sender, RoutedEventArgs e)
        {
            CreateRec();
        }

        private void CreateRec()
        {
            Random rnd = new Random();

            _r1 = new Rect();
            _r2 = new Rect();

            _r1.X = rnd.Next(500);
            _r1.Y = rnd.Next(500);
            _r1.Width = 50;
            _r1.Height = 100;
            R1 = _r1;

            _r2.X = rnd.Next(500);
            _r2.Y = rnd.Next(500);
            _r2.Width = 50;
            _r2.Height = 50;
            R2 = _r2;

            //Thread t = new Thread(SetRandomPositionOfFixedRec);
            //t.Start();
        }

        private void animation()
        {
            // Create the Storyboard object
            var storyboard = new Storyboard();
            storyboard.Duration = TimeSpan.FromSeconds(10);

            // Create a DoubleAnimationUsingPath to animate the Canvas.Left property
            var leftAnimation = new DoubleAnimationUsingPath();
            leftAnimation.PathGeometry = new PathGeometry(new[]
            {
                new PathFigure(new Point(0, 0), new[]
                {
                    new LineSegment(new Point(100, 0), true),
                    new LineSegment(new Point(150, 50), true),
                    new ArcSegment(new Point(200, 100), new Size(50, 50), 0, false, SweepDirection.Clockwise, true)
                }, false)
            });
            Storyboard.SetTarget(leftAnimation, CarShape);
            Storyboard.SetTargetProperty(leftAnimation, new PropertyPath("(Canvas.Left)"));

            // Create a DoubleAnimationUsingPath to animate the Canvas.Top property
            var topAnimation = new DoubleAnimationUsingPath();
            topAnimation.PathGeometry = new PathGeometry(new[]
            {
                new PathFigure(new Point(0, 0), new[]
                {
                    new LineSegment(new Point(100, 0), true),
                    new LineSegment(new Point(150, 50), true)
                }, false)
            });
        }

    }
}
}
