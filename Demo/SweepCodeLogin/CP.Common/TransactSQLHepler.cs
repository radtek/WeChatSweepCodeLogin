using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Reflection;
using System.Data;

namespace CP.Common
{
    public class TransactSQLHepler
    {
        public static string GetMaxIDSql(string Colume, string Table)
        {
            string sql = "select ISNULL(MAX({0}),0) + 1 from {1}";

            sql = string.Format(sql, Colume, Table);
            return sql;
        }


        public static string GetSelectSql(string mtable, string[] Tables, params string[] Colums)
        {
            string sql = "SELECT {0} FROM {1} WHERE  1=1";
            string col = "*", table = mtable;
            if (Colums != null)
            {
                if (Colums.Length > 0)
                {
                    col = "";
                    foreach (string item in Colums)
                        col += item;
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
        /// 获取MS SQL SERVER Update语句
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="tableName">操作表名称</param>
        /// <param name="where">Where条件T-SQL语句</param>
        /// <param name="mol">实体类对象</param>
        /// <param name="parms">返回的参数集合</param>
        /// <returns></returns>
        public static string GetUpdateSql<T>(string tableName, string where, T mol, ref Dictionary<string, object> parms) where T : new()
        {
            string sql = "UPDATE " + tableName + " SET ";
            Type type = typeof(T);

            foreach (PropertyInfo pi in type.GetProperties())
            {
                if (pi.GetValue(mol, null) != null)
                {
                    if (pi.Name.IndexOf("__") < 0)//判断是否为标识列
                    {
                        sql += string.Format(" [{0}] = @{0},", pi.Name);
                    }
                    parms.Add(pi.Name, pi.GetValue(mol, null));
                }
            }

            sql = sql.TrimEnd(',') + "  ";
            return sql + where;
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
                    parms.Add(dc.ColumnName, dr[dc.ColumnName].ToString());
                }
            }

            sql = sql.TrimEnd(',') + "  ";
            return sql + where;
        }
        /// <summary>
        /// 获取MS SQL SERVER Update语句
        /// </summary>
        /// <param name="tableName">操作表名称</param>
        /// <param name="where">Where条件T-SQL语句</param>
        /// <param name="dr">数据集合对象</param> 
        /// <returns></returns>
        public static string GetUpdateSql(string tableName, string where, DataRow dr)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("UPDATE " + tableName + " SET ");

            foreach (DataColumn dc in dr.Table.Columns)
            {
                if (!Equals(dr[dc.ColumnName], DBNull.Value) && !Equals(dr[dc.ColumnName], null))
                {
                    sql.Append(string.Format(" [{0}] = '{1}',", dc.ColumnName, CP.Common.ConvertHepler.ConvertSQL(dr[dc.ColumnName].ToString())));
                }
            }

            string result = sql.ToString().TrimEnd(',') + "  ";
            return result + where;
        }

        /// <summary>
        /// 获取MS SQL SERVER Update语句
        /// </summary>
        /// <param name="tableName">操作表名称</param>
        /// <param name="where">Where条件T-SQL语句</param>
        /// <param name="dr">数据集合对象</param> 
        /// <returns></returns>
        public static string GetUpdateSql(string tableName, string where, DataRow dr, Dictionary<string, object> Default)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("UPDATE " + tableName + " SET ");

            foreach (KeyValuePair<string, object> item in Default)
            {
                if (!string.IsNullOrEmpty(item.Key) && !Equals(item.Value, null) && !Equals(item.Value, DBNull.Value))
                {
                    sql.Append(string.Format(" [{0}] = '{1}',", item.Key, CP.Common.ConvertHepler.ConvertSQL(item.Value.ToString())));
                }
            }

            foreach (DataColumn dc in dr.Table.Columns)
            {
                if (!Equals(dr[dc.ColumnName], DBNull.Value) && !Equals(dr[dc.ColumnName], null))
                {
                    sql.Append(string.Format(" [{0}] = '{1}',", dc.ColumnName, CP.Common.ConvertHepler.ConvertSQL(dr[dc.ColumnName].ToString())));
                }
            }

            string result = sql.ToString().TrimEnd(',') + "  ";
            return result + where;
        }

        /// <summary>
        /// 获取MS SQL SERVER Insert语句
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="tableName">操作表名称</param>
        /// <param name="mol">实体类对象</param>
        /// <param name="parms">返回的参数集合</param>
        /// <returns></returns>
        public static string GetInsertSql<T>(string tableName, T mol, ref Dictionary<string, object> parms) where T : new()
        {

            string sql = "INSERT INTO  " + tableName + "({0}) VALUES ({1})";
            Type type = typeof(T);
            string cols = "", vals = "";
            foreach (PropertyInfo pi in type.GetProperties())
            {
                if (pi.GetValue(mol, null) != null)
                {
                    if (pi.Name.IndexOf("__") < 0)  //判断是否为标识列
                    {
                        cols += string.Format(" [{0}],", pi.Name);
                        vals += string.Format(" @{0},", pi.Name);
                    }
                    parms.Add(pi.Name, pi.GetValue(mol, null));
                }
            }

            sql = string.Format(sql, cols.TrimEnd(','), vals.TrimEnd(',')) + "     ";
            return sql;
        }

        /// <summary>
        /// 获取MS SQL SERVER Insert语句
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="tableName">操作表名称</param>
        /// <param name="mol">实体类对象</param>
        /// <param name="parms">返回的参数集合</param>
        /// <returns></returns>
        public static string GetInsertSql<T>(string tableName, T mol) where T : new()
        {

            string sql = "INSERT INTO  " + tableName + "({0}) VALUES ({1})";
            Type type = typeof(T);
            string cols = "", vals = "";
            foreach (PropertyInfo pi in type.GetProperties())
            {
                if (pi.GetValue(mol, null) != null)
                {
                    if (pi.Name.IndexOf("__") < 0)  //判断是否为标识列
                    {
                        cols += string.Format(" [{0}],", pi.Name);
                        vals += string.Format(" '{0}',", pi.GetValue(mol, null));
                    }
                }
            }

            sql = string.Format(sql, cols.TrimEnd(','), vals.TrimEnd(',')) + "     ";
            return sql;
        }

        /// <summary>
        /// 获取MS SQL SERVER Insert语句
        /// </summary>
        /// <param name="tableName">操作表名称</param>
        /// <param name="dr">数据集合对象</param>
        /// <param name="parms">返回的参数集合</param>
        /// <returns></returns>
        public static string GetInsertSql(string tableName, DataRow dr, ref Dictionary<string, object> parms)
        {
            string sql = "INSERT INTO  " + tableName + "({0}) VALUES ({1})";
            string cols = "", vals = "";
            foreach (DataColumn dc in dr.Table.Columns)
            {
                if (!Equals(dr[dc.ColumnName], DBNull.Value) && !Equals(dr[dc.ColumnName], null))
                {

                    cols += string.Format(" [{0}],", dc.ColumnName);
                    vals += string.Format(" @{0},", dc.ColumnName);
                    parms.Add(dc.ColumnName, dr[dc.ColumnName]);
                }
            }
            sql = string.Format(sql, cols.TrimEnd(','), vals.TrimEnd(',')) + "     ";
            return sql;
        }


        /// <summary>
        /// 获取MS SQL SERVER Insert语句
        /// </summary>
        /// <param name="tableName">操作表名称</param>
        /// <param name="dr">数据集合对象</param>
        /// <param name="parms">返回的参数集合</param>
        /// <returns></returns>
        public static string GetInsertSql(string tableName, DataRow dr, Dictionary<string, object> Default)
        {
            string sql = "INSERT INTO  " + tableName + "({0}) VALUES ({1})";
            string cols = "", vals = "";

            foreach (KeyValuePair<string, object> item in Default)
            {

                if (!string.IsNullOrEmpty(item.Key) && !Equals(item.Value, null) && !Equals(item.Value, DBNull.Value))
                {
                    cols += string.Format(" [{0}],", item.Key);
                    vals += string.Format(" '{0}',", CP.Common.ConvertHepler.ConvertSQL(item.Value.ToString()));

                }

            }

            foreach (DataColumn dc in dr.Table.Columns)
            {
                if (!Default.Keys.Contains(dc.ColumnName))
                {
                    if (!Equals(dr[dc.ColumnName], DBNull.Value) && !Equals(dr[dc.ColumnName], null))
                    {

                        cols += string.Format(" [{0}],", dc.ColumnName);
                        vals += string.Format(" '{0}',", CP.Common.ConvertHepler.ConvertSQL(dr[dc.ColumnName].ToString()));

                    }
                }
            }


            sql = string.Format(sql, cols.TrimEnd(','), vals.TrimEnd(',')) + "     ";
            return sql;
        }

        /// <summary>
        /// 获取MS SQL SERVER Insert语句
        /// </summary>
        /// <param name="tableName">操作表名称</param>
        /// <param name="dr">数据集合对象</param>
        /// <param name="parms">返回的参数集合</param>
        /// <returns></returns>
        public static string GetInsertSelectSql(string tableName, DataRow dr, Dictionary<string, object> Default)
        {
            string sql = "INSERT INTO  " + tableName + "({0}) select {1} ";
            string cols = "", vals = "";

            foreach (KeyValuePair<string, object> item in Default)
            {
                if (!string.IsNullOrEmpty(item.Key) && !Equals(item.Value, null) && !Equals(item.Value, DBNull.Value))
                {
                    cols += string.Format(" [{0}],", item.Key);
                    vals += string.Format(" {0},", CP.Common.ConvertHepler.ConvertSQL(item.Value.ToString()));

                }
            }

            foreach (DataColumn dc in dr.Table.Columns)
            {
                if (!Default.Keys.Contains(dc.ColumnName))
                {
                    if (!Equals(dr[dc.ColumnName], DBNull.Value) && !Equals(dr[dc.ColumnName], null))
                    {

                        cols += string.Format(" [{0}],", dc.ColumnName);
                        vals += string.Format(" '{0}',", CP.Common.ConvertHepler.ConvertSQL(dr[dc.ColumnName].ToString()));

                    }
                }
            }


            sql = string.Format(sql, cols.TrimEnd(','), vals.TrimEnd(',')) + "     ";
            return sql;
        }


        public static string GetDeleteSql(string tableName)
        {
            string sql = "DELETE FROM  " + tableName + " WHERE 1=1 ";
            return sql;
        }
        public static string GetDeleteSql(string tableName, string where)
        {
            string sql = "DELETE FROM  " + tableName + " " + where;
            return sql;
        }



    }
}
