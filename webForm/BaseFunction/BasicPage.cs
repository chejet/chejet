using System;
using System.Data;
using System.Drawing.Imaging;
using XkCms.DataOper.Data;
using XkCms.DataOper.UI;
using System.Web.UI.WebControls;

namespace XkCms.WebForm.BaseFunction
{
    /// <summary>
    /// BasicPage 的摘要说明
    /// </summary>
    public class BasicPage : XkPage
    {
        protected string dbType = "0";
        protected string ErrMsg;
        protected SysInfo Cms = new SysInfo();

        protected override void OnInit(EventArgs e)
        {
            this.ConnectDb();
            if (Application["XkCms"] == null)
                SetupSystemDate();
            Cms = (SysInfo)Application["XkCms"];
            base.OnInit(e);
        }

        /// <summary>
        /// 连接数据库
        /// </summary>
        public override void ConnectDb()
        {
            if (doh == null)
            {
                dbType = System.Configuration.ConfigurationManager.AppSettings["dbType"].ToString();
                if (dbType == "0")
                    doh = new XkCms.DataOper.Data.OleDbOperHandler(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["dbPath"]));
                else
                {
                    dbType = "1";
                    System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.AppSettings["dbConnStr"]);
                    doh = new XkCms.DataOper.Data.SqlDbOperHandler(conn);
                }
            }
        }

        /// <summary>
        /// 初始化系统信息
        /// </summary>
        protected void SetupSystemDate()
        {
            Cms = new SysInfo(doh);
            Application.Lock();
            Application["XkCms"] = Cms;
            Application.UnLock();
        }

        /// <summary>
        /// 获得逐级缩进的栏目名
        /// </summary>
        /// <param name="sName">栏目名</param>
        /// <param name="sCode">栏目code</param>
        /// <returns>逐级缩进的栏目名</returns>
        protected string GetColumnListName(string sName, string sCode)
        {
            int Level = (sCode.Length / 4 - 1) * 2;
            string sStr = sCode.Length == 4 ? "" : "├";
            if (Level > 0)
            {
                for (int i = 0; i < Level; i++)
                    sStr += "－";
            }
            return sStr + sName;
        }

        /// <summary>
        /// 获取querystring
        /// </summary>
        /// <param name="s">参数名</param>
        /// <returns>返回值</returns>
        public string q(string s)
        {
            if (Request.QueryString[s] != null)
            {
                return Request.QueryString[s].ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取post得到的参数
        /// </summary>
        /// <param name="s">参数名</param>
        /// <returns>返回值</returns>
        protected string f(string s)
        {
            if (Request.Form[s] == null) return string.Empty;
            return Request.Form[s];
        }

        /// <summary>
        /// 为GridView行添加功能
        /// </summary>
        /// <param name="e">行</param>
        /// <param name="c">"删除"LinkButton所在单元格</param>
        /// <param name="w">"删除"LinkButton为单元格中第几个控件</param>
        protected void GvRowAddFun(ref GridViewRowEventArgs e, int c, int w)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='E4E8EF'");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=''");
                if (w > 0)
                    ((LinkButton)(e.Row.Cells[c].Controls[w])).Attributes.Add("onclick", "return confirm('确定要删除吗?')");
            }
        }
    }
}
