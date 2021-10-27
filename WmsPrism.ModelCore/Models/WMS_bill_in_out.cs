using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace WmsPrism.Model.Models
{
    public class WMS_bill_in_out
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int History_id { get; set; }
        public int Bill_id { get; set; }
        public int Position_id { get; set; }
        public int In_time { get; set; }
        public int In_user_id { get; set; }
        public int Out_time { get; set; }
        public int Out_user_id { get; set; }

        public int In_status { get; set; }
    }
}
