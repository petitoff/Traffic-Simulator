using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Traffic_Simulator.Const;
using Traffic_Simulator.Model;
using Traffic_Simulator.ViewModel;

namespace Traffic_Simulator.Simulation;

public class CarsManagement
{
    private readonly MainViewModel _mainViewModel;
    private readonly MainWindow _mainWindow;

    public CarsManagement(MainViewModel mainViewModel, MainWindow mainWindow)
    {
        _mainViewModel = mainViewModel;
        _mainWindow = mainWindow;
    }

    public void StartAnimation()
    {
        CreateCar(TraversalDirection.FromTopToBottom);

        MoveCar(_mainViewModel.Cars[0]);
    }

    public void CreateCar(TraversalDirection traversalDirection)
    {
        var topStartPoint = new Point(-20, 259);
        var bottomStartPoint = new Point(1220, 538);
        Point startPoint = traversalDirection == TraversalDirection.FromTopToBottom ? topStartPoint : bottomStartPoint;

        int localId = _mainViewModel.Cars.Count + 1;

        Car car = null;

        _mainWindow.Dispatcher.Invoke(() =>
        {
            car = new Car(localId, startPoint, 0.1, 0, Brushes.HotPink, traversalDirection);
            var carInstance = new CarInstance(car, _mainViewModel);
            _mainViewModel.Cars.Add(carInstance);

        });
    }

    //public void CreateCar(TraversalDirection traversalDirection)
    //{
    //    var topStartPoint = new Point(-20, 218);
    //    var bottomStartPoint = new Point(1220, 538);
    //    Point startPoint = traversalDirection == TraversalDirection.FromTopToBottom ? topStartPoint : bottomStartPoint;

    //    int localId = _mainViewModel.Cars.Count + 1;

    //    var car = new Car(id: localId, startPoint, 5, 0, Brushes.HotPink, traversalDirection);

    //    _mainViewModel.Cars.Add(car);
    //}

    public void MoveCar(CarInstance carInstance)
    {
        Thread t = carInstance.Car.TraversalDirection == TraversalDirection.FromTopToBottom
            ? new Thread(() => MoveCarFromTop(carInstance))
            : new Thread(() => MoveCarFromBottom(carInstance));

        t.Start();
    }

    private void MoveCarFromTop(CarInstance carInstance)
    {
        while (carInstance.Car.Position.X <= 800)
        {
            _mainWindow.Dispatcher.Invoke(() =>
            {
                carInstance.Car.Position = new Point(carInstance.Car.Position.X + 1, carInstance.Car.Position.Y);
                carInstance.Car.UpdateShape(_mainWindow.MainCanvas);
                carInstance.Car.UpdatePosition();


            });

            Thread.Sleep(1);
        }

        while (true)
        {
            double distanceFromLeftBorder = 0;
            _mainWindow.Dispatcher.Invoke(() =>
            {
                carInstance.Car.Direction = 4;
                carInstance.Car.Position = new Point(carInstance.Car.Position.X + 1, carInstance.Car.Position.Y + 1);

                carInstance.Car.UpdateShape(_mainWindow.MainCanvas);
                carInstance.Car.UpdatePosition();

                distanceFromLeftBorder = Canvas.GetLeft(carInstance.Car.Shape);

            });

            if (distanceFromLeftBorder < 100f)
            {
                break;
            }

            Thread.Sleep(1);
        }
    }

    private void MoveCarFromBottom(CarInstance carInstance)
    {
        throw new System.NotImplementedException();
    }
}