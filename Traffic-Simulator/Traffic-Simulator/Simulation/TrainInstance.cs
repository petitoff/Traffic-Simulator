using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traffic_Simulator.Model;
using Traffic_Simulator.ViewModel;

namespace Traffic_Simulator.Simulation
{
    public class TrainInstance
    {
        public readonly Train Train;
        private readonly MainViewModel _mainViewModel;

        public TrainInstance(Train train, MainViewModel mainViewModel)
        {
            Train = train;
            _mainViewModel = mainViewModel;
        }
    }
}
