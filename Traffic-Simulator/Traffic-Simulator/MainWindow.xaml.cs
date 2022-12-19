using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media;
using Traffic_Simulator.ViewModel;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Traffic_Simulator;

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