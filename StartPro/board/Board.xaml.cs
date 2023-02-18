using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace StartPro
{
    public partial class Board : UserControl
    {
        private ProcessStartInfo exec = new ProcessStartInfo(Default.AppName);

        public Board( )
        {
            InitializeComponent( );
            border.DataContext = this;
        }

        public static void AppPathChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            (o as Board).exec = new ProcessStartInfo(e.NewValue as string);
        }

        private void Execute(object o, MouseButtonEventArgs e)
        {
            if (!IsDrag)
                Process.Start(exec);
        }
    }
}
