using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Traffic_Simulator.Command;
using Traffic_Simulator.Const;
using Traffic_Simulator.Model;
using Traffic_Simulator.Simulation;

namespace Traffic_Simulator.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        public bool IsAnimationActive;
        private string _bgImage = @"C:\Users\petit\Desktop\repos\Traffic-Simulator\Traffic-Simulator\Traffic-Simulator\Assets\Image\mapa_v3.png";
        private Thread _mainThread;
        private Thread _trainThread;

        private string _trainActiveMessage;
        private string _numbersOfCars;
        private TrainData? _trainData;

        public MainViewModel(MainWindow mainWindow)
        {
            StartAnimationCommand = new DelegateCommand(StartAnimation);
            StartTrainCommand = new DelegateCommand(StartTrain);

            CarsManagement = new CarsManagement(this, mainWindow);
            Cars = new ObservableCollection<CarData>();
            CarsThreads = new ObservableCollection<Thread>();
            IsAnimationActive = true;
        }

        public DelegateCommand StartAnimationCommand { get; }
        public DelegateCommand StartTrainCommand { get; }
        public CarsManagement CarsManagement { get; }

        public TrainData? TrainData
        {
            get => _trainData;
            set
            {
                _trainData = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<CarData> Cars { get; set; }
        public ObservableCollection<Thread> CarsThreads { get; set; }

        public string TrainActiveMessage
        {
            get => _trainActiveMessage;
            set
            {
                _trainActiveMessage = value;
                OnPropertyChanged();
            }
        }

        public string BgImage
        {
            get => _bgImage;
            set
            {
                _bgImage = value;
                OnPropertyChanged();
            }
        }

        public string NumberOfCars
        {
            get => _numbersOfCars;
            set
            {
                _numbersOfCars = value;
                OnPropertyChanged();
            }
        }

        public void AbortMainThread()
        {
            // TODO: Making threads stop correctly
            //_mainThread.Abort();

            IsAnimationActive = false;
        }

        private void StartAnimation(object obj)
        {
            _mainThread = new Thread(CarsManagement.StartAnimation);
            _mainThread.Start();

            _trainThread = new Thread(CarsManagement.StartTrain);
            _trainThread.Start();
        }


        private void StartTrain(object obj)
        {
            _trainThread = new Thread(CarsManagement.StartTrain);
            _trainThread.Start();
        }
    }
}
