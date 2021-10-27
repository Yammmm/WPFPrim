using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace WmsPrism.Model.Dto
{
    public class BillBarCodesDtoBindPrism: BindableBase
    {
        public int Batch_id { get; set; }
        public int Bill_id { get; set; }
        public int Num { get; set; }
        public int BarCode_Type { get; set; }
        public int Time { get; set; }
        public int User_id { get; set; }
        public int Status { get; set; }


        public int Barcode_id { get; set; }
        public string BarCode { get; set; }
        public string Pdf_file_name { get; set; }
        public string Waybill_no { get; set; }
        public string Weight { get; set; }

        //扩展
        public string TimeStr { get; set; }

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool _isSelected = false;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }
        /// <summary>
        /// 是否全选
        /// </summary>
        /// 
        private bool _isSelectAll = false;

        public bool IsSelectAll
        {
            get { return _isSelectAll; }
            set { SetProperty(ref _isSelectAll, value); }
        }
    }
}
