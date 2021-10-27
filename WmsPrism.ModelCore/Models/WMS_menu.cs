using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace WmsPrism.Model.Models
{
   public partial class WMS_menu
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Menu_id { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }

        public string Module { get; set; }
    }
}
