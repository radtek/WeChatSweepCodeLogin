 <%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Web;
using System.Data;
using System.Collections.Generic;
using CP.Entiry;
using CP.EntityBase;
using CP.Common;

public class Handler : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        try
        {
            string _action = HttpContext.Current.Request.Params["action"];

            switch (_action.ToLower())
            {
                case "p_bandorganization":
                    BandOrganization(context);
                    break;
                case "p_loadindustry":
                    LoadIndustry(context);
                    break;
                case "p_loadindustryb":
                    LoadIndustryB(context);
                    break;
                case "p_loadrunparameter":
                    loadrunparameter(context);
                    break;
                case "p_loaddictionaries":
                    loaddictionaries(context);
                    break;
                case "p_loadcountry":
                    Country(context);
                    break;
                case "p_loadpapromotionprocince":
                    loadpapromotionprocince(context);
                    break;
            }
        }
        catch (System.Threading.ThreadAbortException)
        {
            // Response.End() 引发此异常. 无需处理.
        }
        catch (Exception ex)
        {
            new CP.Core.Web.SuperPage().Exception(ex);
        }
    }



    #region 省 市 区

    private void BandOrganization(HttpContext context)
    {
        Dictionary<string, string> json = new Dictionary<string, string>();
        try
        {
            string _type = context.Request.Params["type"];
            string _provinc = context.Request.Params["provinc"];
            string _city = context.Request.Params["city"];
            string _district = context.Request.Params["district"];

            string textfield, valfield;

            CP.EntityBase.Entity entity = new CP.EntityBase.Entity();
            CP.EntityBase.QueryParams parmas = new CP.EntityBase.QueryParams();

            switch (_type)
            {
                case "province":
                    entity = new CP.Entiry.sys_Province();
                    textfield = "ProvinceName";
                    valfield = "ProvinceID";
                    break;
                case "city":
                    entity = new CP.Entiry.sys_City();
                    parmas.addWhere("provinceID=@provinceID", "@provinceID", _provinc);
                    textfield = "CityName";
                    valfield = "CityID";
                    break;
                case "district":
                    entity = new CP.Entiry.sys_District();
                    parmas.addWhere("cityID=@cityID", "@cityID", _city);
                    textfield = "DistrictName";
                    valfield = "DistrictID";
                    break;
                default:
                    throw new CP.Core.Web.UIException("无效的类型。");
            }

            DataTable dt = entity.QueryToDataTable(parmas);

            json.Clear();
            json.Add("Result", "0");
            json.Add("Data", CP.Common.JSONHelper.DataTableToJson(dt));
            json.Add("textField", textfield);
            json.Add("valField", valfield);
        }
        catch (Exception)
        {
            throw;
        }
        context.Response.Write(CP.Common.JSONHelper.DictionaryToJson(json));
        context.Response.End();
    }


    #endregion


    #region 国家

    private void Country(HttpContext context)
    {
        Dictionary<string, string> json = new Dictionary<string, string>();
        try
        {

            CP.Entiry.sys_Dictionaries Dictionaries = new sys_Dictionaries();
            QueryParams param = new QueryParams();
            param.addWhere(CP.Entiry.sys_DictionariesAttr.Type, "=", "@Country", "Country");
            DataTable dt = Dictionaries.QueryToDataTable(param);

            json.Clear();
            json.Add("Result", "0");
            json.Add("Data", CP.Common.JSONHelper.DataTableToJson(dt));

        }
        catch (Exception)
        {
            throw;
        }
        context.Response.Write(CP.Common.JSONHelper.DictionaryToJson(json));
        context.Response.End();
    }


    #endregion


    #region 行业领域

    private void LoadIndustry(HttpContext context)
    {
        Dictionary<string, string> json = new Dictionary<string, string>();
        try
        {
            #region 验证
            string PID = context.Request.Params["PID"];
            if (string.IsNullOrEmpty(PID))
                throw new CP.Core.Web.UIException("系统出错：父ID为空。");
            #endregion

            CP.Entiry.sys_Dictionaries Dictionaries = new CP.Entiry.sys_Dictionaries();
            QueryParams param = new QueryParams();
            string[] colnum = new string[]{
                   CP.Entiry.sys_DictionariesAttr.ID,
                      CP.Entiry.sys_DictionariesAttr.Code,
                  CP.Entiry.sys_DictionariesAttr.Text
            };
            param.addWhere(CP.Entiry.sys_DictionariesAttr.PartentID, "=", "@PID", PID);
            param.addAnd();
            param.addWhere(CP.Entiry.sys_DictionariesAttr.Type, "=", "@Type", "PA_IndustryArea");
            param.addOrder(CP.Entiry.sys_DictionariesAttr.Code);


            DataTable table = Dictionaries.QueryToDataTable(param, colnum);

            json.Clear();
            json.Add("Result", "0");
            json.Add("Data", CP.Common.JSONHelper.DataTableToJson(table));

        }
        catch (Exception ex)
        {
            throw;
        }
        context.Response.Write(CP.Common.JSONHelper.DictionaryToJson(json));
        context.Response.End();
    }

    /// <summary>
    /// 加载行业领域B计划 崔萌 2020-2-28 17:29:44
    /// </summary>
    /// <param name="context"></param>
    private void LoadIndustryB(HttpContext context)
    {
        Dictionary<string, string> json = new Dictionary<string, string>();

        try
        {
            UnionQuery query = new UnionQuery();

            //1.组装查询字段
            query.Column.Add(new UnionQuery.TableColumn(string.Format(" a.{0} AS FID ", sys_DictionariesAttr.ID)));
            query.Column.Add(new UnionQuery.TableColumn(string.Format(" a.{0} AS FCode ", sys_DictionariesAttr.Code)));
            query.Column.Add(new UnionQuery.TableColumn(string.Format(" a.{0} AS FText ", sys_DictionariesAttr.Text)));
            query.Column.Add(new UnionQuery.TableColumn(string.Format(" b.{0} AS SCode ", sys_DictionariesAttr.Code)));
            query.Column.Add(new UnionQuery.TableColumn(string.Format(" b.{0} AS SText ", sys_DictionariesAttr.Text)));

            //2.组装查询表
            query.MainTable(string.Format(@" ( SELECT {0}, {1}, {2} FROM {3} 
                                             WHERE Type = 'PA_IndustryArea' AND PartentID = 0 ) a ",
                sys_DictionariesAttr.ID, sys_DictionariesAttr.Code, sys_DictionariesAttr.Text,
                sys_Dictionaries.PhysicsTable));

            query.InnerJoin(string.Format(@" (SELECT {0}, {1}, {2} FROM {3} 
                                                WHERE Type = 'PA_IndustryArea' AND PartentID <> 0 ) b",
                                            sys_DictionariesAttr.PartentID, sys_DictionariesAttr.Code,
                                            sys_DictionariesAttr.Text, sys_Dictionaries.PhysicsTable
                                            ),
                                            new UnionQuery.UnionQueryParam(
                                                new UnionQuery.TableColumn(string.Format(" a.{0} ", sys_DictionariesAttr.ID)),
                                                new UnionQuery.TableColumn(string.Format(" b.{0} ", sys_DictionariesAttr.PartentID)),
                                                "="));

            query.addOrder(new UnionQuery.TableColumn[] {
                new UnionQuery.TableColumn(string.Format(" a.{0} ", sys_DictionariesAttr.Code))
            });

            DataTable table = query.QueryToDataTable(sysConst.ReadConnStrName);

            json.Clear();
            json.Add("Result", "0");
            json.Add("Data", JSONHelper.DataTableToJson(table));

        }
        catch (Exception ex)
        {
            throw;
        }

        context.Response.Write(JSONHelper.DictionaryToJson(json));
        context.Response.End();
    }

    #endregion



    #region 读取系统变量
    private void loadrunparameter(HttpContext context)
    {
        Dictionary<string, string> json = new Dictionary<string, string>();
        try
        {
            string _name = context.Request.Params["name"];



            System.Reflection.FieldInfo ftype = typeof(CP.Common.sysConst).GetField(_name);
            var value = ftype.GetValue(null);

            json.Clear();
            json.Add("Result", "0");
            json.Add("Value", value + "");

        }
        catch (Exception)
        {
            throw;
        }
        context.Response.Write(CP.Common.JSONHelper.DictionaryToJson(json));
        context.Response.End();
    }


    #endregion

    #region 读取系统变量
    private void loaddictionaries(HttpContext context)
    {
        Dictionary<string, string> json = new Dictionary<string, string>();
        try
        {
            string _type = context.Request.Params["type"];


            CP.Entiry.sys_Dictionaries dict = new CP.Entiry.sys_Dictionaries();
            CP.EntityBase.QueryParams param = new QueryParams();
            param.addWhere(CP.Entiry.sys_DictionariesAttr.Type, "=", "@Type", _type);

            System.Data.DataTable dt = dict.QueryToDataTable(param);

            json.Clear();
            json.Add("Result", "0");
            json.Add("Data", CP.Common.JSONHelper.DataTableToJson(dt));

        }
        catch (Exception)
        {
            throw;
        }
        context.Response.Write(CP.Common.JSONHelper.DictionaryToJson(json));
        context.Response.End();
    }


    #endregion

    #region
    /// <summary>
    /// 加载区域匹配  省份
    /// </summary>
    private void loadpapromotionprocince(HttpContext context)
    {
        Dictionary<string, string> json = new Dictionary<string, string>();
        try
        {
            CP.Entiry.sys_Province province = new CP.Entiry.sys_Province();
            CP.EntityBase.QueryParams param = new QueryParams();

            param.addOrder("LEN(ProvinceName)");

            System.Data.DataTable province_dt = province.QueryToDataTable(param);

            json.Clear();
            json.Add("Result", "0");
            json.Add("Data", CP.Common.JSONHelper.DataTableToJson(province_dt));
        }
        catch (Exception)
        {

            throw;
        }
        context.Response.Write(CP.Common.JSONHelper.DictionaryToJson(json));
        context.Response.End();

    }

    #endregion


    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}