using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Traffic_Simulator.Command;
using Traffic_Simulator.Const;
using Traffic_Simulator.Model;

namespace Traffic_Simulator.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private readonly MainWindow _mainWindow;
        private string _bgImage = @"C:\Users\petit\Desktop\repos\Traffic-Simulator\Traffic-Simulator\Traffic-Simulator\Assets\Image\mapa_v3.png";
        private Thread _mainThread;
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

        public void AbortMainThread()
        {
            // TODO: Making threads stop correctly
            _mainThread.Abort();
        }

        private void CreateRoad(object obj)
        {
            //CreateCar();
        }

        private void StartAnimation(object obj)
        {
            _mainThread = new Thread(ManageAnimation);
            _mainThread.Start();
        }

        private void ManageAnimation()
        {
            _mainWindow.Dispatcher.Invoke(() =>
            {
                CreateCar(TopOrButton.FromTopToBottom);
                CreateCar(TopOrButton.FromBottomToTop);
            });

            for (int i = 0; i < 2; i++)
            {
                Thread t = _cars[i].TopOrButton == TopOrButton.FromTopToBottom
                    ? new Thread(() => MoveCar(_cars[i]))
                    : new Thread(() => MoveCarFromBottomToTop(_cars[i]));
                t.Start();

                Thread.Sleep(1000);
            }
        }

        private void CreateCar(TopOrButton topOrButton)
        {
            Car car;
            Point startPoint = topOrButton == TopOrButton.FromTopToBottom ? new Point(-20, 218) : new Point(1200, 538);


            if (_cars.Count == 0)
            {
                car = new Car(id: 0, startPoint, 5, 0, Brushes.Red, topOrButton);
            }
            else
            {
                var carIdBefore = _cars.Last().Id + 1;
                car = new Car(carIdBefore, startPoint, 5, 0, Brushes.Red, topOrButton);
            }

            AddCarToMainCanvas(car);
            _cars.Add(car);
            car.UpdateShape(_mainWindow.MainCanvas);
            car.UpdatePosition();
        }

        private void AddCarToMainCanvas(Car car)
        {
            _mainWindow.MainCanvas.Children.Add(car.Shape);
        }

        private void MoveCar(Car car)
        {
            if (_cars.Count <= 0)
            {
                return;
            }

            for (int i = 0; i < 800; i++)
            {
                _mainWindow.Dispatcher.Invoke(() =>
                {
                    car.Position = new Point(car.Position.X + 1, 218);
                    car.UpdateShape(_mainWindow.MainCanvas);
                    car.UpdatePosition();
                });

                double car1PositionLeft = 0;

                _mainWindow.Dispatcher.Invoke(() =>
                {
                    car1PositionLeft = Canvas.GetLeft(car.Shape);
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
                    if (car.Direction >= -3.1)
                    {
                        car.Direction -= 0.1;
                        car.Position = car.Position with { Y = car.Position.Y + 1.5 };
                    }
                    else
                    {
                        car.Direction = -3.144;
                    }

                    car.Position = car.Position with { X = car.Position.X + 0.8 };
                    car.UpdateShape(_mainWindow.MainCanvas);
                    car.UpdatePosition();
                });

                double car1PositionLeft = 0;

                _mainWindow.Dispatcher.Invoke(() =>
                {
                    car1PositionLeft = Canvas.GetLeft(car.Shape);
                });

                if (car1PositionLeft < 180)
                {
                    break;
                }
                Thread.Sleep(10);
            }

            for (int i = 0; i < 500; i++)
            {
                _mainWindow.Dispatcher.Invoke(() =>
                {
                    if (car.Direction < 0)
                    {
                        car.Direction += 0.08;
                        car.Position = car.Position with { Y = car.Position.Y + 2 };
                    }
                    else
                    {
                        car.Direction = 0;
                    }

                    car.Position = car.Position with { X = car.Position.X + 1 };
                    car.UpdateShape(_mainWindow.MainCanvas);
                    car.UpdatePosition();
                });

                double car1PositionLeft = 0;

                _mainWindow.Dispatcher.Invoke(() =>
                {
                    car1PositionLeft = Canvas.GetLeft(car.Shape);
                });

                if (car1PositionLeft > 1200)
                {
                    break;
                }
                Thread.Sleep(10);
            }
        }

        private void MoveCarFromBottomToTop(Car car)
        {
            for (int i = 0; i < 500; i++)
            {
                _mainWindow.Dispatcher.Invoke(() =>
                {
                    car.Direction = -3.144;
                    car.Position = car.Position with { X = car.Position.X + 1 };
                    car.UpdateShape(_mainWindow.MainCanvas);
                    car.UpdatePosition();
                });

                double car1PositionLeft = 0;

                _mainWindow.Dispatcher.Invoke(() =>
                {
                    car1PositionLeft = Canvas.GetLeft(car.Shape);
                });

                if (car1PositionLeft < 230)
                {
                    break;
                }
                Thread.Sleep(10);
            }

            for (int i = 0; i < 500; i++)
            {
                _mainWindow.Dispatcher.Invoke(() =>
                {
                    if (car.Direction >= -6.24)
                    {
                        car.Direction -= 0.07;
                        //car.Position = car.Position with { Y = car.Position.Y + 1 };
                    }
                    else
                    {
                        car.Direction = -6.24;
                    }

                    car.Position = car.Position with { X = car.Position.X + 1};
                    car.UpdateShape(_mainWindow.MainCanvas);
                    car.UpdatePosition();
                });

                double car1PositionLeft = 0;

                _mainWindow.Dispatcher.Invoke(() =>
                {
                    car1PositionLeft = Canvas.GetLeft(car.Shape);
                });

                if (car1PositionLeft > 818)
                {
                    break;
                }
                Thread.Sleep(10);
            }

            for (int i = 0; i < 500; i++)
            {
                _mainWindow.Dispatcher.Invoke(() =>
                {
                    if (car.Direction <= -3.09)
                    {
                        car.Direction += 0.06;
                        car.Position = car.Position with { Y = car.Position.Y - 0.8 };
                    }
                    else
                    {
                        car.Direction = -3.09;
                    }

                    car.Position = car.Position with { X = car.Position.X + 1 };
                    car.UpdateShape(_mainWindow.MainCanvas);
                    car.UpdatePosition();
                });

                double car1PositionLeft = 0;

                _mainWindow.Dispatcher.Invoke(() =>
                {
                    car1PositionLeft = Canvas.GetLeft(car.Shape);
                });

                if (car1PositionLeft < -50)
                {
                    break;
                }
                Thread.Sleep(10);
            }
        }
    }
}
