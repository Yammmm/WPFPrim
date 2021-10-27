using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace WmsPrism.Model.Models
{
    public  class WMS_bill_arrive_history
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int History_id { get; set; }
        public int Bill_id { get; set; }
        public string Bill_no { get; set; }
        public string Plate_no { get; set; }
        public int Arrive_time { get; set; }
        public int Time { get; set; }
        public int User_id { get; set; }

        //20210330新增字段
        public int Actual_num { get; set; }
        public string Actual_weight { get; set; }
    }
}
