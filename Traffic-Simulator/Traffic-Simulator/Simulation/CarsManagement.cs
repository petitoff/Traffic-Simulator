using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Traffic_Simulator.Const;
using Traffic_Simulator.Model;
using Traffic_Simulator.ViewModel;

namespace Traffic_Simulator.Simulation;

public class CarsManagement
{
    private readonly MainViewModel _mainViewModel;
    private readonly MainWindow _mainWindow;
    public bool IsTrainActive;

    public CarsManagement(MainViewModel mainViewModel, MainWindow mainWindow)
    {
        _mainViewModel = mainViewModel;
        _mainWindow = mainWindow;
    }

    public void StartAnimation()
    {
        CreateCar(TraversalDirection.FromTopToBottom);

        MoveCar(_mainViewModel.Cars[0]);
        _mainViewModel.Cars.RemoveAt(0);
    }

    public void StartTrain()
    {
        CreateTrain();

        MoveTrain(_mainViewModel.Trains[0]);
        _mainViewModel.Trains.RemoveAt(0);
    }

    private void CreateCar(TraversalDirection traversalDirection)
    {
        var topStartPoint = new Point(-20, 259);
        var bottomStartPoint = new Point(1220, 538);
        Point startPoint = traversalDirection == TraversalDirection.FromTopToBottom ? topStartPoint : bottomStartPoint;

        int localId = _mainViewModel.Cars.Count + 1;

        Car car;

        _mainWindow.Dispatcher.Invoke(() =>
        {
            car = new Car(localId, startPoint, 2, 0, Brushes.HotPink, traversalDirection);
            var carInstance = new CarInstance(car, _mainViewModel);
            _mainViewModel.Cars.Add(carInstance);
        });
    }

    private void CreateTrain()
    {
        Train train;

        Point startPoint = new Point(1035, -150);

        int localId = _mainViewModel.Trains.Count + 1;

        _mainWindow.Dispatcher.Invoke(() =>
        {
            train = new Train(localId, startPoint, 1, -2.2, Brushes.HotPink, TraversalDirection.FromTopToBottom);
            var trainInstance = new TrainInstance(train, _mainViewModel);
            _mainViewModel.Trains.Add(trainInstance);
        });
    }

    private void MoveTrain(TrainInstance trainInstance)
    {
        Thread t = new Thread(() => MoveTrainWork(trainInstance));
        t.Start();
    }

    private void MoveTrainWork(TrainInstance trainInstance)
    {
        IsTrainActive = true;
        while (true)
        {
            double distanceFromTopBorder = 0;
            _mainWindow.Dispatcher.Invoke(() =>
            {
                trainInstance.Train.Position =
                    trainInstance.Train.Position with { X = trainInstance.Train.Position.X + 0.6 };
                trainInstance.Train.UpdateShape(_mainWindow.MainCanvas);
                trainInstance.Train.UpdatePosition();

                distanceFromTopBorder = Canvas.GetTop(trainInstance.Train.Shape);

            });

            if (distanceFromTopBorder > 800)
            {
                break;
            }

            Thread.Sleep(3);
        }

        IsTrainActive = false;

        _mainViewModel.TrainActive = "Pociąg jest nieaktywny";
    }

    private void MoveCar(CarInstance carInstance)
    {
        Thread t = carInstance.Car.TraversalDirection == TraversalDirection.FromTopToBottom
            ? new Thread(() => MoveCarFromTop(carInstance))
            : new Thread(() => MoveCarFromBottom(carInstance));

        t.Start();
    }

    private void MoveCarFromTop(CarInstance carInstance)
    {
        // do prawej
        while (carInstance.Car.Position.X <= 780)
        {
            _mainWindow.Dispatcher.Invoke(() =>
            {
                carInstance.Car.Position = new Point(carInstance.Car.Position.X + 1, carInstance.Car.Position.Y);
                carInstance.Car.UpdateShape(_mainWindow.MainCanvas);
                carInstance.Car.UpdatePosition();


            });

            Thread.Sleep(3);
        }

        // zakręt w prawo i do lewej
        while (true)
        {
            double distanceFromLeftBorder = 0;
            _mainWindow.Dispatcher.Invoke(() =>
            {
                if (carInstance.Car.Direction >= -3.1)
                {
                    carInstance.Car.Direction -= 0.025;
                    carInstance.Car.UpdateDirection();
                }
                else
                {
                    carInstance.Car.Direction = -3.144;
                }

                carInstance.Car.Position = carInstance.Car.Position with { X = carInstance.Car.Position.X + 0.6 };
                carInstance.Car.UpdateShape(_mainWindow.MainCanvas);
                carInstance.Car.UpdatePosition();

                distanceFromLeftBorder = Canvas.GetLeft(carInstance.Car.Shape);

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
                if (carInstance.Car.Direction < 0)
                {
                    carInstance.Car.Direction += 0.0151;
                    carInstance.Car.UpdateDirection();
                }
                else
                {
                    carInstance.Car.Direction = 0;
                }

                carInstance.Car.Position = carInstance.Car.Position with { X = carInstance.Car.Position.X + 0.00001 };
                carInstance.Car.UpdateShape(_mainWindow.MainCanvas);
                carInstance.Car.UpdatePosition();

                distanceFromLeftBorder = Canvas.GetLeft(carInstance.Car.Shape);
            });


            if (distanceFromLeftBorder > 500)
            {
                break;
            }

            Thread.Sleep(3);
        }

        // pociąg

        while (true)
        {
            double distanceFromLeftBorder = 0;

            if (IsTrainActive)
            {
                _mainWindow.Dispatcher.Invoke(() =>
                {

                    if (carInstance.Car.Speed > 0)
                    {
                        carInstance.Car.Speed -= 0.01;
                        carInstance.Car.Position =
                            carInstance.Car.Position with { X = carInstance.Car.Position.X + 0.6 };
                    }
                    else
                    {
                        carInstance.Car.Speed = 0;
                    }

                    carInstance.Car.UpdateShape(_mainWindow.MainCanvas);
                    carInstance.Car.UpdatePosition();
                });
            }
            else
            {

                if (carInstance.Car.Speed < 2)
                {
                    carInstance.Car.Speed += 0.01;
                }
                else
                {
                    carInstance.Car.Speed = 2;
                }
                
                _mainWindow.Dispatcher.Invoke(() =>
                {
                    carInstance.Car.Position = carInstance.Car.Position with { X = carInstance.Car.Position.X + 0.6 };
                    carInstance.Car.UpdateShape(_mainWindow.MainCanvas);
                    carInstance.Car.UpdatePosition();

                });
            }

            _mainWindow.Dispatcher.Invoke(() => { distanceFromLeftBorder = Canvas.GetLeft(carInstance.Car.Shape); });

            if (distanceFromLeftBorder > 1200)
            {
                break;
            }

            Thread.Sleep(3);
        }

    }

    private void MoveCarFromBottom(CarInstance carInstance)
    {
        throw new System.NotImplementedException();
    }
}