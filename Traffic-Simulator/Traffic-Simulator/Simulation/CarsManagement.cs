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
            // random number, 0 or 1
            var random = new Random().Next(0, 2);

            // if random number == 0 create new card from top to bottom
            // else create new card from bottom to top
            CreateNewCar(random == 0
                ? TraversalDirection.FromTopToBottom
                : TraversalDirection.FromBottomToTop);

            // get last car from list
            var car = _mainViewModel.Cars.Last();
            CreateCarThread(car);

            // random number from 2 to 4
            var randomSpawnCar = new Random().Next(2, 4);
            randomSpawnCar *= 1000;
            Thread.Sleep(randomSpawnCar);
        }
    }

    /// <summary>
    /// Method to start train simulation
    /// </summary>
    public void StartTrain()
    {
        while (_mainViewModel.IsAnimationActive)
        {

            if (IsTrainActive)
            {
                continue;
            }

            var random = new Random();
            //var randomNumber = random.Next(10, 15);
            var randomNumber = random.Next(1, 5);
            randomNumber *= 1000;

            Thread.Sleep(randomNumber);
            _mainViewModel.TrainData = null;

            CreateTrain();
            //CreateThreadForTrain(_mainViewModel.TrainData);
            CreateThreadForInstance(_mainViewModel.TrainData, MoveTrain);

        }
    }

    /// <summary>
    /// Method to delete car from canvas and list
    /// </summary>
    /// <param name="carId">Id of car to delete</param>
    public void DeleteCar(int carId)
    {
        var car = _mainViewModel.Cars.FirstOrDefault(c => c.Car.Id == carId);
        if (car is not null)
        {
            _mainViewModel.Cars.Remove(car);
            _mainViewModel.NumberOfCars = _mainViewModel.Cars.Count.ToString();
        }

        var carThread = _mainViewModel.CarsThreads.FirstOrDefault(c => c.ManagedThreadId == carId);
        if (carThread is not null)
        {
            _mainViewModel.CarsThreads.Remove(carThread);
        }
    }

    /// <summary>
    /// Method to create a new car
    /// </summary>
    /// <param name="traversalDirection">The direction in which the car will move</param>
    private void CreateNewCar(TraversalDirection traversalDirection)
    {
        var topStartPoint = new Point(-20, 259);
        var bottomStartPoint = new Point(1220, 633);
        Point startPoint = traversalDirection == TraversalDirection.FromTopToBottom ? topStartPoint : bottomStartPoint;

        // get list of Cars with the same traversalDirection
        var carsWithSameTraversalDirection = _mainViewModel.Cars.Where(x => x.Car.TraversalDirection == traversalDirection).ToList();

        // get last car in _mainViewModel.Cars
        var lastCar = carsWithSameTraversalDirection.LastOrDefault();
        int localId = lastCar is not null ? lastCar.Car.Id + 1 : 0;

        _mainWindow.Dispatcher.Invoke(() =>
        {
            var car = new Car(localId, startPoint, 2, 0, Brushes.HotPink, traversalDirection);
            var carInstance = new CarData(car, _mainWindow, _mainViewModel, this);
            _mainViewModel.Cars.Add(carInstance);
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
        try
        {
            Point startPointTop = new Point(1033, -150);
            Point startPointBottom = new Point(1033, 790);

            double fromTop = -2.21;
            double fromBottom = 2.22;

            // generate random number 0 or 1
            var random = new Random().Next(0, 2);
            var direction = random == 0 ? fromTop : fromBottom;
            var startPoint = random == 0 ? startPointTop : startPointBottom;
            var traversalDirection =
                random == 0 ? TraversalDirection.FromTopToBottom : TraversalDirection.FromBottomToTop;

            _mainWindow.Dispatcher.Invoke(() =>
            {
                var train = new Train(0, startPoint, 1, direction, Brushes.HotPink, traversalDirection);
                var trainInstance = new TrainData(train, _mainViewModel);
                _mainViewModel.TrainData = trainInstance;
            });
        }
        catch (TaskCanceledException)
        {

        }
    }

    private void CreateThreadForInstance<T>(T instance, Action<T> move)
    {
        Thread t = new Thread(() =>
        {
            if (instance != null) move(instance);
        });
        t.Start();
    }

    private void MoveTrain(TrainData? trainInstance)
    {
        try
        {
            if (trainInstance == null) throw new ArgumentNullException(nameof(trainInstance));
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

                if (distanceFromTopBorder > 800 && trainInstance.Train.TraversalDirection == TraversalDirection.FromTopToBottom)
                {
                    break;
                }

                if (distanceFromTopBorder < -200 &&
                    trainInstance.Train.TraversalDirection == TraversalDirection.FromBottomToTop)
                {
                    break;
                }

                Thread.Sleep(3);
            }

            IsTrainActive = false;

            _mainViewModel.TrainActiveMessage = "Pociąg jest nieaktywny";
        }
        catch (TaskCanceledException)
        {

        }
    }
}