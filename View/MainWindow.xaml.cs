using System.Windows;
using System.Windows.Input;
using TaskList.ViewModel;

namespace TaskList
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                viewModel.OnMainWIndowClosing(e);
            }
        }

        private void GrabOrMaximizeWindow(object sender, MouseButtonEventArgs e)
        {
            // Maximize the window on double-click.
            if (e.ClickCount >= 2)
                MaximizeWindow(sender, e);
            // Drag The window.
            else
            {
                /* Problem: Can't drag the window when maximized.
                 * Solution: Un-maximize!
                 * 
                 * Problem: Un-maximizing teleports the window to its original location, hindering dragging.
                 * Solution: Move the window to the cursor (intersect with cursor at the middle-top of the window).
                 */
                if (WindowState == WindowState.Maximized)
                {
                    WindowState = WindowState.Normal;
                    Point p = e.GetPosition(null);
                    Top = p.Y - 50; // Adjust the top position with a 50px wllowing the user to choose between maximize or dock to window.
                    Left = p.X - Width / 2;
                }

                DragMove();
            }
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MaximizeWindow(object sender, RoutedEventArgs e)
        {
            WindowState = (WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
        }

        private void MinimizeWindow(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}