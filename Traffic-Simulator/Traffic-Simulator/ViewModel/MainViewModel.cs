using Traffic_Simulator.Command;

namespace Traffic_Simulator.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private readonly MainWindow _mainWindow;
        private string _bgImage = @"C:\Users\petit\Desktop\repos\Traffic-Simulator\Traffic-Simulator\Traffic-Simulator\Assets\Image\mapa_v3.png";

        public MainViewModel(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            StartAnimationCommand = new DelegateCommand(StartAnimation);
        }

        private void StartAnimation(object obj)
        {
            
        }

        public DelegateCommand StartAnimationCommand { get; }
        
        public string BgImage
        {
            get => _bgImage;
            set
            {
                _bgImage = value;
                OnPropertyChanged();
            }
        }

    }
}
