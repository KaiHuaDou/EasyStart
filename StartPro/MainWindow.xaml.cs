using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StartPro
{
    public partial class MainWindow : Window
    {
        public MainWindow( )
        {
            InitializeComponent( );
        }

        private void GoDragMove(object sender, MouseButtonEventArgs e)
        {
            //this.DragMove( );
        }

        private void ChangeClipType(object sender, SelectionChangedEventArgs e)
        {
            appClip.ClipSize = (ClipType) (clipTypeCombo.SelectedIndex);
        }
    }
}
