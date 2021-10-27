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
    /// <summary>
    /// BingWeight.xaml 的交互逻辑
    /// </summary>
    public partial class BingWeight : UserControl
    {
        public BingWeight()
        {
            InitializeComponent();
            Loaded += BingWeight_Loaded;

        }

        private void BingWeight_Loaded(object sender, RoutedEventArgs e)
        {
            WeighTxt.Focus();

            IRequestFocus focus = (IRequestFocus)DataContext;
            focus.FocusRequested += Focus_FocusRequested;
        }

        private void Focus_FocusRequested(object sender, FocusRequestedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Weigh":
                    WeighTxt.Focus();
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

        private void BarCodeTxt_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BarCodeTxt.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }
    }
}
