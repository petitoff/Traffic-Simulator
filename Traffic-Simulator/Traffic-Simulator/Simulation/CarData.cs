using System;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Traffic_Simulator.Const;
using Traffic_Simulator.Model;
using Traffic_Simulator.ViewModel;

namespace Traffic_Simulator.Simulation
{
    public class CarData
    {
        public readonly Car Car;
        private readonly MainViewModel _mainViewModel;
        private MainWindow _mainWindow;
        private readonly CarsManagement _carsManagement;
        private int _cordXOfRailway = 1090;

        public CarData(Car car, MainWindow mainWindow, MainViewModel mainViewModel, CarsManagement carsManagement)
        {
            Car = car;
            _mainWindow = mainWindow;
            _mainViewModel = mainViewModel;
            _carsManagement = carsManagement;
        }

        public async void StartMovingCar()
        {
            if (Car.TraversalDirection == TraversalDirection.FromTopToBottom)
            {
                await MoveCarFromTop();
            }

            if (Car.TraversalDirection == TraversalDirection.FromBottomToTop)
            {
                await MoveCarFromBottom();
            }

            _carsManagement.DeleteCar(Car.Id);
        }

        /// <summary>
        /// Method that check if car needs to be stopped
        /// </summary>
        /// <returns></returns>
        private bool CheckDistanceBetweenNearCar()
        {
            if (_mainViewModel.Cars.Count <= 1) return false;
            
            var carsWithSameTraversalDirection = _mainViewModel.Cars.Where(x => x.Car.TraversalDirection == Car.TraversalDirection).ToList();

            var carAfterThatCar = carsWithSameTraversalDirection.FirstOrDefault(x => x.Car.Id == Car.Id - 1);

            if (carAfterThatCar == null)
            {
                return false;
            }

            var positionOfTheCarAfterThatCar = carAfterThatCar.Car.Position;
            //foreach (var carData in carsWithSameTraversalDirection)
            //{
            //    if (carData.Car.Id == Car.Id - 1 && carData.Car.TraversalDirection == Car.TraversalDirection)
            //    {
            //        positionOfTheCarAfterThatCar = carData.Car.Position;
            //    }
            //}

            // sub Position of Car and Position of the car after that car
            double distanceBetweenCars = Math.Abs(Car.Position.X - positionOfTheCarAfterThatCar.X);
            double distanceBetweenCarsY = Math.Abs(Car.Position.Y - positionOfTheCarAfterThatCar.Y);


            return distanceBetweenCars < 200 && distanceBetweenCarsY < 40;
        }

        private void StoppingCar()
        {
            _mainWindow.Dispatcher.Invoke(() =>
            {

                if (Car.Speed > 0)
                {
                    Car.Speed -= 0.2;
                    Car.Position =
                        Car.Position with
                        { X = Car.Position.X + 0.6 };
                }
                else
                {
                    Car.Speed = 0;
                }

                Car.UpdateShape(_mainWindow.MainCanvas);
                Car.UpdatePosition();
            });
        }

        private Task MoveCarFromTop()
        {
            // do prawej
            while (Car.Position.X <= 780)
            {
                _mainWindow.Dispatcher.Invoke(() =>
                {
                    Car.Position = new Point(Car.Position.X + 1, Car.Position.Y);
                    Car.UpdateShape(_mainWindow.MainCanvas);
                    Car.UpdatePosition();
                });

                Thread.Sleep(3);
            }

            // zakręt w prawo i do lewej
            while (true)
            {
                double distanceFromLeftBorder = 0;
                _mainWindow.Dispatcher.Invoke(() =>
                {
                    if (Car.Direction >= -3.1)
                    {
                        Car.Direction -= 0.025;
                        Car.UpdateDirection();
                    }
                    else
                    {
                        Car.Direction = -3.144;
                    }
                    Car.Position = Car.Position with { X = Car.Position.X + 0.6 };
                    Car.UpdateShape(_mainWindow.MainCanvas);
                    Car.UpdatePosition();

                    distanceFromLeftBorder = Canvas.GetLeft(Car.Shape);

                });

                if (distanceFromLeftBorder < 260)
                {
                    break;
                }

                Thread.Sleep(3);
            }

            while (true)
            {
                double distanceFromLeftBorder = 0;

                _mainWindow.Dispatcher.Invoke(() =>
                {
                    if (Car.Direction < 0)
                    {
                        Car.Direction += 0.0151;
                        Car.UpdateDirection();
                    }
                    else
                    {
                        Car.Direction = 0;
                    }

                    Car.Position = Car.Position with { X = Car.Position.X + 0.00001 };
                    Car.UpdateShape(_mainWindow.MainCanvas);
                    Car.UpdatePosition();

                    distanceFromLeftBorder = Canvas.GetLeft(Car.Shape);
                });

                if (distanceFromLeftBorder > 340)
                {
                    break;
                }

                Thread.Sleep(3);
            }

            // droga w prawo i pociąg
            while (true)
            {
                if (CheckDistanceBetweenNearCar())
                {
                    StoppingCar();

                    Thread.Sleep(3);

                    continue;
                }


                double distanceFromLeftBorder = 0;
                const int distanceCarFromRailway = 960;

                if (_carsManagement.IsTrainActive && Car.Position.X is < distanceCarFromRailway and > 600)
                {
                    _mainWindow.Dispatcher.Invoke(() =>
                    {

                        if (Car.Speed > 0)
                        {
                            Car.Speed -= 0.01;
                            Car.Position =
                                Car.Position with
                                { X = Car.Position.X + 0.6 };
                        }
                        else
                        {
                            Car.Speed = 0;
                        }

                        Car.UpdateShape(_mainWindow.MainCanvas);
                        Car.UpdatePosition();
                    });
                }
                else
                {

                    if (Car.Speed < 2)
                    {
                        Car.Speed += 0.01;
                    }
                    else
                    {
                        Car.Speed = 2;
                    }

                    _mainWindow.Dispatcher.Invoke(() =>
                    {
                        Car.Position = Car.Position with { X = Car.Position.X + 0.6 };
                        Car.UpdateShape(_mainWindow.MainCanvas);
                        Car.UpdatePosition();

                    });
                }

                _mainWindow.Dispatcher.Invoke(() => { distanceFromLeftBorder = Canvas.GetLeft(Car.Shape); });

                if (distanceFromLeftBorder > 1200)
                {
                    break;
                }

                Thread.Sleep(3);
            }

            return Task.CompletedTask;
        }

        private Task MoveCarFromBottom()
        {
            _mainWindow.Dispatcher.Invoke(() =>
            {
                Car.Direction = -3.143;
                Car.UpdateDirection();
            });

            while (true)
            {
                if (CheckDistanceBetweenNearCar())
                {
                    StoppingCar();

                    Thread.Sleep(3);

                    continue;
                }
                
                double distanceFromLeftBorder = 0;

                if (_carsManagement.IsTrainActive && Car.Position.X > _cordXOfRailway && Car.Position.X < 1150)
                {
                    _mainWindow.Dispatcher.Invoke(() =>
                    {

                        if (Car.Speed > 0)
                        {
                            Car.Speed -= 1;
                            Car.Position =
                                Car.Position with
                                { X = Car.Position.X + 1 };
                        }
                        else
                        {
                            Car.Speed = 0;
                        }

                        Car.UpdateShape(_mainWindow.MainCanvas);
                        Car.UpdatePosition();
                    });
                }
                else
                {
                    Car.Speed = 2;

                    _mainWindow.Dispatcher.Invoke(() =>
                    {
                        Car.Position = Car.Position with { X = Car.Position.X + 1 };
                        Car.UpdateShape(_mainWindow.MainCanvas);
                        Car.UpdatePosition();
                    });
                }

                _mainWindow.Dispatcher.Invoke(() =>
                {
                    distanceFromLeftBorder = Canvas.GetLeft(Car.Shape);
                });

                if (distanceFromLeftBorder < 240)
                {
                    break;
                }

                Thread.Sleep(3);
            }

            while (true)
            {
                double distanceFromLeftBorder = 0;

                _mainWindow.Dispatcher.Invoke(() =>
                {
                    if (Car.Direction >= -6.23)
                    {
                        Car.Direction -= 0.025;
                        //car.Position = car.Position with { Y = car.Position.Y + 1 };
                    }
                    else
                    {
                        Car.Direction = -6.23;
                    }

                    Car.Position = Car.Position with { X = Car.Position.X + 0.2 };
                    Car.UpdateShape(_mainWindow.MainCanvas);
                    Car.UpdatePosition();
                });

                _mainWindow.Dispatcher.Invoke(() =>
                {
                    distanceFromLeftBorder = Canvas.GetLeft(Car.Shape);
                });

                if (distanceFromLeftBorder > 818)
                {
                    break;
                }
                Thread.Sleep(3);
            }

            while (true)
            {
                double distanceFromLeftBorder = 0;

                _mainWindow.Dispatcher.Invoke(() =>
                {
                    if (Car.Direction <= -3.158)
                    {
                        Car.Direction += 0.017;
                    }
                    else
                    {
                        Car.Direction = -3.158;
                    }

                    Car.Position = Car.Position with { X = Car.Position.X + 0.3 };
                    Car.UpdateShape(_mainWindow.MainCanvas);
                    Car.UpdatePosition();
                });

                _mainWindow.Dispatcher.Invoke(() =>
                {
                    distanceFromLeftBorder = Canvas.GetLeft(Car.Shape);
                });

                if (distanceFromLeftBorder < -50)
                {
                    break;
                }
                Thread.Sleep(3);
            }

            return Task.CompletedTask;
        }
    }
}
