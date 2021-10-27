using AutoMapper;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WmsPrism.IServices;
using WmsPrism.Model.Dto;
using WmsPrism.Model.Models;
using WmsPrism.Repository.Base;
using WmsPrism.Services.Base;

namespace WmsPrism.Services
{
    public class UserServices : BaseServices<WMS_user>, IUserServices
    {
        public UserServices() 
        {
        
        }
       
        public async Task<WMS_user> GetUser(int userid)
        {            
      
            var user = await base.BaseDal.GetByIdAsync(userid);
            return user;
        }

        public async Task<UserDto> UserSign(string signname, string password)
        {
            UserDto userDto = await Task.Run(() =>
            {
                return base.BaseDal.dbBase.Queryable<WMS_user, WMS_user_role, WMS_role>((wmsuser, userrole, role) => new JoinQueryInfos(
                        JoinType.Inner, userrole.User_id == wmsuser.User_id,
                        JoinType.Inner, role.Role_id == userrole.Role_id))
                         .Where((wmsuser, userrole, role) => wmsuser.UserName == signname && wmsuser.PassWord == password && wmsuser.Status == 1)
                         .Select((wmsuser, userrole, role) => new UserDto
                         {
                             User_id = wmsuser.User_id,
                             UserName = wmsuser.UserName,
                             PassWord = wmsuser.PassWord,
                             Status = wmsuser.Status,
                             Role_id = userrole.Role_id,
                             Menu_ids = role.Menu_ids,
                         }).FirstAsync();

            });
            return userDto;
        }

        public async Task<List<WMS_menu>> GetUserMenu(string Menu_ids)
        {
            if (string.IsNullOrEmpty(Menu_ids)) { return null; }
            string[] menuIdsStr = Menu_ids.Split(',');
            int[] menuArr = Array.ConvertAll(menuIdsStr, int.Parse);

            List<WMS_menu> menus = await Task.Run(() =>
            {
                return base.BaseDal.dbBase.Context.Queryable<WMS_menu>().Where(m => menuArr.Contains(m.Menu_id)).ToListAsync();
            });

            return menus;
        }
    }
}
