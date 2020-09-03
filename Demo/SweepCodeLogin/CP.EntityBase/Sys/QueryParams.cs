using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;


namespace CP.EntityBase
{
    public class QueryParams
    {

        StringBuilder _SQL = new StringBuilder();
        List<DbParameter> _Params = new List<DbParameter>();

        public string SQL
        {
            get { return _SQL.ToString(); }
        }
        public List<DbParameter> Params
        {
            get { return _Params; } 
        }

        public void addWhere(QueryParam[] parms)
        {
            foreach (QueryParam parm in parms)
            {
                addWhere(parm.SQL, parm.Parms, parm.Val);
            } 
        }

        public void addWhere(QueryParam parm)
        {
            addWhere(parm.SQL, parm.Parms, parm.Val);
        }
         
        public void addWhere(string sql)
        {
            addWhere(sql, null, null);
        }
        public void addWhere(string attr, string ext, string parms, object val)
        {
            string sql = string.Format("{0} {1} {2}", attr, ext, parms);
            addWhere(sql, parms, val);
        }
        public void addWhere(string sql, string parms, object val)
        {
            if (val == null)
                val = "";

            if (!this.SQL.ToString().ToUpper().Contains("WHERE"))
                this._SQL.Append(" WHERE ");

            if (!string.IsNullOrEmpty(parms))
            {
                SqlParameter _parms = new SqlParameter(parms, val);
                if (_Params.Where(T => T.ParameterName == parms).Count() == 0)
                    _Params.Add(_parms);
            }
            this._SQL.Append(sql);
        }
         
        public void addOrder(string sql)
        {
            if (!sql.ToUpper().Contains("ORDER BY"))
                this._SQL.Append(" ORDER BY "); 

            this._SQL.Append(sql);
        }

        public void addGroup(string sql)
        {
            if (!sql.ToUpper().Contains("GROUP BY"))
                this._SQL.Append(" GROUP BY ");

            this._SQL.Append(sql);
        }

        public void addAnd()
        {
            this._SQL.Append(" AND ");
        }

        public void addOr()
        {
            this._SQL.Append(" OR ");
        }

         
    }

    public class QueryParam
    {
        public QueryParam(string _sql, string _parms, string _val)
        {
            this.sql = _sql;
            this.parms = _parms;
            this.val = _val;
        }

        string sql; string parms; string val;

        /// <summary>
        /// 参数名称 需要执行的Where SQL语句 需包含参数
        /// 例如: UserName = @UserName
        /// </summary>
        public string SQL
        {
            get { return sql; }
            set { sql = value; }
        }

        /// <summary>
        /// 参数名称 带@ 例如 @UserName
        /// </summary>
        public string Parms
        {
            get { return parms; }
            set { parms = value; }
        } 
        /// <summary>
        /// 值
        /// </summary>
        public string Val
        {
            get { return val; }
            set { val = value; }
        }


    }
}
