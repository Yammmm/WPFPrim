using System;
using System.Collections.Generic;
using System.Text;

namespace WmsPrism.Model.Dto
{
    public class WayBillDto
    {
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
    
        public string Bill_no { get; set; }


        //扩展
        public string Bill_idStr { get; set; }
        public string RecipientStr { get; set; }
        public string Ware_statusStr { get; set; }

        public string Arrive_statusStr { get; set; }

        public string PositionTitle { get; set; }
    }
}
