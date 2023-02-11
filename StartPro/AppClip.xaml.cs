using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace StartPro
{
    public enum ClipType
    {
        Small, Medium, Wide, Large
    }
    public partial class AppClip : UserControl
    {
        private ProcessStartInfo exec = new ProcessStartInfo(Setting.ProcessName);

        public static readonly DependencyProperty AppNameProperty;
        public static readonly DependencyProperty AppIconProperty;
        public static readonly DependencyProperty AppPathProperty;
        public static readonly DependencyProperty ClipSizeProperty;
        public static readonly DependencyProperty ClipRadiusProperty;


        public string AppName
        {
            get { return (string) GetValue(AppNameProperty); }
            set { SetValue(AppNameProperty, value); }
        }
        public ImageSource AppIcon
        {
            get { return (ImageSource) GetValue(AppIconProperty); }
            set { SetValue(AppIconProperty, value); }
        }
        public string AppPath
        {
            get { return (string) GetValue(AppPathProperty); }
            set { SetValue(AppPathProperty, value); }
        }
        public ClipType ClipSize
        {
            get { return (ClipType) GetValue(ClipSizeProperty); }
            set { SetValue(ClipSizeProperty, value); }
        }
        public CornerRadius ClipRadius
        {
            get { return (CornerRadius) GetValue(ClipRadiusProperty); }
            set { SetValue(ClipRadiusProperty, value); }
        }

        public AppClip( )
        {
            InitializeComponent( );
            border.DataContext = this;
        }

        static AppClip( )
        {
            Type thisType = typeof(AppClip);
            PropertyMetadata appNameMetadata = new PropertyMetadata("Application");
            PropertyMetadata appIconMetadata = new PropertyMetadata(SysIcon.Get(Setting.ProcessName));
            PropertyMetadata appPathMetadata = new PropertyMetadata(Setting.ProcessName, AppPathChanged);
            PropertyMetadata clipSizeMetaData = new PropertyMetadata(ClipSizeChanged);
            PropertyMetadata clipRadiusMetaData = new PropertyMetadata(Setting.Radius);
            AppNameProperty = DependencyProperty.Register("AppName", typeof(string), thisType, appNameMetadata);
            AppIconProperty = DependencyProperty.Register("SysIcon", typeof(ImageSource), thisType, appIconMetadata);
            AppPathProperty = DependencyProperty.Register("AppPath", typeof(string), thisType, appPathMetadata);
            ClipSizeProperty = DependencyProperty.Register("ClipSize", typeof(ClipType), thisType, clipSizeMetaData);
            ClipRadiusProperty = DependencyProperty.Register("ClipRadius", typeof(CornerRadius), thisType, clipRadiusMetaData);
        }

        public static void AppPathChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            (o as AppClip).exec = new ProcessStartInfo(e.NewValue as string);
        }
        public static void ClipSizeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            AppClip control = (AppClip)o;
            switch ((ClipType) e.NewValue)
            {
                case ClipType.Small: control.Width = control.Height = Setting.SmallSize; break;
                case ClipType.Medium:
                    control.Width = control.Height = Setting.MediumSize;
                    break;
                case ClipType.Wide:
                    control.Width = Setting.WideSize;
                    control.Height = (int) (0.5 * control.Width);
                    break;
                case ClipType.Large:
                    control.Width = control.Height = Setting.LargeSize;
                    break;
            }
        }

        private void Execute(object sender, MouseButtonEventArgs e)
        {
            Process.Start(exec);
        }
    }
}
