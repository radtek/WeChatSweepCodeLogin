using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data.Common;
using System.Data;
using System.Reflection;
using System.Data.SqlClient;
 

namespace CP.Common
{
    /// <summary>
    /// 版本 1.0.3
    /// 修改日期 2015-10-16
    /// </summary>
    public static class DBHelper
    {
        //private static string connectionString = ConfigurationManager.ConnectionStrings["PlatFormDB"].ToString();
        private static readonly string providerName = "System.Data.SqlClient";

        #region Basic Function
         
        public static DbConnection GetConnection(string connStrName)
        {
            DbProviderFactory _factory = DbProviderFactories.GetFactory(providerName);
            DbConnection connection = _factory.CreateConnection();
            
            //ConnectionStringSettings setting = ConfigurationManager.ConnectionStrings[connStrName];
            //string connString = string.Empty; 
            //if (setting != null) 
            //    connString = setting.ConnectionString;
             
            //if (string.IsNullOrEmpty(connString))
            //    connString = ConfigurationManager.AppSettings[connStrName];

            string connString = string.Empty;
             
            if (Common.AppSetting("IsEncrypt").ToLower() == "true")
            {
                connString = DESEncrypt.Decrypt(ConfigurationManager.ConnectionStrings[connStrName].ConnectionString);
            }
            else
            {
                connString = ConfigurationManager.ConnectionStrings[connStrName].ConnectionString;
            }

            connection.ConnectionString = connString;
            return connection;
        }

        public static DbTransaction GetTransaction(string connStrName)
        {
            DbConnection conn = GetConnection(connStrName);
            if (conn.State != ConnectionState.Open)
                conn.Open();
            DbTransaction tran = conn.BeginTransaction(); 
            return tran;
        }

        /// <summary>
        /// GetCommand 获取命令参数 command 对象
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        private static DbCommand GetCommand(string commandText, CommandType commandType, DbConnection connection)
        {
            DbCommand command = connection.CreateCommand();
            command.CommandTimeout = 0;
            command.CommandText = commandText;
            command.CommandType = commandType;
            return command;
        }
        /// <summary>
        /// GetCommand 方法重载
        /// </summary>
        /// <param name="commandText">sql语句</param>
        /// <param name="connection"></param>
        /// <returns></returns>
        private static DbCommand GetCommand(string commandText, DbConnection connection)
        {
            DbCommand command = connection.CreateCommand();
            command.CommandTimeout = 0;
            command.CommandText = commandText;
            command.CommandType = CommandType.Text;
            return command;
        }
         
        /// <summary>
        /// GetParameter 用于为命令设置参数
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        private static DbParameter GetParameter(string paramName, object paramValue, DbCommand command)
        {
            DbParameter parameter = command.CreateParameter();
            parameter.ParameterName = paramName;
            parameter.Value = paramValue;
            return parameter;
        }
        #endregion

        #region 存储过程
         
         
        

        /// <summary>
        /// 执行有返回值的有参数的存储过程
        /// </summary>
        /// <param name="cmdText">存储过程名</param>
        /// <param name="para">参数</param>
        /// <returns></returns>
        public static DataTable ExecuteScalarProc(string connStrName, string proname)
        {
            using (DbConnection connection = GetConnection(connStrName))
            {
                DbProviderFactory _factory = DbProviderFactories.GetFactory(providerName);
                DbCommand command = GetCommand(proname, CommandType.StoredProcedure, connection);
                command.CommandTimeout = 0;
                
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                DbDataAdapter da = _factory.CreateDataAdapter();
                da.SelectCommand = command;
                DataTable datatable = new DataTable();
                da.Fill(datatable);
                return datatable;
            }
        }
         
        
        /// <summary>
        /// 执行有返回值的有参数的存储过程
        /// </summary>
        /// <param name="cmdText">存储过程名</param>
        /// <param name="para">参数</param>
        /// <returns></returns>
        public static DataTable ExecuteScalarProc(string connStrName, string proname, Dictionary<string, object> parameters)
        {
            using (DbConnection connection = GetConnection(connStrName))
            { 
                DbProviderFactory _factory = DbProviderFactories.GetFactory(providerName);
                DbCommand command = GetCommand(proname, CommandType.StoredProcedure, connection);
                command.CommandTimeout = 0;
                if (parameters != null)
                {
                    foreach (KeyValuePair<string, object> p in parameters)
                        command.Parameters.Add(GetParameter(p.Key, p.Value == null ? DBNull.Value : p.Value, command));
                }
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                DbDataAdapter da = _factory.CreateDataAdapter();
                da.SelectCommand = command;
                DataTable datatable = new DataTable();
                da.Fill(datatable);
                return datatable; 
            }
        }


        public static DataTable ExecuteScalarProc(DbTransaction tran, string proname, Dictionary<string, object> parameters)
        {
            DbConnection connection = tran.Connection;

            DbProviderFactory _factory = DbProviderFactories.GetFactory(providerName);
            DbCommand command = GetCommand(proname, CommandType.StoredProcedure, connection);
            command.CommandTimeout = 0;
            command.Transaction = tran;
            if (parameters != null)
            {
                foreach (KeyValuePair<string, object> p in parameters)
                    command.Parameters.Add(GetParameter(p.Key, p.Value == null ? DBNull.Value : p.Value, command));
            }
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            DbDataAdapter da = _factory.CreateDataAdapter();
            da.SelectCommand = command;
            DataTable datatable = new DataTable();
            da.Fill(datatable);
            return datatable; 
        }
        #endregion

        #region 执行sql语句
         
         

        /// <summary>
        /// 执行有返回值有参数的sql语句
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string connStrName,string cmdText, Dictionary<string, object> para)
        {
            using (DbConnection connection = GetConnection(connStrName))
            {
                DbCommand command = GetCommand(cmdText, CommandType.Text, connection);
                command.CommandTimeout = 0;
                if (para != null)
                { 
                    foreach (KeyValuePair<string, object> p in para) 
                        command.Parameters.Add(GetParameter(p.Key, p.Value == null ? DBNull.Value : p.Value, command));
                }
                connection.Open();
                object val = command.ExecuteScalar();
                command.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        /// 执行有返回值有参数的sql语句
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public static object ExecuteScalar(DbTransaction tran, string cmdText, Dictionary<string, object> para)
        {
            DbConnection connection = tran.Connection;

            DbCommand command = GetCommand(cmdText, CommandType.Text, connection);
            command.CommandTimeout = 0;
            if (para != null)
            {
                foreach (KeyValuePair<string, object> p in para)
                    command.Parameters.Add(GetParameter(p.Key, p.Value == null ? DBNull.Value : p.Value, command));
            }
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            command.Transaction = tran;
            object val = command.ExecuteScalar();
            command.Parameters.Clear();
            return val;

        }
        
         
        
        #endregion

        #region GetReader
        /// <summary>
        /// 执行无参数的sql语句,返回DbDataReader对象
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <returns></returns>
        public static DbDataReader GetReader(string connStrName, string sqlCommand)
        {
            try
            {
                DbConnection connection = GetConnection(connStrName);
                DbCommand command = GetCommand(sqlCommand, CommandType.Text, connection);
                command.CommandTimeout = 0;
                connection.Open();
                DbDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                command.Parameters.Clear();
                return reader;
            }
            catch (Exception ex)
            {
                Console.Write("" + ex.Message);
                return null;
            }
        }
      
        
        #endregion

        #region ExecuteDateTable
        
        public static DataTable ExecuteDataTable(string connStrName, string safeSql)
        {
            using (DbConnection connection = GetConnection(connStrName))
            {
                DbProviderFactory _factory = DbProviderFactories.GetFactory(providerName);
                DbCommand command = GetCommand(safeSql, CommandType.Text, connection);
                command.CommandTimeout = 0;
                connection.Open();
                DbDataAdapter da = _factory.CreateDataAdapter();
                da.SelectCommand = command;
                DataTable datatable = new DataTable();
                da.Fill(datatable);
                return datatable;
            }
        }
         
        public static DataTable ExecuteDataTable(DbTransaction tran, string safeSql)
        {
            DbConnection connection = tran.Connection;
            DbProviderFactory _factory = DbProviderFactories.GetFactory(providerName);

            DbCommand command = GetCommand(safeSql, CommandType.Text, connection);
            command.CommandTimeout = 0;
            command.Transaction = tran; 

            if (connection.State == ConnectionState.Closed)
                connection.Open();
            DbDataAdapter da = _factory.CreateDataAdapter();
            da.SelectCommand = command;
            DataTable datatable = new DataTable();
            da.Fill(datatable);
            return datatable;
        }

        public static DataTable ExecuteDataTable(DbConnection connection, string safeSql)
        { 
            DbProviderFactory _factory = DbProviderFactories.GetFactory(providerName);
            DbCommand command = GetCommand(safeSql, CommandType.Text, connection);
            command.CommandTimeout = 0;
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            DbDataAdapter da = _factory.CreateDataAdapter();
            da.SelectCommand = command;
            DataTable datatable = new DataTable();
            da.Fill(datatable);
            return datatable;

        }
         
        public static DataTable ExecuteDataTable(string connStrName, string safeSql, Dictionary<string, object> parameters)
        {
            using (DbConnection connection = GetConnection(connStrName))
            {
                DbProviderFactory _factory = DbProviderFactories.GetFactory(providerName);
                DbCommand command = GetCommand(safeSql, CommandType.Text, connection);
                command.CommandTimeout = 0;
                if (parameters != null)
                {
                    foreach (KeyValuePair<string, object> p in parameters)
                        command.Parameters.Add(GetParameter(p.Key, p.Value == null ? DBNull.Value : p.Value, command));
                }
                connection.Open();
                DbDataAdapter da = _factory.CreateDataAdapter();
                da.SelectCommand = command;
                DataTable datatable = new DataTable();
                da.Fill(datatable);
                return datatable;
            }
        }
         
        public static DataTable ExecuteDataTable(string connStrName, string safeSql, params DbParameter[] parameters)
        {
            using (DbConnection connection = GetConnection(connStrName))
            {
                DbProviderFactory _factory = DbProviderFactories.GetFactory(providerName);
                DbCommand command = GetCommand(safeSql, CommandType.Text, connection);
                command.CommandTimeout = 0;
                if (parameters != null)
                    command.Parameters.AddRange(ConvertParms(parameters));
                connection.Open();
                
                DbDataAdapter da = _factory.CreateDataAdapter();
                da.SelectCommand = command;
                DataTable datatable = new DataTable();
                da.Fill(datatable);
                command.Parameters.Clear();
                return datatable;
            }
        }


        public static DataTable ExecuteDataTable(DbTransaction tran, string safeSql, params DbParameter[] parameters)
        {
            DbConnection connection = tran.Connection;
            DbProviderFactory _factory = DbProviderFactories.GetFactory(providerName);
             
            DbCommand command = GetCommand(safeSql, CommandType.Text, connection);
            command.CommandTimeout = 0; 
            if (parameters != null)
                command.Parameters.AddRange(ConvertParms(parameters)); 
            command.Transaction = tran;

            if (connection.State == ConnectionState.Closed)
                connection.Open();
            DbDataAdapter da = _factory.CreateDataAdapter();
            da.SelectCommand = command;
            DataTable datatable = new DataTable();
            da.Fill(datatable);
            return datatable;
        }
        public static DataTable ExecuteDataTable(DbConnection connection, string safeSql, params DbParameter[] parameters)
        {

            DbProviderFactory _factory = DbProviderFactories.GetFactory(providerName);
            DbCommand command = GetCommand(safeSql, CommandType.Text, connection);
            command.CommandTimeout = 0;
            if (parameters != null)
                command.Parameters.AddRange(ConvertParms(parameters));
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            DbDataAdapter da = _factory.CreateDataAdapter();
            da.SelectCommand = command;
            DataTable datatable = new DataTable();
            da.Fill(datatable);
            command.Parameters.Clear();
            return datatable;

        }
        #endregion

        #region ExecuteNonQuery
        
        public static int ExecuteNonQuery(string connStrName, string sqlCommand)
        {
            int result = 0;
            using (DbConnection connection = GetConnection(connStrName))
            {
                DbCommand command = GetCommand(sqlCommand, CommandType.Text, connection);
                command.CommandTimeout = 0;
                connection.Open();
                result = command.ExecuteNonQuery();
                command.Parameters.Clear();
                return result;
            }
        }

        public static int ExecuteNonQuery(DbTransaction tran, string sqlCommand)
        { 
            DbConnection connection = tran.Connection; 
            DbCommand command = GetCommand(sqlCommand, CommandType.Text, connection);
            command.CommandTimeout = 0;
            command.Transaction = tran;
            if (connection.State == ConnectionState.Closed) 
                connection.Open();
            int result = command.ExecuteNonQuery();
            command.Parameters.Clear();
            return result; 
        }

        public static int ExecuteNonQuery(DbConnection connection, string sqlCommand)
        { 
            DbCommand command = GetCommand(sqlCommand, CommandType.Text, connection);
            command.CommandTimeout = 0; 
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            int result = command.ExecuteNonQuery();
            command.Parameters.Clear();
            return result;
        }
         
        public static int ExecuteNonQuery(string connStrName, string sqlCommand, Dictionary<string, object> parameters)
        {
            int result = 0;
            using (DbConnection connection = GetConnection(connStrName))
            {
                DbCommand command = GetCommand(sqlCommand, CommandType.Text, connection);
                command.CommandTimeout = 0;
                if (parameters != null)
                {
                    foreach (KeyValuePair<string, object> p in parameters)
                        command.Parameters.Add(GetParameter(p.Key, p.Value == null ? DBNull.Value : p.Value, command));
                }
                connection.Open();
                result = command.ExecuteNonQuery();
                command.Parameters.Clear();
                return result;
            }
        }

 
        public static int ExecuteNonQuery(string connStrName, string sqlCommand, params DbParameter[] parameters)
        {
            int result = 0;
            using (DbConnection connection = GetConnection(connStrName))
            {
                DbCommand command = GetCommand(sqlCommand, CommandType.Text, connection);
                command.CommandTimeout = 0;
                if (parameters != null)
                {
                    foreach (DbParameter p in parameters)
                        command.Parameters.Add(p);
                }
                connection.Open();
                result = command.ExecuteNonQuery();
                command.Parameters.Clear();
                return result;
            }
        }

        /// <summary>
        /// 执行有事务的Sql语句
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(DbTransaction tran, string sqlCommand, Dictionary<string, object> parameters)
        {
            int result = 0;
            DbConnection connection = tran.Connection;

            DbCommand command = GetCommand(sqlCommand, CommandType.Text, connection);
            command.CommandTimeout = 0;
            if (parameters != null)
            {
                foreach (KeyValuePair<string, object> p in parameters)
                    command.Parameters.Add(GetParameter(p.Key, p.Value == null ? DBNull.Value : p.Value, command));
            }
            command.Transaction = tran;
            if (connection.State== ConnectionState.Closed) 
                connection.Open();
            result = command.ExecuteNonQuery();
            command.Parameters.Clear();
            return result; 
        }
        /// <summary>
        /// 执行有事务的Sql语句
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(DbTransaction tran, string sqlCommand, params DbParameter[] parameters)
        {
            int result = 0;
            DbConnection connection = tran.Connection;

            DbCommand command = GetCommand(sqlCommand, CommandType.Text, connection);
            command.CommandTimeout = 0;
            if (parameters != null)
            {
                foreach (DbParameter p in parameters)
                    command.Parameters.Add(p);
            }
            command.Transaction = tran;
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            result = command.ExecuteNonQuery();
            command.Parameters.Clear();
            return result;
        }

        /// <summary>
        /// 执行有事务的Sql语句
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(DbConnection connection, string sqlCommand, params DbParameter[] parameters)
        {
            int result = 0;
          
            DbCommand command = GetCommand(sqlCommand, CommandType.Text, connection);
            command.CommandTimeout = 0;
            if (parameters != null)
            {
                foreach (DbParameter p in parameters)
                    command.Parameters.Add(p);
            } 
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            result = command.ExecuteNonQuery();
            command.Parameters.Clear();
            return result;
        }
        #endregion

        #region ExecuteDataSet

      
        public static DataSet ExecuteDataSet(string connStrName, string safeSql, Dictionary<string, object> parameters)
        {
            using (DbConnection connection = GetConnection(connStrName))
            {
                DbProviderFactory _factory = DbProviderFactories.GetFactory(providerName);
                DbCommand command = GetCommand(safeSql, CommandType.Text, connection);
                command.CommandTimeout = 0;
                if (parameters != null)
                {
                    foreach (KeyValuePair<string, object> p in parameters)
                        command.Parameters.Add(GetParameter(p.Key, p.Value == null ? DBNull.Value : p.Value, command));
                }
                connection.Open();
                DbDataAdapter da = _factory.CreateDataAdapter();
                da.SelectCommand = command;
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
        }

        #endregion 

         

        #region Ohter
        /// <summary>
        /// 将参数集合合并
        /// </summary> 
        public static DbParameter[] CompoundParms(List<DbParameter[]> parms)
        {
            List<DbParameter> list = new List<DbParameter>();

            foreach (DbParameter[] arr in parms)
            {
                foreach (DbParameter item in arr)
                    list.Add(item);
            }
            return list.ToArray();
        }

        public static DbParameter[] ConvertParms(DbParameter[] parms)
        {
            foreach (DbParameter item in parms)
            {
                if (item.Value == null) 
                    item.Value = DBNull.Value; 
            } 
            return parms;
        }
        #endregion


        #region 分页查询

        public static DataTable PaginationToDataTable(string connectStr, string[] fields, string sql, string limit, string offset, string sort, string order)
        {
            try
            {

                int startNum, endNum;
                int _topCount, _offset, _limit;


                if (!int.TryParse(offset, out _offset))
                    _offset = 0;
                if (!int.TryParse(limit, out _limit))
                    _limit = 0;


                // end -- 处理 所有行数SQL语句 


                if (!int.TryParse(limit, out _topCount))
                    _topCount = int.MaxValue;

                //处理 开始结束的数量
                startNum = _limit * (_offset) + 1;
                endNum = _limit * (_offset) + _topCount;


                //处理排序语句
                string sqlOrder = "ORDER BY " + sort + " " + order;

                //处理 数据查询SQL语句
                string sqlData = string.Format(@"
                SELECT TOP " + _topCount.ToString() + @" * FROM (
                   SELECT ROW_NUMBER() OVER({1}) AS '_row_num_' , {4} FROM (
                   {0}
                   ) AS _Z
                ) AS _T
                WHERE [_row_num_] BETWEEN '{2}' AND '{3}'
                ", sql, sqlOrder, startNum, endNum, string.Join(",", fields));


                return DBHelper.ExecuteDataTable(connectStr, sqlData);
            }
            catch (Exception)
            {

                throw;
            }
        }



        public static int GetTotalCount(string connStrName, string sql)
        {
            string _sql = string.Format(@"SELECT COUNT(1) AS 'COUNT' FROM({0})AS _Z", sql);

            DataTable dt = DBHelper.ExecuteDataTable(connStrName, _sql);
            if (dt.Rows.Count > 0)
                return Convert.ToInt32(dt.Rows[0]["COUNT"]);
            return 0;
        }
        #endregion
    }
}