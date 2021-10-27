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
    /// BillSearch.xaml 的交互逻辑
    /// </summary>
    public partial class BillSearch : UserControl
    {
        public BillSearch()
        {
            InitializeComponent();
        }

        //回车后实去焦点获取值
        private void SearchTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchTextBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }
    }
}
