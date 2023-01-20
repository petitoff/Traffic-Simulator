using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Traffic_Simulator.Const;
using Traffic_Simulator.Model;
using Traffic_Simulator.ViewModel;

namespace Traffic_Simulator.Simulation;

public class CarsManagement
{
    public bool IsTrainActive;
    private readonly MainViewModel _mainViewModel;
    private readonly MainWindow _mainWindow;
    private int _numberOfCars;

    public CarsManagement(MainViewModel mainViewModel, MainWindow mainWindow)
    {
        _mainViewModel = mainViewModel;
        _mainWindow = mainWindow;
    }

    /// <summary>
    /// Method to start car simulation
    /// </summary>
    public void StartAnimation()
    {
        while (_mainViewModel.IsAnimationActive)
        {
            try
            {
                var localNumberOfCars = int.Parse(_mainViewModel.NumberOfCars);

                if (localNumberOfCars == 0)
                {
                    continue;
                }
                
                if (localNumberOfCars != _numberOfCars)
                {
                    // remove all cars
                    _mainViewModel.Cars.Clear();

                    // stop all thread from CarsThreads
                    foreach (var carThread in _mainViewModel.CarsThreads)
                    {
                        carThread.Abort();
                    }

                    _numberOfCars = localNumberOfCars;
                }

                if (_mainViewModel.Cars.Count < _numberOfCars)
                {
                    for (int i = 0; i < _numberOfCars; i++)
                    {
                        CreateCar(TraversalDirection.FromTopToBottom);

                        // get last car from list
                        var car = _mainViewModel.Cars.Last();
                        CreateCarThread(car);

                        Thread.Sleep(2000);
                    }
                }
            }
            catch (Exception e)
            {
                _mainViewModel.NumberOfCars = "0";
                MessageBox.Show($"Error: {e.Message}\nSolution: Please enter a valid number of cars");
            }
        }
    }

    /// <summary>
    /// Method to start train simulation
    /// </summary>
    public void StartTrain()
    {
        while (true)
        {
            var random = new Random();
            var randomNumber = random.Next(10, 15);
            randomNumber *= 1000;

            Thread.Sleep(randomNumber);
            _mainViewModel.TrainData = null;
            
            CreateTrain();
            CreateThreadForTrain(_mainViewModel.TrainData);
            
        }
    }

    /// <summary>
    /// Method to create a new car
    /// </summary>
    /// <param name="traversalDirection">The direction in which the car will move</param>
    private void CreateCar(TraversalDirection traversalDirection)
    {
        var topStartPoint = new Point(-20, 259);
        var bottomStartPoint = new Point(1220, 538);
        Point startPoint = traversalDirection == TraversalDirection.FromTopToBottom ? topStartPoint : bottomStartPoint;

        // get last car in _mainViewModel.Cars
        var lastCar = _mainViewModel.Cars.LastOrDefault();
        int localId = lastCar is not null ? lastCar.Car.Id + 1 : 0;

        _mainWindow.Dispatcher.Invoke(() =>
        {
            var car = new Car(localId, startPoint, 2, 0, Brushes.HotPink, traversalDirection);
            var carInstance = new CarData(car, _mainWindow, _mainViewModel, this);
            _mainViewModel.Cars.Add(carInstance);
        });
    }

    private void DeleteCar(int carId)
    {
        _mainWindow.Dispatcher.Invoke(() =>
        {
            var car = _mainViewModel.Cars.FirstOrDefault(c => c.Car.Id == carId);
            if (car is not null)
            {
                _mainViewModel.Cars.Remove(car);
            }
        });
    }

    private void CreateCarThread(CarData carData)
    {
        var t = new Thread(carData.StartMovingCar);
        t.Start();

        // add thread to list of thread car
        _mainViewModel.CarsThreads.Add(t);

        
    }

    private void CreateTrain()
    {
        Point startPoint = new Point(1035, -150);

        _mainWindow.Dispatcher.Invoke(() =>
        {
            var train = new Train(0, startPoint, 1, -2.2, Brushes.HotPink, TraversalDirection.FromTopToBottom);
            var trainInstance = new TrainData(train, _mainViewModel);
            _mainViewModel.TrainData = trainInstance;
        });
    }

    private void CreateThreadForTrain(TrainData? trainInstance)
    {
        Thread t = new Thread(() => MoveTrain(trainInstance));
        t.Start();
    }

    private void CreateInstance<T>()
    {

    }

    private Thread CreateThreadForInstance<T>(T instance, Action<T> move)
    {
        return new Thread(() => move(instance));
    }

    private void MoveTrain(TrainData? trainInstance)
    {
        if (IsTrainActive)
        {
            return;
        }

        IsTrainActive = true;

        _mainViewModel.TrainActiveMessage = "Pociąg jest aktywny";

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

        _mainViewModel.TrainActiveMessage = "Pociąg jest nieaktywny";
    }
}