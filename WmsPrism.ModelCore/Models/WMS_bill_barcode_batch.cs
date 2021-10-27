using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace WmsPrism.Model.Models
{
    //提单标签表
    public class WMS_bill_barcode_batch
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Batch_id { get; set; }
        public int Bill_id { get; set; }
        public int Num { get; set; }
        public int BarCode_Type { get; set; }
        public int Time { get; set; }
        public int User_id { get; set; }
        public int Status { get; set; }
    }
}
