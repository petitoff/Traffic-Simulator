using System;
using System.ComponentModel;
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
    private MainViewModel _mainViewModel;
    public MainWindow()
    {
        InitializeComponent();

        _mainViewModel = new MainViewModel(this);
        DataContext = _mainViewModel;

        this.Closing += OnClosing;
    }


    private void OnClosing(object sender, CancelEventArgs e)
    {
        _mainViewModel.AbortMainThread();
    }
}