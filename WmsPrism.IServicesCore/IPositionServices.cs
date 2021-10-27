using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WmsPrism.IServices.Base;
using WmsPrism.Model;
using WmsPrism.Model.Models;

namespace WmsPrism.IServices
{
    public interface IPositionServices : IBaseServices<WMS_position>
    {
        Task<List<WMS_position>> GetPositionAll();

        Task<MessageModel<string>> CommitBulkStored(string billNo, List<WMS_position> positionList, int userId);

    }
}
