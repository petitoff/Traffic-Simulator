using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
            CreateCar();
        }

        private void StartAnimation(object obj)
        {
            Thread t = new Thread(MoveCar);
            t.Start();
        }

        private void CreateCar()
        {
            Car car = new Car(new Point(-20, 218), 5, 0, Brushes.Red);
            AddCarToMainCanvas(car);
            _cars.Add(car);
            car.UpdateShape(_mainWindow.MainCanvas);
            car.UpdatePosition();
        }

        private void AddCarToMainCanvas(Car car)
        {
            _mainWindow.MainCanvas.Children.Add(car.Shape);
        }

        private void MoveCar()
        {
            if (_cars.Count <= 0)
            {
                return;
            }

            var car1 = _cars[0];

            for (int i = 0; i < 800; i++)
            {
                _mainWindow.Dispatcher.Invoke(() =>
                {
                    car1.Position = new Point(car1.Position.X + 1, 218);
                    car1.UpdateShape(_mainWindow.MainCanvas);
                    car1.UpdatePosition();
                });

                double car1PositionLeft = 0;

                _mainWindow.Dispatcher.Invoke(() =>
                {
                    car1PositionLeft = Canvas.GetLeft(car1.Shape);
                });

                if (car1PositionLeft > 800)
                {
                    break;
                }
                Thread.Sleep(10);
            }

            for (int i = 0; i < 500; i++)
            {
                _mainWindow.Dispatcher.Invoke(() =>
                {
                    if (car1.Direction >= -3.1)
                    {
                        car1.Direction -= 0.1;
                        car1.Position = car1.Position with { Y = car1.Position.Y + 1 };
                    }
                    else
                    {
                        car1.Direction = -3.12;
                    }

                    car1.Position = car1.Position with { X = car1.Position.X + 1 };
                    car1.UpdateShape(_mainWindow.MainCanvas);
                    car1.UpdatePosition();
                });

                double car1PositionLeft = 0;

                _mainWindow.Dispatcher.Invoke(() =>
                {
                    car1PositionLeft = Canvas.GetLeft(car1.Shape);
                });

                if (car1PositionLeft < 200)
                {
                    break;
                }
                Thread.Sleep(10);
            }

            for (int i = 0; i < 500; i++)
            {
                _mainWindow.Dispatcher.Invoke(() =>
                {
                    if (car1.Direction < 0)
                    {
                        car1.Direction += 0.09;
                        car1.Position = car1.Position with { Y = car1.Position.Y + 3 };
                    }
                    else
                    {
                        car1.Direction = 0;
                    }

                    car1.Position = car1.Position with { X = car1.Position.X + 0.7 };
                    car1.UpdateShape(_mainWindow.MainCanvas);
                    car1.UpdatePosition();
                });

                double car1PositionLeft = 0;

                _mainWindow.Dispatcher.Invoke(() =>
                {
                    car1PositionLeft = Canvas.GetLeft(car1.Shape);
                });

                if (car1PositionLeft > 1200)
                {
                    break;
                }
                Thread.Sleep(10);
            }
        }
    }
}
