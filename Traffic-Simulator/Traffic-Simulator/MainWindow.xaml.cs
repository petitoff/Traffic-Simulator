using System.Windows;
using Traffic_Simulator.ViewModel;

namespace Traffic_Simulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var viewModel = new MainViewModel(this);
            DataContext = viewModel;
        }
    }
}
