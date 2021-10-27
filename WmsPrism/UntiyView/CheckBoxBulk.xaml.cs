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
using WmsPrism.Model.Models;

namespace WmsPrism.UntiyView
{
    /// <summary>
    /// CheckBoxBulk.xaml 的交互逻辑
    /// </summary>
    public partial class CheckBoxBulk : UserControl
    {
        private string _title { get; set; }
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
            }
        }

        private int _positionid { get; set; }
        public int Positionid
        {
            get { return _positionid; }
            set
            {
                _positionid = value;
            }
        }

        public bool IsFullStore { get; set; } = false;

        public List<WMS_position> positions { get; set; }
        public CheckBoxBulk()
        {
            InitializeComponent();
            this.Loaded +=  CheckBoxBulk_Loaded;
        }

        private void CheckBoxBulk_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public void AddCheckBoxBulk(string content, string tag, int status)
        {
            CheckBox cb = new CheckBox();
            cb.Width = 80;
            cb.Height = 35;
            cb.VerticalAlignment = VerticalAlignment.Top;
            cb.HorizontalAlignment = HorizontalAlignment.Left;
            cb.Margin = new Thickness(5, 5, 5, 5);
            //cb.GroupName = "BulkCheckBox";
            cb.Content = content;

            string pid = Positionid.ToString();
            cb.Tag = tag;

            if (status == 1)
            {

                cb.Background = Brushes.Orange;
                cb.IsEnabled = false;
            }

            RadioBulkWarapPanel.Children.Add(cb);
        }



    }
}
