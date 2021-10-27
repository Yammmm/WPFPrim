using System;
using System.Collections.Generic;
using System.Text;

namespace WmsPrism.Model.Dto
{
    public class BillOfLadingDetailsDto
    {
        public int Bill_id { get; set; }
        /// <summary>
        /// 标签码
        /// </summary>
        public string BarCode { get; set; }

        /// <summary>
        /// 面单号
        /// </summary>
        public string Waybill_no { get; set; }
        
        /// <summary>
        /// 重量
        /// </summary>
        public string Weight { get; set; }


        public int In_time { get; set; }
        /// <summary>
        /// 进仓时间
        /// </summary>
        public string IntimeData { get; set; }


        /// <summary>
        /// 标签数量
        /// </summary>
        public int Num { get; set; }
    }
}
