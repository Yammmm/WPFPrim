using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace WmsPrism.Model.Models
{
    public class WMS_customer
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Customer_id { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
    }
}
