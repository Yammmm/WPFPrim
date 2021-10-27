using System;
using System.Collections.Generic;
using System.Text;
using WmsPrism.Model.Models;

namespace WmsPrism.Model.Dto
{
    public class BillCheckDto
    {
        public int Bill_id { get; set; }
        public string Bill_no { get; set; }
        public string CustomerName { get; set; }
        public string To_country { get; set; }
        public int In_time { get; set; }
        public string Plate_no { get; set; }
        public int In_status { get; set; }
        public string Total_weight { get; set; }
        public int Total_num { get; set; }


        public List<BillPositionDto> PositionList { get; set; }

        //WMS_bill_barcode_batch 表
        public int Num { get; set; }
        //扩展
        public string In_timeStr { get; set; }

        public string In_statusStr { get; set; }

        public string PostionInfoStr { get; set; }

        public string Total_numStr { get; set; }

        public string NumStr { get; set; }
    }
}
