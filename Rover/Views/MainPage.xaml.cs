using System.Windows.Controls;
using System.Windows.Input;
using Rover.ViewModels;

namespace Rover.Views;

public partial class MainPage : Page
{
    MainViewModel viewModel;

    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        this.viewModel = viewModel;
        DataContext = viewModel;
    }

    //public void OnKeyDownHandler(object obj, KeyEventArgs e)
    //{
    //    if (e.Key == Key.Enter)
    //        viewModel.Model.Reload(viewModel.UriPath);
    //}
}
