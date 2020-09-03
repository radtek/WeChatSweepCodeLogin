using CP.Common;
using System;
using System.Collections.Generic;
using System.Data;

public partial class Console_Console : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            string action = Request.Params["action"];
            //判断action为空为页面初始化加载 执行列表初始化操作
            if (!string.IsNullOrEmpty(action))
            {
                switch (action)
                {
                    case "command":
                        Command();
                        break;
                }
            }
        }
    }

    private void Command()
    {
        Dictionary<string, string> _json_list = new Dictionary<string, string>();

        try
        {
            string _command = Request.Params["command"];
            string _msg;
            DataTable _dt;
            int _affected;

            CP.Command.Command comm = new CP.Command.Command();
            if (!comm.Run(_command, out _msg, out _dt, out _affected))
            {
                throw new CP.Core.Web.UIException(_msg);
            }
            else
            {
                _json_list.Clear();
                _json_list.Add("Result", "0");
                _json_list.Add("Msg", _msg);
                if (_dt != null)
                    _json_list.Add("Data", CP.Common.JSONHelper.DataTableToJson(_dt));
                else
                    _json_list.Add("Data", null);
            }
        }
        catch (CP.Core.Web.UIException ex)
        {
            #region 异常信息编辑
            _json_list.Clear();
            _json_list.Add("Result", "-99");
            _json_list.Add("errMsg", "系统异常：" + ex.Message + "！");
            #endregion
        }

        string json = JSONHelper.DictionaryToJson(_json_list);
        Response.Write(json);
        Response.End();
    }
}