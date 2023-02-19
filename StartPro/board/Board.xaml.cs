using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
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
            Board board = o as Board;
            board.exec = new ProcessStartInfo(e.NewValue as string);
            board.AppName = new FileInfo(e.NewValue as string).Name;
            board.AppIcon = StdApi.GetIcon(e.NewValue as string);

        }

        private void Execute(object o, MouseButtonEventArgs e)
        {
            if (!IsDrag && IsEnabled)
                Process.Start(exec);
        }
    }
}
