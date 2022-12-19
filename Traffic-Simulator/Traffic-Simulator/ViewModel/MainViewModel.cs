using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Traffic_Simulator.Command;
using Traffic_Simulator.Model;

namespace Traffic_Simulator.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private readonly MainWindow _mainWindow;
        private string _bgImage = @"C:\Users\petit\Desktop\repos\Traffic-Simulator\Traffic-Simulator\Traffic-Simulator\Assets\Image\mapa_v3.png";

        private List<Car> _cars = new List<Car>();

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
            Rectangle road = new Rectangle
            {
                Width = 500,
                Height = 20,
                Fill = Brushes.Gray
            };

            Canvas.SetTop(road, 210);

            _mainWindow.MainCanvas.Children.Add(road);

            CreateCar();
        }

        private void StartAnimation(object obj)
        {
            Thread t = new Thread(MoveCar);
            t.Start();
        }

        private void CreateCar()
        {
            Car car = new Car(new Point(50, 50), 50, 0, Brushes.Red);
            AddCarToMainCanvas(car);
            _cars.Add(car);
        }

        private void AddCarToMainCanvas(Car car)
        {
            _mainWindow.MainCanvas.Children.Add(car.Shape);
        }

        private void MoveCar()
        {
            var car1 = _cars[0];
            
            for (int i = 0; i < 500; i++)
            {
                _mainWindow.Dispatcher.Invoke(() =>
                {
                    car1.Position = new Point(i, 218);
                    car1.UpdateShape(_mainWindow.MainCanvas);
                });
                Thread.Sleep(1);
            }

        }
    }
}
