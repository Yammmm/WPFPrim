using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace WmsPrism.Model.Models
{
    public class WMS_waybill
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Waybill_id { get; set; }
        public int Bill_id { get; set; }
        public string Waybill_no { get; set; }
        public string Pack_no { get; set; }
        public int Recipient { get; set; }
        public string Weight { get; set; }
        public string Actual_weight { get; set; }
        public int Ware_status { get; set; }
        public int Arrive_status { get; set; }
        public string Goods_info { get; set; }
    }
}
