using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace StartPro
{
    public enum ClipType
    {
        Small, Medium, Wide, Large
    }
    public partial class AppClip : UserControl
    {
        private ProcessStartInfo exec = new ProcessStartInfo(Default.AppName);

        public AppClip( )
        {
            InitializeComponent( );
            border.DataContext = this;
        }

        public static void AppPathChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            (o as AppClip).exec = new ProcessStartInfo(e.NewValue as string);
        }

        private void Execute(object sender, MouseButtonEventArgs e)
        {
            Process.Start(exec);
        }
    }
}
