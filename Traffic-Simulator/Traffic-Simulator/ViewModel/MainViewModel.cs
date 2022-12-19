using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Traffic_Simulator.Command;

namespace Traffic_Simulator.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private readonly MainWindow _mainWindow;
        private string _bgImage = @"C:\Users\petit\Desktop\repos\Traffic-Simulator\Traffic-Simulator\Traffic-Simulator\Assets\Image\mapa_v3.png";

        private List<Rectangle> _cars = new List<Rectangle>();

        public MainViewModel(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            
            StartAnimationCommand = new DelegateCommand(StartAnimation);
            CreateRoadCommand = new DelegateCommand(CreateRoad);
        }

        public DelegateCommand StartAnimationCommand { get; }
        public DelegateCommand CreateRoadCommand { get; }

        public string BgImage
        {
            get => _bgImage;
            set
            {
                _bgImage = value;
                OnPropertyChanged();
            }
        }

        private void CreateRoad(object obj)
        {
            Polygon polygon = new Polygon();

            // Set the Stroke and StrokeThickness properties of the polygon.
            polygon.Stroke = Brushes.Black;
            polygon.StrokeThickness = 0.1;
            polygon.Name = "RoadLeft";

            // Set the Fill property of the polygon.
            polygon.Fill = Brushes.Red;

            // Create a collection of points for the polygon.
            PointCollection points = new PointCollection();
            points.Add(new Point(0, 222));
            points.Add(new Point(0, 245));
            points.Add(new Point(755, 245));
            points.Add(new Point(755, 222));

            // Set the Points property of the polygon to the collection of points.
            polygon.Points = points;

            // Add the polygon to the visual tree.
            _mainWindow.MainCanvas.Children.Add(polygon);
        }

        private void StartAnimation(object obj)
        {
            CreateCar();
        }

        private void CreateCar()
        {
            Rectangle car = new Rectangle();
            car.Width = 30;
            car.Height = 25;
            car.Fill = Brushes.Red;
            Canvas.SetLeft(car, 0);
            Canvas.SetTop(car, 222);
            _mainWindow.MainCanvas.Children.Add(car);
            _cars.Add(car);
        }

        private void MoveCar()
        {

        }
    }
}
