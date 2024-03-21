using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Rover.Models;
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

    /*
     * Not satisfied with this approach, does it break MVVM principles?, it's technically code-behind, 
     * how else could I easily and cleanly acheive this?
     * 
     * https://stackoverflow.com/a/1510592
     */
    private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (sender is not ListView view)
            return;
        if (view.SelectedItem is not FolderItem item)
            return;
        viewModel.FolderItemInteract.Execute(item);
    }

    private void SortableListViewColumnHeaderClicked(object sender, RoutedEventArgs e)
    {
        ((Controls.SortableListView)sender).GridViewColumnHeaderClicked(e.OriginalSource as GridViewColumnHeader);
    }
}
