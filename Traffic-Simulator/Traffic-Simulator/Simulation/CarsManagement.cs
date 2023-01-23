﻿using System;
using System.Linq;
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
                //var localNumberOfCars = int.Parse(_mainViewModel.NumberOfCars);

                //if (localNumberOfCars == 0)
                //{
                //    continue;
                //}

                //if (localNumberOfCars != _numberOfCars)
                //{
                //    // remove all cars
                //    _mainViewModel.Cars.Clear();

                //    // stop all thread from CarsThreads
                //    foreach (var carThread in _mainViewModel.CarsThreads)
                //    {
                //        carThread.Abort();
                //    }

                //    _numberOfCars = localNumberOfCars;
                //}


                int localNumberOfCars = int.Parse(_mainViewModel.NumberOfCars);
                if (_mainViewModel.Cars.Count < localNumberOfCars)
                {
                    for (int i = 0; i < localNumberOfCars; i++)
                    {
                        // random number, 0 or 1
                        var random = new Random().Next(0, 2);

                        // if random number == 0 create new card from top to bottom
                        // else create new card from bottom to top
                        CreateNewCar(random == 0
                            ? TraversalDirection.FromTopToBottom
                            : TraversalDirection.FromBottomToTop);

                        // for dev
                        //CreateNewCar(TraversalDirection.FromBottomToTop);


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
            CreateThreadForTrain(_mainViewModel.TrainData);

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
            _mainViewModel.NumberOfCars =  _mainViewModel.Cars.Count.ToString();
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
                if (trainInstance == null)
                {
                    return;
                }

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