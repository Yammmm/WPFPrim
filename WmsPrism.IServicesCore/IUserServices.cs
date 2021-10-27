using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WmsPrism.IServices.Base;
using WmsPrism.Model.Dto;
using WmsPrism.Model.Models;

namespace WmsPrism.IServices
{
    public interface IUserServices : IBaseServices<WMS_user>
    {
        Task<UserDto> UserSign(string signname, string password);
        Task<WMS_user> GetUser(int userid);

        Task<List<WMS_menu>> GetUserMenu(string Menu_ids);

    }
}
