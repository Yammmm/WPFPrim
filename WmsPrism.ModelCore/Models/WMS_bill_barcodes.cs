using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace WmsPrism.Model.Models
{
    //提单标签明细表
    public class WMS_bill_barcodes
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Barcode_id { get; set; }
        public int Batch_id { get; set; }
        public string BarCode { get; set; }
        public string Pdf_file_name { get; set; }
        public string Waybill_no { get; set; }
        public string Weight { get; set; }
        public int Time { get; set; }
        public int User_id { get; set; }
    }
}
