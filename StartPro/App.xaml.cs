﻿using System;
using System.Windows;

namespace StartPro;

public partial class App : Application
{
    public static class Program
    {
        [STAThread]
        public static void Main( )
        {
            App app = new( );
            app.InitializeComponent( );
            app.Run( );
        }
    }
}
