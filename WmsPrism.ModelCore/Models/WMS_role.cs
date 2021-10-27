using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace WmsPrism.Model.Models
{
    public  class WMS_role
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Role_id { get; set; }
        public string Menu_ids { get; set; }
    }
}
