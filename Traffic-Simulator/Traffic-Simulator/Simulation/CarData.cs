using System;
using System.Threading;
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

        public CarData(Car car, MainWindow mainWindow, MainViewModel mainViewModel, CarsManagement carsManagement)
        {
            Car = car;
            _mainWindow = mainWindow;
            _mainViewModel = mainViewModel;
            _carsManagement = carsManagement;
        }

        public void StartMovingCar()
        {
            if (Car.TraversalDirection == TraversalDirection.FromTopToBottom)
            {
                MoveCarFromTop();
            }
        }

        private void MoveCarFromTop()
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

                // get postion of the car after that car
                Point positionOfTheCarAfterThatCar = default;
                if (_mainViewModel.Cars.Count > 1)
                {
                    //positionOfTheCarAfterThatCar = _mainViewModel.Cars[Car.Id - 1].Car.Position;

                    // find Car by id in _mainViewModel.Cars where id is Car.Id - 1
                    foreach (var carData in _mainViewModel.Cars)
                    {
                        if (carData.Car.Id == Car.Id - 1)
                        {
                            positionOfTheCarAfterThatCar = carData.Car.Position;
                        }
                    }

                }

                // sub Position of Car and Position of the car after that car
                double distanceBetweenCars = Math.Abs(Car.Position.X - positionOfTheCarAfterThatCar.X);
                double distanceBetweenCarsY = Math.Abs(Car.Position.Y - positionOfTheCarAfterThatCar.Y);


                if (distanceFromLeftBorder > 500 || (distanceBetweenCars < 400 && distanceBetweenCarsY < 20))
                {
                    break;
                }

                Thread.Sleep(3);
            }

            // droga w prawo i pociąg

            while (true)
            {
                double distanceFromLeftBorder = 0;
                const int distanceCarFromRailway = 960;

                if (_carsManagement.IsTrainActive && Car.Position.X < distanceCarFromRailway)
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

        }
    }
}
