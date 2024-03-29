﻿using Traffic_Simulator.Model;
using Traffic_Simulator.ViewModel;

namespace Traffic_Simulator.Simulation
{
    public class TrainData
    {
        public readonly Train Train;
        private readonly MainViewModel _mainViewModel;

        public TrainData(Train train, MainViewModel mainViewModel)
        {
            Train = train;
            _mainViewModel = mainViewModel;
        }
    }
}
