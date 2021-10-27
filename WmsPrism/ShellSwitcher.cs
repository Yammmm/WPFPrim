using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using WmsPrism.ViewModels;
using WmsPrism.Views;

namespace WmsPrism
{
    //Window 打开Window
    public static class ShellSwitcher
    {
        public static void Switch<TClosed, TShow>() where TClosed : Window where TShow : Window, new()
        {
            //Show<TShow>();
            //Closed<TClosed>();
            LoginOpenMain<TClosed>();

        }

        public static void Show<T>(T window = null) where T : Window, new()
        {
            var shell = Application.Current.MainWindow = window ?? new T();
            shell?.Show();
        }

        public static void Closed<T>() where T : Window
        {
            var shell = Application.Current.Windows.OfType<Window>().FirstOrDefault(window => window is T);
            shell?.Close();
        }

        public static void LoginOpenMain<T>() where T : Window
        {
            var shell = Application.Current.Windows.OfType<Window>().FirstOrDefault(window => window is T);
            
            shell.DialogResult = true;
        }

        public static void CloseLogin<T>() where T : Window
        {
            var shell = Application.Current.Windows.OfType<Window>().FirstOrDefault(window => window is T);
            shell.DialogResult = false;
        }


    }
}
