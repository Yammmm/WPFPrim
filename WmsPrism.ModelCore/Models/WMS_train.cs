using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace WmsPrism.Model.Models
{
    public class WMS_train
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Train_id { get; set; }
        public string Train_no { get; set; }
        public int Line_id { get; set; }
        public int Transfer_date { get; set; }
        public int Carriage_num { get; set; }
        public int Status { get; set; }

    }
}
