using System;
using System.Collections.Generic;
using System.Text;

namespace WmsPrism.Model.Dto
{
    public class UserDto
    {
        public int User_id { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public int Status { get; set; }

        public int Role_id { get; set; }

        public string Menu_ids { get; set; }
    }
}
