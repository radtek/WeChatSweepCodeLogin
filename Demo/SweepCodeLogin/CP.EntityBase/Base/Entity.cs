using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CP.Common;
using System.Data.Common;
using System.Data.OleDb;

namespace CP.EntityBase
{
    [Serializable]
    public class Entity : EntityBase
    {


        #region 属性
        /// <summary>
        /// 标识列
        /// </summary>
        public string IDENTITY { get { return this.EnMap.IDENTITY; } }
        /// <summary>
        /// 表名
        /// </summary>
        public string PhysicsTable { get { return this.EnMap.PhysicsTable; } }


        public List<string> PrimaryKey { get { return this.EnMap.PrimaryKey; } }


        private string _ReadConnStrName = string.Empty;
        private string _WriteConnStrName = string.Empty;

        public string ReadConnStrName
        {
            get { return string.IsNullOrEmpty(_ReadConnStrName) ? Common.Common.AppSetting("ReadConnStrName") : _ReadConnStrName; }
            set { _ReadConnStrName = value; }
        }

        public string WriteConnStrName
        {
            get { return string.IsNullOrEmpty(_WriteConnStrName) ? Common.Common.AppSetting("WriteConnStrName") : _WriteConnStrName; }
            set { _WriteConnStrName = value; }
        }

        #endregion


        #region 方法

        public static string GetValByKey(Entity en, string key)
        {
            Type type = en.GetType();

            object obj = type.GetProperty(key).GetValue(en, null);

            if (obj == null)
                return null;
            else
                return obj.ToString();
        }


        public Entity Clone()
        {
            return base.Clone() as Entity;
        }


        #endregion

        #region 查询函数

        /// <summary>
        /// 根据标识列值给当前实体类赋值
        /// </summary>
        /// <param name="ID"></param>
        public virtual void Query(string ID)
        {
            QueryParams parms = new QueryParams();
            parms.addWhere(string.Format("{0}=@ID", this.IDENTITY), "@ID", ID);
            Query(parms);
        }

        /// <summary>
        /// 根据查询参数给当前实体类赋值
        /// </summary>
        /// <param name="ID"></param>
        public virtual void Query(QueryParams parms)
        {
            DataTable dt = QueryToDataTable(parms);

            if (dt.Rows.Count > 0)
            {
                Type type = this.GetType();
                DataRow dr = dt.Rows[0];

                System.Reflection.PropertyInfo[] pi = type.GetProperties();
                foreach (System.Reflection.PropertyInfo item in pi)
                {
                    string feildName = item.Name.Replace("__", "");
                    if (dr.Table.Columns.Contains(feildName))
                    {
                        if (dr[feildName] != null && dr[feildName] != DBNull.Value)
                        {
                            item.SetValue(this, CP.Common.DataTableHepler.ConvertVal(item, dr[feildName]), null);
                        }
                    }
                }
            }
        }


        #region QueryToList
        /// <summary>
        /// 根据查询参数 获取泛型集合
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <returns></returns>
        public virtual List<T> QueryToList<T>() where T : new()
        {
            return DataTableHepler.DataTableToModel<T>(QueryToDataTable());
        }
        /// <summary>
        /// 根据查询参数 获取泛型集合
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="parms">查询参数</param>
        /// <returns></returns>
        public virtual List<T> QueryToList<T>(QueryParams parms) where T : new()
        {
            return DataTableHepler.DataTableToModel<T>(QueryToDataTable(parms));
        }

        /// <summary>
        /// 根据查询参数 获取泛型集合
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="parms">查询参数</param>
        /// <returns></returns>
        public virtual List<T> QueryToList<T>(QueryParams parms, string[] colums) where T : new()
        {
            return DataTableHepler.DataTableToModel<T>(QueryToDataTable(parms, colums));
        }
        public virtual List<T> QueryToList<T>(QueryParams parms, string[] colums, int top) where T : new()
        {
            return DataTableHepler.DataTableToModel<T>(QueryToDataTable(parms, colums, top));
        }
        #endregion

        #region QueryToDataTable
        /// <summary>
        /// 根据查询参数 获取数据集合
        /// </summary> 
        /// <param name="parms">查询参数</param>
        /// <returns></returns>
        public virtual DataTable QueryToDataTable()
        {
            return QueryToDataTable(null);
        }

        public virtual DataTable QueryToDataTable(QueryParams parms)
        {
            return QueryToDataTable(parms, null);
        }

        public virtual DataTable QueryToDataTable(QueryParams parms, string[] colums)
        {
            string sql = SqlBuilder.GetSelectSql(this.PhysicsTable, null, null, colums);
            if (parms == null)
                parms = new QueryParams();

            DbParameter[] dbparms = parms.Params.ToArray();
            sql = sql + parms.SQL;
            return DBHelper.ExecuteDataTable(this.ReadConnStrName, sql, dbparms);
        }
        public virtual DataTable QueryToDataTable(QueryParams parms, string[] colums, int top)
        {
            string sql = SqlBuilder.GetSelectSql(this.PhysicsTable, null, top, colums);
            if (parms == null)
                parms = new QueryParams();

            DbParameter[] dbparms = parms.Params.ToArray();
            sql = sql + parms.SQL;
            return DBHelper.ExecuteDataTable(this.ReadConnStrName, sql, dbparms);
        }
        #endregion

        #region PaginationToDataTable

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <returns></returns>
        public virtual DataTable PaginationToDataTable(string limit, string offset, string sort, string order, out int allCount)
        {
            return PaginationToDataTable(limit, offset, sort, order, null, out allCount);
        }
        public virtual DataTable PaginationToDataTable(string limit, string offset, string sort, string order, QueryParams parms, out int allCount)
        {
            try
            {
                if (parms == null)
                    parms = new QueryParams();

                DbParameter[] dbparms = parms.Params.ToArray();


                //处理 所有行数SQL语句
                allCount = GetTotalCount(parms);
                // end -- 处理 所有行数SQL语句 

                //处理 开始结束的数量
                int startNum, endNum;

                int _topCount, _offset;

                if (!int.TryParse(limit, out _topCount))
                    _topCount = allCount;
                if (!int.TryParse(offset, out _offset))
                    _offset = 0;

                startNum = _offset + 1;
                endNum = _offset + _topCount;


                //处理排序语句
                string sqlOrder = string.IsNullOrEmpty(sort) ? "ORDER BY " + this.EnMap.IDENTITY : "ORDER BY " + sort + " " + order;

                //处理 数据查询SQL语句
                string sqlData = string.Format(@"
                SELECT TOP " + _topCount.ToString() + @" * FROM (
                   SELECT ROW_NUMBER() OVER({1}) AS '_row_num_' , * FROM {0}
                   {2}
                ) AS _T
                WHERE [_row_num_] BETWEEN '{3}' AND '{4}'
                ", PhysicsTable, sqlOrder, parms.SQL, startNum, endNum);


                return DBHelper.ExecuteDataTable(this.ReadConnStrName, sqlData, dbparms);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 分页查询只查询指定字段
        /// </summary> 
        /// <returns></returns>
        public virtual DataTable PaginationToDataTable(string[] fields, string limit, string offset, string sort, string order, out int allCount)
        {
            return PaginationToDataTable(fields, limit, offset, sort, order, null, out allCount);
        }
        public virtual DataTable PaginationToDataTable(string[] fields, string limit, string offset, string sort, string order, QueryParams parms, out int allCount, bool isCalculation = false)
        {
            try
            {
                if (parms == null)
                    parms = new QueryParams();

                DbParameter[] dbparms = parms.Params.ToArray();

                int startNum, endNum;
                int _topCount, _offset;

                if (isCalculation == true)
                {
                    int limitI;
                    if (!int.TryParse(limit, out limitI))
                        throw new Exception("参数名:limit 格式错误,值:" + limit);

                    if (!int.TryParse(offset, out _offset))
                        throw new Exception("参数名:offset 格式错误,值:" + offset);

                    _offset = (_offset - 1) * limitI;
                }
                else
                {
                    if (!int.TryParse(offset, out _offset))
                        _offset = 0;
                }

                //处理 所有行数SQL语句
                allCount = GetTotalCount(parms);
                // end -- 处理 所有行数SQL语句 


                if (!int.TryParse(limit, out _topCount))
                    _topCount = allCount;

                //处理 开始结束的数量
                startNum = _offset + 1;
                endNum = _offset + _topCount;


                //处理排序语句
                string sqlOrder = string.IsNullOrEmpty(sort) ? "ORDER BY " + this.EnMap.IDENTITY : "ORDER BY " + sort + " " + order;

                //处理 数据查询SQL语句
                string sqlData = string.Format(@"
                SELECT TOP " + _topCount.ToString() + @" * FROM (
                   SELECT ROW_NUMBER() OVER({1}) AS '_row_num_' , {5} FROM {0}
                   {2}
                ) AS _T
                WHERE [_row_num_] BETWEEN '{3}' AND '{4}'
                ", PhysicsTable, sqlOrder, parms.SQL, startNum, endNum, string.Join(",", fields));


                return DBHelper.ExecuteDataTable(this.ReadConnStrName, sqlData, dbparms);
            }
            catch (Exception)
            {

                throw;
            }
        }



        public virtual DataTable PaginationToDataTable(string[] fields, string limit, string offset, string sort, string order)
        {
            return PaginationToDataTable(fields, limit, offset, sort, order, null);
        }
        public virtual DataTable PaginationToDataTable(string[] fields, string limit, string offset, string sort, string order, QueryParams parms, bool isCalculation = false)
        {
            try
            {
                if (parms == null)
                    parms = new QueryParams();

                DbParameter[] dbparms = parms.Params.ToArray();

                int startNum, endNum;
                int _topCount, _offset;

                if (isCalculation == true)
                {
                    int limitI;
                    if (!int.TryParse(limit, out limitI))
                        throw new Exception("参数名:limit 格式错误,值:" + limit);

                    if (!int.TryParse(offset, out _offset))
                        throw new Exception("参数名:offset 格式错误,值:" + offset);

                    _offset = (_offset - 1) * limitI;
                }
                else
                {
                    if (!int.TryParse(offset, out _offset))
                        _offset = 0;
                }

                if (!int.TryParse(limit, out _topCount))
                    _topCount = int.MaxValue;

                //处理 开始结束的数量
                startNum = _offset + 1;
                endNum = _offset + _topCount;


                //处理排序语句
                string sqlOrder = string.IsNullOrEmpty(sort) ? "ORDER BY " + this.EnMap.IDENTITY : "ORDER BY " + sort + " " + order;

                //处理 数据查询SQL语句
                string sqlData = string.Format(@"
                SELECT TOP " + _topCount.ToString() + @" * FROM (
                   SELECT ROW_NUMBER() OVER({1}) AS '_row_num_' , {5} FROM {0}
                   {2}
                ) AS _T
                WHERE [_row_num_] BETWEEN '{3}' AND '{4}'
                ", PhysicsTable, sqlOrder, parms.SQL, startNum, endNum, string.Join(",", fields));


                return DBHelper.ExecuteDataTable(this.ReadConnStrName, sqlData, dbparms);
            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <summary>
        /// 分页查询 用于自定义开始行数与截止行数 函数内部提供分页计算逻辑
        /// </summary>
        /// <param name="fields">字段列表</param>
        /// <param name="startnum">开始行数</param>
        /// <param name="endnum">结束行数</param>
        /// <param name="sort">排序字段</param>
        /// <param name="order">排序方式</param> 
        /// <returns>DataTable类型结果集</returns>
        public virtual DataTable PaginationToDataTable(string[] fields, int startnum, int endnum, string sort, string order)
        {
            try
            {
                return PaginationToDataTable(fields, startnum, endnum, sort, order, null);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 分页查询 用于自定义开始行数与截止行数 函数内部提供分页计算逻辑
        /// </summary>
        /// <param name="fields">字段列表</param>
        /// <param name="startnum">开始行数</param>
        /// <param name="endnum">结束行数</param>
        /// <param name="sort">排序字段</param>
        /// <param name="order">排序方式</param>
        /// <param name="parms">查询参数</param>
        /// <returns>DataTable类型结果集</returns>
        public virtual DataTable PaginationToDataTable(string[] fields, int startnum, int endnum, string sort, string order, QueryParams parms)
        {
            try
            {
                if (startnum > endnum)
                    throw new Exception("参数名:startnum,endnum 起始行数不能大于截止行数");

                if (parms == null)
                    parms = new QueryParams();

                DbParameter[] dbparms = parms.Params.ToArray();

                int startNum, endNum, _topCount;

                //处理 开始结束的数量
                startNum = startnum;
                endNum = endnum;

                _topCount = endNum - startNum + 1;

                //处理排序语句
                string sqlOrder = string.IsNullOrEmpty(sort) ? "ORDER BY " + this.EnMap.IDENTITY : "ORDER BY " + sort + " " + order;

                //处理 数据查询SQL语句
                string sqlData = string.Format(@"
                SELECT TOP " + _topCount.ToString() + @" * FROM (
                   SELECT ROW_NUMBER() OVER({1}) AS '_row_num_' , {5} FROM {0}
                   {2}
                ) AS _T
                WHERE [_row_num_] BETWEEN '{3}' AND '{4}'
                ", PhysicsTable, sqlOrder, parms.SQL, startNum, endNum, string.Join(",", fields));


                return DBHelper.ExecuteDataTable(this.ReadConnStrName, sqlData, dbparms);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region GetTotalCount
        /// <summary>
        /// 根据传入查询条件返回总行数
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public virtual int GetTotalCount()
        {
            return GetTotalCount(null);
        }
        public virtual int GetTotalCount(QueryParams parms)
        {
            string sql = SqlBuilder.GetSelectSql(PhysicsTable, null, null, " COUNT(1) AS 'COUNT'");

            if (parms == null)
                parms = new QueryParams();

            DbParameter[] dbparms = parms.Params.ToArray();
            sql = sql + parms.SQL;

            DataTable dt = DBHelper.ExecuteDataTable(this.ReadConnStrName, sql, dbparms);
            if (dt.Rows.Count > 0)
                return Convert.ToInt32(dt.Rows[0]["COUNT"]);
            return 0;
        }
        #endregion

        #endregion

        #region 新增函数
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public virtual int Insert(List<Entity> list)
        {
            return Insert(null, list);
        }
        public virtual int Insert(DbTransaction tran, List<CP.EntityBase.Entity> list)
        {

            try
            {
                int relt = 0;

                if (list.Count > 0)
                {
                    Dictionary<string, object> parms = new Dictionary<string, object>();

                    string sql = SqlBuilder.GetInsertSql(list, ref parms);

                    if (tran != null)
                        relt = DBHelper.ExecuteNonQuery(tran, sql, parms);
                    else
                        relt = DBHelper.ExecuteNonQuery(this.WriteConnStrName, sql, parms);
                }
                return relt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual int Insert()
        {
            return Insert(null, this);
        }
        public virtual int Insert(DbTransaction tran)
        {
            return Insert(tran, this);
        }
        public virtual int Insert(DbTransaction tran, Entity en)
        {
            try
            {
                Dictionary<string, object> dparms = new Dictionary<string, object>();
                string sql = SqlBuilder.GetInsertSql(en, ref dparms);

                int relt = 0;
                if (tran != null)
                    relt = DBHelper.ExecuteNonQuery(tran, sql, dparms);
                else
                    relt = DBHelper.ExecuteNonQuery(this.WriteConnStrName, sql, dparms);

                return relt;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public virtual int InsertReID()
        {
            return InsertReID(null, this);
        }
        public virtual int InsertReID(DbTransaction tran)
        {
            return InsertReID(tran, this);
        }
        public virtual int InsertReID(DbTransaction tran, Entity en)
        {
            try
            {
                Dictionary<string, object> parms = new Dictionary<string, object>();
                string sql = SqlBuilder.GetInsertSqlForID(en, ref parms);
                int relt = 0;
                if (tran != null)
                    relt = Convert.ToInt32(DBHelper.ExecuteScalar(tran, sql, parms));
                else
                    relt = Convert.ToInt32(DBHelper.ExecuteScalar(this.WriteConnStrName, sql, parms));

                return relt;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 修改函数
        public virtual int Update()
        {
            DbTransaction tran = null;
            return Update(tran);
        }
        public virtual int Update(DbTransaction tran)
        {
            if (string.IsNullOrEmpty(this.IDENTITY))
            {
                if (this.PrimaryKey.Count == 0)
                    throw new Exception("实体类未设置标识列或主键，无法执行修改函数");
            }

            QueryParams objQuery = new QueryParams();

            if (!string.IsNullOrEmpty(this.IDENTITY))
            {
                objQuery.addWhere(string.Format("{0} = @IDENTITY", this.IDENTITY), "@IDENTITY", GetValByKey(this, this.IDENTITY));
            }
            else
            {
                foreach (var item in this.PrimaryKey)
                {
                    objQuery.addWhere(string.Format("{0} = @{0}", item), "@" + item, GetValByKey(this, item));
                    objQuery.addAnd();
                }
                objQuery.addWhere("1=1");
            }

            if (tran != null)
                return Update(tran, this, objQuery);
            else
                return Update(null, this, objQuery);
        }
        public virtual int Update(QueryParams parms)
        {
            return Update(null, this, parms);
        }
        public virtual int Update(DbTransaction tran, QueryParams parms)
        {
            return Update(tran, this, parms);
        }

        public virtual int Update(DbTransaction tran, Entity en, QueryParams parms)
        {
            try
            {
                Dictionary<string, object> dparms = new Dictionary<string, object>();

                string sql = SqlBuilder.GetUpdateSql(en, ref dparms);

                if (string.IsNullOrEmpty(sql))
                    return 1;

                List<DbParameter> parameters = new List<DbParameter>();
                if (parms != null)
                {
                    sql += parms.SQL;
                    parameters = parms.Params;
                }

                foreach (KeyValuePair<string, object> item in dparms)
                    parameters.Add(new System.Data.SqlClient.SqlParameter(item.Key, item.Value));

                int relt = 0;
                if (tran != null)
                    relt = DBHelper.ExecuteNonQuery(tran, sql, parameters.ToArray());
                else
                    relt = DBHelper.ExecuteNonQuery(this.WriteConnStrName, sql, parameters.ToArray());

                return relt;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 逻辑删除函数
        public virtual int Delete(List<Entity> list, bool oneConnect)
        {
            //if (oneConnect)
            //{
            //    StringBuilder sql = new StringBuilder();
            //    foreach (var en in list)
            //    {
            //        sql.Append(SqlBuilder.GetDeleteSql(en));

            //        QueryParams objQuery = new QueryParams();
            //        objQuery.addWhere(string.Format("{0} = '{1}'", en.IDENTITY, GetValByKey(en, en.IDENTITY)));

            //        sql.Append(objQuery.SQL);
            //    }

            //    if (!string.IsNullOrEmpty(sql.ToString()))
            //        return DBHelper.ExecuteNonQuery(this.WriteConnStrName, sql.ToString());
            //    else
            //        return 0;
            //}
            //else
            //{
            #region  事务

            using (DbConnection conn = DBHelper.GetConnection(this.WriteConnStrName))
            {
                conn.Open();
                DbTransaction tran = conn.BeginTransaction();
                try
                {
                    #region 写入数据
                    foreach (Entity mod in list)
                    {
                        if (string.IsNullOrEmpty(mod.IDENTITY))
                        {
                            throw new Exception("实体类未设置标识列，无法执行修改函数");
                        }

                        QueryParams objQuery = new QueryParams();
                        objQuery.addWhere(string.Format("{0} = @IDENTITY", mod.IDENTITY), "@IDENTITY", GetValByKey(mod, mod.IDENTITY));

                        int i = Delete(tran, mod, objQuery);
                        //如果有失败的数据 则全部回滚 抛出异常
                        if (i <= 0)
                            throw new Exception("存在执行失败数据， 数据已回滚。");
                    }
                    #endregion

                    tran.Commit();
                    return 1;
                }
                catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

            #endregion
            //}
        }

        public virtual int Delete()
        {
            if (string.IsNullOrEmpty(this.IDENTITY))
            {
                if (this.PrimaryKey.Count == 0)
                    throw new Exception("实体类未设置标识列或主键，无法执行逻辑删除函数。");
            }

            QueryParams objQuery = new QueryParams();

            if (!string.IsNullOrEmpty(this.IDENTITY))
            {
                objQuery.addWhere(string.Format("{0} = @IDENTITY", this.IDENTITY), "@IDENTITY", GetValByKey(this, this.IDENTITY));
            }
            else
            {
                foreach (var item in this.PrimaryKey)
                {
                    objQuery.addWhere(string.Format("{0} = @{0}", item), "@" + item, GetValByKey(this, item));
                    objQuery.addAnd();
                }
                objQuery.addWhere("1=1");
            }
            return Delete(null, this, objQuery);
        }

        public virtual int Delete(QueryParams parms)
        {
            return Delete(null, this, parms);
        }
        public virtual int Delete(DbTransaction tran, QueryParams parms)
        {
            return Delete(tran, this, parms);
        }
        public virtual int Delete(DbTransaction tran, Entity en, QueryParams parms)
        {
            if (string.IsNullOrEmpty(this.EnMap.DeleteFeild))
                throw new Exception("实体类未设置逻辑删除标识字段，所以无法使用逻辑删除功能。");

            DbParameter[] parameters = new DbParameter[0];
            string sql = string.Format(@"UPDATE {0} SET {1}='{2}' ",
                this.PhysicsTable, this.EnMap.DeleteFeild, IsDelete.Y
            );

            if (parms == null)
                parms = new QueryParams();

            sql += parms.SQL;
            parameters = parms.Params.ToArray();

            if (tran != null)
                return DBHelper.ExecuteNonQuery(tran, sql, parameters);
            return DBHelper.ExecuteNonQuery(this._WriteConnStrName, sql, parameters);
        }
        #endregion

        #region 逻辑删除恢复函数
        public virtual int Restore()
        {
            if (string.IsNullOrEmpty(this.IDENTITY))
            {
                if (this.PrimaryKey.Count == 0)
                    throw new Exception("实体类未设置标识列或主键，无法执行逻辑删除函数。");
            }

            QueryParams objQuery = new QueryParams();

            if (!string.IsNullOrEmpty(this.IDENTITY))
            {
                objQuery.addWhere(string.Format("{0} = @IDENTITY", this.IDENTITY), "@IDENTITY", GetValByKey(this, this.IDENTITY));
            }
            else
            {
                foreach (var item in this.PrimaryKey)
                {
                    objQuery.addWhere(string.Format("{0} = @{0}", item), "@" + item, GetValByKey(this, item));
                    objQuery.addAnd();
                }
                objQuery.addWhere("1=1");
            }
            return Restore(null, this, objQuery);
        }

        public virtual int Restore(QueryParams parms)
        {
            return Restore(null, this, parms);
        }
        public virtual int Restore(DbTransaction tran, QueryParams parms)
        {
            return Restore(tran, this, parms);
        }
        public virtual int Restore(DbTransaction tran, Entity en, QueryParams parms)
        {
            if (string.IsNullOrEmpty(this.EnMap.DeleteFeild))
                throw new Exception("实体类未设置逻辑删除标识字段，所以无法使用逻辑删除功能。");

            DbParameter[] parameters = new DbParameter[0];
            string sql = string.Format(@"UPDATE {0} SET {1}='{2}' ",
                this.PhysicsTable, this.EnMap.DeleteFeild, IsDelete.N
            );

            if (parms == null)
                parms = new QueryParams();

            sql += parms.SQL;
            parameters = parms.Params.ToArray();

            if (tran != null)
                return DBHelper.ExecuteNonQuery(tran, sql, parameters);
            return DBHelper.ExecuteNonQuery(this._WriteConnStrName, sql, parameters);
        }
        #endregion

        #region 强制物理删除函数
        public virtual int Remove(List<Entity> list, bool oneConnect)
        {
            if (oneConnect)
            {
                StringBuilder sql = new StringBuilder();
                foreach (var en in list)
                {
                    sql.Append(SqlBuilder.GetDeleteSql(en));

                    QueryParams objQuery = new QueryParams();
                    objQuery.addWhere(string.Format("{0} = '{1}'", en.IDENTITY, GetValByKey(en, en.IDENTITY)));

                    sql.Append(objQuery.SQL);
                }

                if (!string.IsNullOrEmpty(sql.ToString()))
                    return DBHelper.ExecuteNonQuery(this.WriteConnStrName, sql.ToString());
                else
                    return 0;
            }
            else
            {
                #region  事务

                using (DbConnection conn = DBHelper.GetConnection(this.WriteConnStrName))
                {
                    conn.Open();
                    DbTransaction tran = conn.BeginTransaction();
                    try
                    {
                        #region 写入数据
                        foreach (Entity mod in list)
                        {
                            if (string.IsNullOrEmpty(mod.IDENTITY))
                            {
                                throw new Exception("实体类未设置标识列，无法执行修改函数");
                            }

                            QueryParams objQuery = new QueryParams();
                            objQuery.addWhere(string.Format("{0} = @IDENTITY", mod.IDENTITY), "@IDENTITY", GetValByKey(mod, mod.IDENTITY));

                            int i = Remove(tran, mod, objQuery);
                            //如果有失败的数据 则全部回滚 抛出异常
                            if (i <= 0)
                                throw new Exception("存在执行失败数据， 数据已回滚。");
                        }
                        #endregion

                        tran.Commit();
                        return 1;
                    }
                    catch (Exception)
                    {
                        tran.Rollback();
                        throw;
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }

                #endregion
            }
        }

        public virtual int Remove()
        {
            DbTransaction tran = null;
            return Remove(tran);
        }

        public virtual int Remove(DbTransaction tran)
        {
            if (string.IsNullOrEmpty(this.IDENTITY))
            {
                if (this.PrimaryKey.Count == 0)
                    throw new Exception("实体类未设置标识列或主键，无法执行删除函数");
            }

            QueryParams objQuery = new QueryParams();

            if (!string.IsNullOrEmpty(this.IDENTITY))
            {
                objQuery.addWhere(string.Format("{0} = @IDENTITY", this.IDENTITY), "@IDENTITY", GetValByKey(this, this.IDENTITY));
            }
            else
            {
                foreach (var item in this.PrimaryKey)
                {
                    objQuery.addWhere(string.Format("{0} = @{0}", item), "@" + item, GetValByKey(this, item));
                    objQuery.addAnd();
                }
                objQuery.addWhere("1=1");
            }
            return Remove(tran, this, objQuery);
        }

        public virtual int Remove(QueryParams parms)
        {
            return Remove(null, this, parms);
        }
        public virtual int Remove(DbTransaction tran, QueryParams parms)
        {
            return Remove(tran, this, parms);
        }
        public virtual int Remove(DbTransaction tran, Entity en, QueryParams parms)
        {
            DbParameter[] parameters = new DbParameter[0];
            string sql = SqlBuilder.GetDeleteSql(en);

            if (parms == null)
                parms = new QueryParams();

            sql += parms.SQL;
            parameters = parms.Params.ToArray();

            if (tran != null)
                return DBHelper.ExecuteNonQuery(tran, sql, parameters);
            return DBHelper.ExecuteNonQuery(this._WriteConnStrName, sql, parameters);
        }
        #endregion

        #region Excel操作
        /// <summary>
        /// 函数停用 需重新改造 Aviv 2016-08-27
        /// 因SQL语句拼接时,直接拼接字符串而没有使用参数特此停用
        /// </summary>
        /// <param name="path">excel表在服务器端路径</param>
        /// <param name="sheetName">数据所在sheet名</param>
        /// <returns></returns>
        public int ImportDBFromExcel(string path, string sheetName)
        {
            try
            {
                string errMsg = string.Empty;
                using (DataTable dt = ExcelHepler.RenderFromExcel(path, sheetName, out errMsg))
                {
                    if (dt == null) 
                        throw new Exception(errMsg); 

                    DataTable mydt = dt.Copy();
                    foreach (DataColumn item in dt.Columns)
                    {
                        Attr attr = this.EnMap.Attrs.GetAttrByDesc(item.ColumnName);
                        if (attr != null)
                            mydt.Columns[item.ColumnName].ColumnName = attr.Field;
                        else
                            mydt.Columns.Remove(item.ColumnName);
                    }

                    StringBuilder sql = new StringBuilder();

                    foreach (DataRow item in mydt.Rows)
                    {
                        //创建临时表
                        sql.Append(SqlBuilder.GetInsertSql(this.PhysicsTable, item) + "\r\n");
                    }

                    if (!string.IsNullOrEmpty(sql.ToString()))
                        return DBHelper.ExecuteNonQuery(this.ReadConnStrName, sql.ToString());
                    else
                        return 0;
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// excel数据导入数据库
        /// </summary>
        /// <param name="dt">excel表中数据</param>
        /// <returns></returns>
        public int ImportDBFromExcel(DataTable dt)
        {
            try
            {
                DataTable mydt = dt.Copy();
                foreach (DataColumn item in dt.Columns)
                {
                    Attr attr = this.EnMap.Attrs.GetAttrByDesc(item.ColumnName);
                    if (attr != null)
                        mydt.Columns[item.ColumnName].ColumnName = attr.Field;
                    else
                        mydt.Columns.Remove(item.ColumnName);
                }

                StringBuilder sql = new StringBuilder();

                foreach (DataRow item in mydt.Rows)
                {
                    //创建临时表
                    sql.Append(SqlBuilder.GetInsertSql(this.PhysicsTable, item) + "\r\n");
                }

                if (!string.IsNullOrEmpty(sql.ToString()))
                    return DBHelper.ExecuteNonQuery(this.ReadConnStrName, sql.ToString());
                else
                    return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region 判断实体类是不是所有字段都是空的
        /// <summary>
        /// 判断实体类map映射字段列表中所有字段是否都为null 或者 string.Empty
        /// </summary>
        /// <returns></returns>
        public bool IsNullOrEmpty()
        {
            foreach (Attr attr in this.EnMap.Attrs)
            {
                if (!string.IsNullOrEmpty(GetValByKey(this, attr.Key)))
                    return false;
            }

            return true;
        }

        #endregion

        public override Map CreateEnMap()
        {
            return new Map();
        }


    }
}
