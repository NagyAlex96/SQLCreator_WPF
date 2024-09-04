using System.Windows;
using System.Windows.Input;

namespace SQLCreator
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.WindowState =
                (this.WindowState == WindowState.Normal ?
                WindowState.Maximized :
                WindowState.Normal);
        }
    }
}