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

namespace WmsPrism.Views.BillCheck
{
    /// <summary>
    /// BarCodeDialog.xaml 的交互逻辑
    /// </summary>
    public partial class BarCodeDialog : UserControl
    {
        public BarCodeDialog()
        {
            InitializeComponent();
        }

        private void SearchBarCodeTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchBarCodeTextBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }
    }
}
