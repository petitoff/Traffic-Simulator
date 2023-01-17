using Traffic_Simulator.Model;
using Traffic_Simulator.ViewModel;

namespace Traffic_Simulator.Simulation
{
    public class CarInstance
    {
        public Car Car;
        private readonly MainViewModel _mainViewModel;

        public CarInstance(Car car, MainViewModel mainViewModel)
        {
            Car = car;
            _mainViewModel = mainViewModel;
        }
    }
}
