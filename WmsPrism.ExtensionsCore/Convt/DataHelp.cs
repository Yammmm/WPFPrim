using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Text;

namespace WmsPrism.Extensions.Convt
{
    public static class DataHelp
    {
        /// <summary>
        /// DataTable转List
        /// </summary>
        /// <typeparam name="T">list中的类型</typeparam>
        /// <param name="dt">要转换的DataTable</param>
        /// <returns></returns>
        public static List<T> DatatTableToList<T>(DataTable dt) where T : class, new()
        {
            List<T> list = new List<T>();
            T t = new T();
            PropertyInfo[] prop = t.GetType().GetProperties();
            //遍历所有DataTable的行
            foreach (DataRow dr in dt.Rows)
            {
                t = new T();
                //通过反射获取T类型的所有成员
                foreach (PropertyInfo pi in prop)
                {
                    //DataTable列名=属性名
                    if (dt.Columns.Contains(pi.Name))
                    {
                        //属性值不为空
                        if (dr[pi.Name] != DBNull.Value)
                        {
                            object value = Convert.ChangeType(dr[pi.Name], pi.PropertyType);
                            //给T类型字段赋值
                            pi.SetValue(t, value, null);
                        }
                    }
                }
                //将T类型添加到集合list
                list.Add(t);
            }
            return list;

        }

        /// <summary>
        /// 将List转换成DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable dt = new DataTable();
            for (int i = 0; i < properties.Count; i++)
            {
                PropertyDescriptor property = properties[i];
                dt.Columns.Add(property.Name, property.PropertyType);
            }
            object[] values = new object[properties.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = properties[i].GetValue(item);
                }
                dt.Rows.Add(values);
            }
            return dt;
        }
    }
}
