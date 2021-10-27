using System;
using System.Collections.Generic;
using System.Text;

namespace WmsPrism.Model.Dto
{
     public  class BillPositionDto
    {
        public int History_id { get; set; }
        public int Bill_id { get; set; }
        public int Position_id { get; set; }
        public int In_time { get; set; }
        public int In_user_id { get; set; }
        public int Out_time { get; set; }
        public int Out_user_id { get; set; }

        public int In_status { get; set; }


        //Wms_position 库位表
        public string Title { get; set; }
        public int Status { get; set; }
    }
}
