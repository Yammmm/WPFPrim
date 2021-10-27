using System;
using System.Collections.Generic;
using System.Text;

namespace WmsPrism.Model.Dto
{
    public class BarCodeCheckDto
    {
        public int Bill_id { get; set; }
        public string Bill_no { get; set; }
        public string CustomerName { get; set; }
        public string To_country { get; set; }
        public int In_time { get; set; }
        public string Plate_no { get; set; }
        public int In_status { get; set; }
        public string Weight { get; set; }
        public List<BillPositionDto> PositionList { get; set; }

        //WMS_bill_barcodes
        public string BarCode { get; set; }

        //扩展
        public string In_timeStr { get; set; }

        public string In_statusStr { get; set; }

        public string PostionInfoStr { get; set; }

    }
}
