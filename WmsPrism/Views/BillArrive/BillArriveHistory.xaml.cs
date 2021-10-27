using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WmsPrism.ViewModels.BillArrive;

namespace WmsPrism.Views.BillArrive
{
    /// <summary>
    /// BillArriveHistory.xaml 的交互逻辑
    /// </summary>
    public partial class BillArriveHistory : UserControl
    {
        public BillArriveHistory()
        {
            InitializeComponent();

           
        }


        public void CombinedDialogClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (Equals(eventArgs.Parameter, "1") &&
                CombinedCalendar.SelectedDate is DateTime selectedDate)
            {
                var combined = selectedDate.AddSeconds(CombinedClock.Time.TimeOfDay.TotalSeconds);
                ((BillArriveHistoryViewModel)DataContext).Time = combined;
                ((BillArriveHistoryViewModel)DataContext).Date = combined.ToString();
            }
        }
    }
}
