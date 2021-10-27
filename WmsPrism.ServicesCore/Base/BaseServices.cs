using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WmsPrism.IServices.Base;
using WmsPrism.Repository.Base;

namespace WmsPrism.Services.Base
{
    public class BaseServices<TEntity> : IBaseServices<TEntity> where TEntity : class, new()
    {
        public BaseRepository<TEntity> BaseDal=new BaseRepository<TEntity>();
     
        
        public async Task<TEntity> QueryById(object objId)
        {
            return await BaseDal.GetByIdAsync(objId);
        }
    }
}
