using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.Data.Common;
using CP.Common;

namespace CP.EntityBase
{
    public class SqlBuilder
    {
        public static string GetAllCountSql(string mtable)
        {
            string sql = string.Format("SELECT count(1) as 'count'  FROM {0} ", mtable);
            return sql;
        }

        public static string GetSelectSql(string mtable)
        {
            return GetSelectSql(mtable, null, null);
        }
        public static string GetSelectSql(string mtable, string[] Tables, int? TopCount = null, params string[] Colums)
        {
            string sql = "SELECT {0} FROM {1} ";

            if (TopCount != null) 
                sql = "SELECT TOP "+ TopCount +" {0} FROM {1} "; 

            string col = "*", table = mtable;
            if (Colums != null)
            {
                if (Colums.Length > 0)
                {
                    col = string.Join(",", Colums); 
                }
            }
            if (Tables != null)
            {
                if (Tables.Length > 0)
                {
                    foreach (string item in Tables)
                        table += item;
                }
            }

            sql = string.Format(sql, col, table);
            return sql;
        }
 

        /// <summary>
        /// 获取MS SQL SERVER Update语句 注意判断返回的字符串是不是空
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="tableName">操作表名称</param>
        /// <param name="where">Where条件T-SQL语句</param>
        /// <param name="mol">实体类对象</param>
        /// <param name="parms">返回的参数集合</param>
        /// <returns></returns>
        public static string GetUpdateSql(Entity en, ref Dictionary<string, object> parms)
        {
            string sql = "UPDATE " + en.PhysicsTable + " SET ";
            Type type = en.GetType();
            bool isupdate = false;

            foreach (PropertyInfo pi in type.GetProperties())
            {
                if (pi.GetValue(en, null) != null)
                {
                    if (!Equals(pi.Name, en.IDENTITY))  //判断是否为标识列
                    {
                        if (en.EnMap.Attrs.Contains(pi.Name))
                        {
                            string p = GetParmsName(pi.Name, null);

                            sql += string.Format(" [{0}] = {1},", pi.Name, p);

                            parms.Add(p, pi.GetValue(en, null));

                            isupdate = true;
                        } 
                    }
                }
            }

            if (isupdate)
            {
                sql = sql.TrimEnd(',') + "  ";
                return sql;
            }
            else
                return "";
        }
        /// <summary>
        /// 获取MS SQL SERVER Update语句
        /// </summary>
        /// <param name="tableName">操作表名称</param>
        /// <param name="where">Where条件T-SQL语句</param>
        /// <param name="dr">数据集合对象</param>
        /// <param name="parms">返回的参数集合</param>
        /// <returns></returns>
        public static string GetUpdateSql(string tableName, string where, DataRow dr, ref Dictionary<string, object> parms)
        {
            string sql = "UPDATE " + tableName + " SET ";

            foreach (DataColumn dc in dr.Table.Columns)
            {
                if (!Equals(dr[dc.ColumnName], DBNull.Value) && !Equals(dr[dc.ColumnName], null))
                {
                    sql += string.Format(" [{0}] = @{0},", dc.ColumnName);
                    parms.Add(dc.ColumnName, dr[dc.ColumnName]);
                }
            }

            sql = sql.TrimEnd(',') + "  ";
            return sql + where;
        }
        /// <summary>
        /// 获取MS SQL SERVER Insert语句
        /// </summary>
        /// <param name="en">实体类</param>
        /// <returns></returns>
        public static string GetInsertSql(Entity en, ref Dictionary<string, object> parms)
        { 
            string sql = "INSERT INTO  " + en.PhysicsTable + "({0}) VALUES ({1})";

            Type type = en.GetType();
            string cols = "", vals = "";
            foreach (PropertyInfo pi in type.GetProperties())
            {
                if (pi.GetValue(en, null) != null)
                {
                    if (!Equals(pi.Name, en.IDENTITY))  //判断是否为标识列
                    {
                        if (en.EnMap.Attrs.Contains(pi.Name))
                        {
                            string p = GetParmsName(pi.Name, null);

                            cols += string.Format(" [{0}],", pi.Name);
                            vals += string.Format(p + ",");

                            parms.Add(p, pi.GetValue(en, null));  
                        }
                    }
                   
                }
            }

            sql = string.Format(sql, cols.TrimEnd(','), vals.TrimEnd(',')) + " ";
            return sql;
        }

        /// <summary>
        /// 获取MS SQL SERVER Insert语句
        /// </summary>
        /// <param name="list">实体类集合</param>
        /// <returns></returns>
        public static string GetInsertSql(List<Entity> list, ref Dictionary<string, object> parms)
        {
            StringBuilder buffer = new StringBuilder(); 

            int index = 0;
            foreach (Entity en in list)
            { 
                string sql = "INSERT INTO  " + en.PhysicsTable + "({0}) VALUES ({1})";

                Type type = en.GetType();
                string cols = "", vals = "";
                foreach (PropertyInfo pi in type.GetProperties())
                {
                    if (pi.GetValue(en, null) != null)
                    {
                        if (!Equals(pi.Name, en.IDENTITY))  //判断是否为标识列
                        {
                            if (en.EnMap.Attrs.Contains(pi.Name))
                            {
                                string p = GetParmsName(pi.Name, index);

                                cols += string.Format(" [{0}],", pi.Name);
                                vals += string.Format(p + ","); 

                                parms.Add(p, pi.GetValue(en, null)); 
                            }
                        }
                    }
                }

                sql = string.Format(sql, cols.TrimEnd(','), vals.TrimEnd(',')) + " ";
                buffer.Append(sql);
                buffer.Append("\r\n"); 

                index++;
            }

            return buffer.ToString();
        }


        /// <summary>
        /// 获取MS SQL SERVER Insert语句
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="tableName">操作表名称</param>
        /// <param name="mol">实体类对象</param>
        /// <param name="parms">返回的参数集合</param>
        /// <returns></returns>
        public static string GetInsertSqlForID(Entity en, ref Dictionary<string, object> parms)
        {

            string sql = "INSERT INTO  " + en.PhysicsTable + "({0}) VALUES ({1});select @@identity "; 
            
            Type type = en.GetType();
            string cols = "", vals = "";
            foreach (PropertyInfo pi in type.GetProperties())
            {
                if (pi.GetValue(en, null) != null)
                {
                    if (!Equals(pi.Name, en.IDENTITY))  //判断是否为标识列
                    {
                        if (en.EnMap.Attrs.Contains(pi.Name))
                        {
                            string p = GetParmsName(pi.Name, null);

                            cols += string.Format(" [{0}],", pi.Name);
                            vals += string.Format(p + ",");

                            parms.Add(p, pi.GetValue(en, null));  
                        }
                    }

                }
            }

            sql = string.Format(sql, cols.TrimEnd(','), vals.TrimEnd(',')) + " ";
            return sql;
        }
        /// <summary>
        /// 获取MS SQL SERVER Insert语句
        /// </summary>
        /// <param name="tableName">操作表名称</param>
        /// <param name="dr">数据集合对象</param>
        /// <param name="parms">返回的参数集合</param>
        /// <returns></returns>
        public static string GetInsertSql(string tableName, DataRow dr)
        {
            string sql = "INSERT INTO  " + tableName + "({0}) VALUES ({1})";
            string cols = "", vals = "";
            foreach (DataColumn dc in dr.Table.Columns)
            {
                if (!Equals(dr[dc.ColumnName], DBNull.Value) && !Equals(dr[dc.ColumnName], null))
                {

                    cols += string.Format(" [{0}],", dc.ColumnName);
                    vals += string.Format(" '{0}',", dr[dc.ColumnName].ToString().Replace("'","''")); 
                }
            }
            sql = string.Format(sql, cols.TrimEnd(','), vals.TrimEnd(',')) + "     ";
            return sql;
        }

        public static string GetDeleteSql(Entity en)
        {
            string sql = "DELETE FROM  " + en.PhysicsTable  ;
            return sql;
        }
        public static string GetDeleteSql(string tableName, string where)
        {
            string sql = "DELETE FROM  " + tableName + " " + where;
            return sql;
        }

         

        private static string GetParmsName(string fieldname, int? index)
        {
            if (index != null)  
                return  "@" + CP.Common.Common.CreateGUID().Replace("-","") + index;
            else
                return "@" + CP.Common.Common.CreateGUID().Replace("-", "");
        }
    }
}
