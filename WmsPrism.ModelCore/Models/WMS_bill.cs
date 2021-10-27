using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace WmsPrism.Model.Models
{
     public class WMS_bill
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Bill_id { get; set; }
        public int Customer_id { get; set; }
        public string Bill_no { get; set; }
        public string Plate_no { get; set; }
        public string Total_weight { get; set; }
        public int Total_num { get; set; }
        public int Date { get; set; }
        public string To_country { get; set; }

        //public int Train_id { get; set; }

        public int Arrive_time { get; set; }

        //20210330新增字段
        public int Actual_num { get; set; }
        public string Actual_weight { get; set; }
    }
}
