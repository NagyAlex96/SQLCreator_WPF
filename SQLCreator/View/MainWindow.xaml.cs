using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
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

        #region Mentés helyének megnyitásához
        private void TextBlock_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock tBlock)
            {
                destinationPath = tBlock.Text;
                tBlock.ContextMenu = TBlockForOutDestination();
            }
        }

        private ContextMenu TBlockForOutDestination()
        {
            ContextMenu ctxMenu = new ContextMenu();
            // "Open Folder" menüelem
            MenuItem openFolderItem = new MenuItem();
            openFolderItem.Header = "Mappa megnyitása";
            openFolderItem.Click += OpenFolder_Click; // Eseménykezelő hozzáadása
            ctxMenu.Items.Add(openFolderItem);


            return ctxMenu;
        }

        private string destinationPath;
        private void OpenFolder_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(destinationPath) && Path.Exists(destinationPath))
            {
                Process.Start("explorer.exe", destinationPath);
            }
        }

        #endregion    

    }
}