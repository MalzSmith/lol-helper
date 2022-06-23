using System.Windows;
using System.Windows.Input;
using JustAnotherLeagueHelperApp.ViewModels;

namespace JustAnotherLeagueHelperApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow(MainViewModel dataContext)
        {
            InitializeComponent();

            this.DataContext = dataContext;
        }

        private void MainWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }
        
        private void KeyTextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            KeyTextBox.Select(0, KeyTextBox.Text.Length);
        }
    }
}