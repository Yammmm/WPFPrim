using SqlSugar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmsPrism.Repository.Base
{

    public class BaseRepository<TEntity> :SimpleClient<TEntity>, IBaseRepository<TEntity> where TEntity : class, new()
    {
        private readonly string connString = ConfigurationManager.ConnectionStrings["WmsContext"].ToString();

        public SqlSugarClient dbBase;

        private ISqlSugarClient _db;
        internal ISqlSugarClient Db
        {
            get { return _db; }
        }
        public BaseRepository(ISqlSugarClient context = null) : base(context)//注意这里要有默认值等于null
        {

            if (context == null)
            {
                dbBase = new SqlSugarClient(new ConnectionConfig()
                {
                    DbType = SqlSugar.DbType.SqlServer,
                    InitKeyType = InitKeyType.Attribute,
                    IsAutoCloseConnection = true,
                    ConnectionString = connString,
                    AopEvents = new AopEvents
                    {
                        OnLogExecuting = (sql, p) =>
                        {
                            Console.WriteLine(sql);
                            Console.WriteLine(string.Join(",", p?.Select(it => it.ParameterName + ":" + it.Value)));
                        }
                    }
                });


            }
        }

    }
}
