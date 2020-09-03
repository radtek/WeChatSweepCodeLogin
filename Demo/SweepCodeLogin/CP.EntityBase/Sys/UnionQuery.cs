using CP.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP.EntityBase
{
    public class UnionQuery
    {
        StringBuilder _SQL = new StringBuilder();
        StringBuilder _WHERE = new StringBuilder();
        StringBuilder _ORDER = new StringBuilder();
        StringBuilder _GROUP = new StringBuilder();
        List<DbParameter> _Params = new List<DbParameter>();
        List<TableColumn> _Column = new List<TableColumn>();


        public List<TableColumn> Column
        {
            get { return _Column; }
        }


        public void MainTable(string maintable)
        {
            this._SQL.AppendLine("SELECT {0} FROM " + maintable + " ");
        }

        #region 添加查询条件
        public void addWhere(string sql)
        {
            addWhere(sql, null, null);
        }

        public void addWhere(QueryParam parm)
        {
            addWhere(parm.SQL, parm.Parms, parm.Val);
        }

        public void addWhere(string sql, string parms, object val)
        {
            if (val == null)
                val = "";

            if (!this._WHERE.ToString().ToUpper().Contains("WHERE"))
                this._WHERE.Append(" WHERE ");

            if (!string.IsNullOrEmpty(parms))
            {
                SqlParameter _parms = new SqlParameter(parms, val);
                if (_Params.Where(T => T.ParameterName == parms).Count() == 0)
                    _Params.Add(_parms);
            }
            this._WHERE.AppendLine(sql);
        }

        public void addWhere(TableColumn col, string ext, string parms, object val)
        {
            string sql = string.Format(" {0} {1} {2} ",
                    col.GetFormatString, ext, parms);

            if (val == null)
                val = "";

            if (!this._WHERE.ToString().ToUpper().Contains("WHERE"))
                this._WHERE.Append(" WHERE ");

            if (!string.IsNullOrEmpty(parms))
            {
                SqlParameter _parms = new SqlParameter(parms, val);
                if (_Params.Where(T => T.ParameterName == parms).Count() == 0)
                    _Params.Add(_parms);
            }
            this._WHERE.AppendLine(sql);
        }

        public void addWhereAnd()
        {
            this._WHERE.Append(" AND ");
        }

        public void addWhereOr()
        {
            this._WHERE.Append(" OR ");
        }

        public void addOrder(TableColumn[] col)
        {
            string sql = " ORDER BY ";
            for (int i = 0; i < col.Length; i++)
            {
                sql += col[i].GetFormatString;
                if (i < col.Length - 1)
                    sql += ", ";
            }

            this._ORDER.AppendLine(sql);
        }

        public void addGroup(TableColumn[] col)
        {
            string sql = " GROUP BY ";
            for (int i = 0; i < col.Length; i++)
            {
                sql += col[i].GetFormatString;
                if (i < col.Length - 1)
                    sql += ", ";
            }

            this._GROUP.AppendLine(sql);
        }
        #endregion

        #region 连接查询
        public void LeftJoin(string sql)
        {
            this._SQL.AppendLine(sql);
        }

        public void LeftJoin(string tablename, params UnionQueryParam[] parm)
        {
            string sql = string.Format(" LEFT JOIN {0} ON ", tablename);

            for (int i = 0; i < parm.Length; i++)
            {
                sql += string.Format(" {0} {1} {2} ",
                    parm[i].Filed1.GetFormatString,
                    parm[i].Ext,
                    parm[i].Filed2.GetFormatString);

                if (i < parm.Length - 1)
                    sql += " AND ";
            }

            this._SQL.AppendLine(sql);
        }


        public void RightJoin(string sql)
        {
            this._SQL.AppendLine(sql);
        }

        public void RightJoin(string tablename, params UnionQueryParam[] parm)
        {
            string sql = string.Format(" RIGHT JOIN {0} ON ", tablename);

            for (int i = 0; i < parm.Length; i++)
            {
                sql += string.Format(" {0} {1} {2} ",
                    parm[i].Filed1.GetFormatString,
                    parm[i].Ext,
                    parm[i].Filed2.GetFormatString);

                if (i < parm.Length - 1)
                    sql += " AND ";
            }

            this._SQL.AppendLine(sql);
        }


        public void InnerJoin(string sql)
        {
            this._SQL.AppendLine(sql);
        }

        public void InnerJoin(string tablename, params UnionQueryParam[] parm)
        {
            string sql = string.Format(" INNER JOIN {0} ON ", tablename);

            for (int i = 0; i < parm.Length; i++)
            {
                sql += string.Format(" {0} {1} {2} ",
                    parm[i].Filed1.GetFormatString,
                    parm[i].Ext,
                    parm[i].Filed2.GetFormatString);

                if (i < parm.Length - 1)
                    sql += " AND ";
            }
            this._SQL.AppendLine(sql);
        }
        #endregion

        public virtual DataTable QueryToDataTable(string connStrName)
        {
            string sql = GetSelectSQL();

            return DBHelper.ExecuteDataTable(connStrName, sql, this._Params.ToArray());
        }

        public virtual DataTable PaginationToDataTable(string connStrName, string limit, string offset, string sort, string order, out int allCount, bool isCalculation = false)
        {
            try
            {
                DbParameter[] dbparms = this._Params.ToArray();

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
                string sql = GetSelectSQL();

                allCount = GetTotalCount(connStrName, sql);
                // end -- 处理 所有行数SQL语句 

                if (!int.TryParse(limit, out _topCount))
                    _topCount = allCount;

                //处理 开始结束的数量
                startNum = _offset + 1;
                endNum = _offset + _topCount;

                //处理排序语句 (如果sort为空，则默认this.Column中第一个字段，如果该字段有别名，则用该字段的别名)
                string sqlOrder = string.IsNullOrEmpty(sort) ? "ORDER BY " + (string.IsNullOrEmpty(this.Column[0].AsName) ? this.Column[0].ColumnName : this.Column[0].AsName) : "ORDER BY " + sort + " " + order;

                //处理 数据查询SQL语句
                string sqlData = string.Format(@"
                        SELECT TOP " + _topCount.ToString() + @" * FROM (
                           SELECT ROW_NUMBER() OVER({1}) AS '_row_num_' , * FROM ({0}) AS _Tab

                        ) AS _T
                        WHERE [_row_num_] BETWEEN '{2}' AND '{3}'
                        ", sql, sqlOrder, startNum, endNum);


                return DBHelper.ExecuteDataTable(connStrName, sqlData, dbparms);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public virtual int GetTotalCount(string connStrName, string sql)
        {
            sql = string.Format("SELECT COUNT(1) AS Count FROM({0}) AS _Z", sql);

            DataTable dt = DBHelper.ExecuteDataTable(connStrName, sql, this._Params.ToArray());
            if (dt.Rows.Count > 0)
                return Convert.ToInt32(dt.Rows[0]["Count"]);
            else
                return 0;            
        }

        private string GetSelectSQL()
        {

            if (this.Column.Count <= 0)
            {
                throw new Exception("CP.EntityBase.UnionQuery.GetSelectSQL()引发了系统异常，异常描述：查询字段列表未赋值。");
            }

            string cols = string.Empty;
            for (int i = 0; i < this.Column.Count; i++)
            {
                cols += this.Column[i].GetFormatString;
                if (i < this.Column.Count - 1)
                    cols += ", ";
            }

            string sql = string.Format(this._SQL.ToString(), cols);
            sql += this._WHERE.ToString();
            sql += this._GROUP.ToString();
            sql += this._ORDER.ToString();

            return sql;
        }


        #region 实体类

        public class TableColumn
        {
            /// <summary>
            /// 
            /// 有根据某一类型状态，取不同表中的字段数据
            /// </summary>
            /// <param name="_sql">sql语句</param>
            public TableColumn(string _sql)
            {
                this.ColumnName = _sql;
            }

            public TableColumn(string _tableName, string _columnName)
            {
                this.TableName = _tableName;
                this.ColumnName = _columnName;
            }

            public TableColumn(string _tableName, string _columnName, string _asName)
            {
                this.TableName = _tableName;
                this.ColumnName = _columnName;
                this.AsName = _asName;
            }

            public string GetFormatString
            {
                get
                {
                    if (string.IsNullOrEmpty(this.TableName))
                        return this.ColumnName + (string.IsNullOrEmpty(this.AsName) ? "" : " AS " + this.AsName);
                    else
                        return this.TableName + "." + this.ColumnName + (string.IsNullOrEmpty(this.AsName) ? "" : " AS " + this.AsName);
                }
            }

            public string TableName
            {
                get;
                set;
            }
            public string ColumnName
            {
                get;
                set;
            }

            public string AsName
            {
                get;
                set;
            }

        }


        public class UnionQueryParam
        {
            public UnionQueryParam(TableColumn _filed1, TableColumn _filed2, string _ext)
            {
                this.filed1 = _filed1;
                this.filed2 = _filed2;
                this.ext = _ext;
            }

            TableColumn filed1; TableColumn filed2; string ext;

            /// <summary>
            /// 主表字段
            /// </summary>
            public TableColumn Filed1
            {
                get { return filed1; }
                set { filed1 = value; }
            }

            /// <summary>
            /// 连接表字段
            /// </summary>
            public TableColumn Filed2
            {
                get { return filed2; }
                set { filed2 = value; }
            }
            /// <summary>
            /// 运算符
            /// </summary>
            public string Ext
            {
                get { return ext; }
                set { ext = value; }
            }


        }
        #endregion

    }


}
