
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WmsPrism.Model.Models;
using WmsPrism.Model.Dto;

namespace WmsPrism.Extensions.AutoMapper
{
    public class CustomProfile : Profile
    {
        public CustomProfile() 
        {
            CreateMap<WMS_user, UserDto>();
            CreateMap<UserDto, WMS_user>();

            CreateMap<WMS_bill, BillDto>();
            CreateMap<BillDto, WMS_bill>();
        }
    }
}
