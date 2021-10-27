using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace WmsPrism.Model.Models
{
    public class WMS_user
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int User_id { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public int Status { get; set; }
    }
}
