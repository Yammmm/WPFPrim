using System;
using System.Collections.Generic;
using System.Text;

namespace WmsPrism.Model.Dto
{
   public  class BillInOutDto
    {
        public int History_id { get; set; }
        public int Bill_id { get; set; }
        public int Position_id { get; set; }
        public int In_time { get; set; }
        public int In_user_id { get; set; }
        public int Out_time { get; set; }
        public int Out_user_id { get; set; }
        public string Title { get; set; }

        //扩展
        public string Bill_no { get; set; }
        public string BillDateTime { get; set; }
        public string Total_num { get; set; }

        public string inStoreCount { get; set; }
        //客户名称
        public string Name { get; set; }

        public string inTimeDateTime { get; set; }
        
        //所有所在仓库
        public string PostionIdArr { get; set; }
        public string PostionInfoStr { get; set; }

    }
}
