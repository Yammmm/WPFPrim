using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace WmsPrism.Model.Models
{
    public class WMS_barcode_waybill
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int History_id { get; set; }
        public int Bill_id { get; set; }
        public string BarCode { get; set; }
        public string WayBill_no { get; set; }
        public int Time { get; set; }
        public int User_id { get; set; }
        public int Status { get; set; }
    }
}
