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
using WmsPrism.Unit;

namespace WmsPrism.Views.BingCargo
{
    //这里不做逻辑 只坐简单焦点操作(扫描枪)
    /// <summary>
    /// BingWayBillNo.xaml 的交互逻辑
    /// </summary>
    public partial class BingWayBillNo : UserControl
    {
        public BingWayBillNo()
        {
            InitializeComponent();
           
            Loaded += BingWayBillNo_Loaded;
        }

        private void BingWayBillNo_Loaded(object sender, RoutedEventArgs e)
        {
            BarCodeTxt.Focus();
            IRequestFocus focus = (IRequestFocus)DataContext;
            focus.FocusRequested += Focus_FocusRequested;
        }

        private void Focus_FocusRequested(object sender, FocusRequestedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "WayBillNo":
                    WayBillTxt.Focus();
                    break;
                case "BarCode":
                    BarCodeTxt.Focus();
                    break;
                case "RichText":
                    this.richTextBox.ScrollToEnd();
                    break;
                default:
                    break;
            }
        }

        private void WayBillTxt_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //BarCodeTxt.Focus();
                WayBillTxt.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));

            }
        }
    }
}
