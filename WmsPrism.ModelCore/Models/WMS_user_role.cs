using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace WmsPrism.Model.Models
{
    public class WMS_user_role
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public int User_id { get; set; }
        public int Role_id { get; set; }
    }
}
