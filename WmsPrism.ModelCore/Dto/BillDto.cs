using System;
using System.Collections.Generic;
using System.Text;

namespace WmsPrism.Model.Dto
{
    public class BillDto
    {
        public int Bill_id { get; set; }
        public int Customer_id { get; set; }
        public string Bill_no { get; set; }
        public string Plate_no { get; set; }
        public string Total_weight { get; set; }
        public int Total_num { get; set; }
        public int Date { get; set; }
        public int Wms_id { get; set; }
        public string To_country { get; set; }
        public int Train_id { get; set; }

        public int Arrive_time { get; set; }

        public string CustomerName { get; set; }

        public string Company { get; set; }

        public string Train_no { get; set; }



        //扩展
        public string Bill_idStr { get; set; }
        public string DateFormat { get; set; }
        public string Total_numStr { get; set; }
        public string Arrive_timeFormat { get; set; }

        //WMS_bill_arrive_history 表
        public int Station_id { get; set; }
        public string Actual_num { get; set; }
        public string Actual_weight { get; set; }

    }
}
