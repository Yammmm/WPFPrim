using System;
using System.Collections.Generic;
using System.Text;

namespace WmsPrism.Model.Dto
{
    public class OutBillDto
    {       
        public int Bill_id { get; set; }
        public string Bill_no { get; set; }
        public int Total_num { get; set; }
        public int Name { get; set; }

        public List<BillPositionDto> PositionList { get; set; }

        public string PostionInfoStr { get; set; }
    }
}
