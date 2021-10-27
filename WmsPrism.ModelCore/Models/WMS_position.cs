using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace WmsPrism.Model.Models
{

    public class WMS_position
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Position_id { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
    }
}
