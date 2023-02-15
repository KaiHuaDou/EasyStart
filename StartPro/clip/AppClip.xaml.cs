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
        public static void ClipSizeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            AppClip control = (AppClip) o;
            switch ((ClipType) e.NewValue)
            {
                case ClipType.Small:
                    control.Width = control.Height = Default.SmallSize;
                    control.label.Visibility = Visibility.Collapsed;
                    control.image.Margin = new Thickness(3);
                    break;
                case ClipType.Medium:
                    control.Width = control.Height = Default.MediumSize;
                    control.label.Visibility = Visibility.Visible;
                    control.image.Margin = new Thickness(8, 8, 8, 5);
                    break;
                case ClipType.Wide:
                    control.Width = Default.WideSize;
                    control.Height = (int) (0.5 * control.Width);
                    control.label.Visibility = Visibility.Visible;
                    control.image.Margin = new Thickness(8, 8, 8, 5);
                    break;
                case ClipType.Large:
                    control.Width = control.Height = Default.LargeSize;
                    control.label.Visibility = Visibility.Visible;
                    control.image.Margin = new Thickness(10, 10, 10, 5);
                    break;
            }
        }

        private void Execute(object sender, MouseButtonEventArgs e)
        {
            Process.Start(exec);
        }
    }
}
