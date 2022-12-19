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
            //Rectangle road = new Rectangle
            //{
            //    Width = 500,
            //    Height = 20,
            //    Fill = Brushes.Gray
            //};

            //Canvas.SetTop(road, 210);

            //_mainWindow.MainCanvas.Children.Add(road);

            CreateCar();
        }

        private void StartAnimation(object obj)
        {
            Thread t = new Thread(MoveCar);
            t.Start();
        }

        private void CreateCar()
        {
            Car car = new Car(new Point(0, 218), 5, 0, Brushes.Red);
            AddCarToMainCanvas(car);
            _cars.Add(car);
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
                Thread.Sleep(50);
            }

            _mainWindow.MainCanvas.Dispatcher.Invoke(() => car1.Speed = 5);

            for (int i = 0; i < 500; i++)
            {
                _mainWindow.Dispatcher.Invoke(() =>
                {
                    if (car1.Direction >= -3.1)
                    {
                        car1.Direction -= 0.1;
                        car1.Position = new Point(car1.Position.X, car1.Position.Y + 1);
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

                if (car1PositionLeft < 262)
                {
                    break;
                }
                Thread.Sleep(50);
            }

            for(int i = 0; i < 500; i++)
            {
                _mainWindow.Dispatcher.Invoke(() =>
                {
                    if (car1.Direction <=0)
                    {
                        car1.Direction += 0.05;
                        car1.Position = new Point(car1.Position.X, car1.Position.Y + 1);
                    }
                    else
                    {
                        car1.Direction = 0;
                    }

                    car1.Position = car1.Position with { X = car1.Position.X + 1 };
                    car1.UpdateShape(_mainWindow.MainCanvas);
                    car1.UpdatePosition();
                });
                Thread.Sleep(50);
            }
        }
    }
}
