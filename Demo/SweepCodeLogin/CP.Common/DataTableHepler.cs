using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;

namespace CP.Common
{
    public class DataTableHepler
    {
        public static T DataRowToModel<T>(DataRow dr) where T : new()
        {
            try
            {
                T mod = new T();
                Type type = typeof(T);

                PropertyInfo[] pi = type.GetProperties();
                foreach (PropertyInfo item in pi)
                {
                    string feildName = item.Name.Replace("__", "");
                    if (dr.Table.Columns.Contains(feildName))
                    {
                        if (dr[feildName] != null && dr[feildName] != DBNull.Value)
                        {
                            item.SetValue(mod, ConvertVal(item, dr[feildName]), null); 
                        }
                    }
                }
                return mod;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static object ConvertVal(PropertyInfo item, object val)
        {
            Type _t = item.PropertyType; 
            if (_t == typeof(int?)) 
                return Convert.ToInt32(val);
            if (_t == typeof(int))
                return Convert.ToInt32(val);
            else if (_t == typeof(double?))
                return Convert.ToDouble(val);
            else if (_t == typeof(double))
                return Convert.ToDouble(val);
            else if (_t == typeof(decimal?))
                return Convert.ToDecimal(val);
            else if (_t == typeof(decimal))
                return Convert.ToDecimal(val);
            else if (_t == typeof(bool?))
                return Convert.ToBoolean(val);
            else if (_t == typeof(bool))
                return Convert.ToBoolean(val);
            else if (_t == typeof(DateTime))
                return Convert.ToDateTime(val);
            else if (_t == typeof(DateTime?))
                return Convert.ToDateTime(val);
            else if (_t == typeof(char?))
                return Convert.ToChar(val);

            

            return val.ToString();
        }

        public static List<T> DataTableToModel<T>(DataTable dt) where T : new()
        {
            try
            {
                List<T> list = new List<T>(); 
                foreach (DataRow dr in dt.Rows) 
                    list.Add(DataRowToModel<T>(dr)); 
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
