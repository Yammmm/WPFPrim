using System;
using System.Collections.Generic;
using System.Text;

namespace WmsPrism.Model.Dto
{
    public class BillBarCodesDto
    {
        public int Batch_id { get; set; }
        public int Bill_id { get; set; }
        public int Num { get; set; }
        public int BarCode_Type { get; set; }
        public int Time { get; set; }
        public int User_id { get; set; }
        public int Status { get; set; }


        public int Barcode_id { get; set; }
        public string BarCode { get; set; }
        public string Pdf_file_name { get; set; }
        public string Waybill_no { get; set; }
        public string Weight { get; set; }

        //扩展
        public string TimeStr { get; set; }

        public string Bill_no { get; set; }

    }
}
