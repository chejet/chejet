using System;
using System.Data;
using System.Drawing.Imaging;
using XkCms.DataOper.Data;
using XkCms.DataOper.UI;
using System.Web.UI.WebControls;

namespace XkCms.WebForm.BaseFunction
{
    /// <summary>
    /// BasicPage ��ժҪ˵��
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
        /// �������ݿ�
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
        /// ��ʼ��ϵͳ��Ϣ
        /// </summary>
        protected void SetupSystemDate()
        {
            Cms = new SysInfo(doh);
            Application.Lock();
            Application["XkCms"] = Cms;
            Application.UnLock();
        }

        /// <summary>
        /// �������������Ŀ��
        /// </summary>
        /// <param name="sName">��Ŀ��</param>
        /// <param name="sCode">��Ŀcode</param>
        /// <returns>����������Ŀ��</returns>
        protected string GetColumnListName(string sName, string sCode)
        {
            int Level = (sCode.Length / 4 - 1) * 2;
            string sStr = sCode.Length == 4 ? "" : "��";
            if (Level > 0)
            {
                for (int i = 0; i < Level; i++)
                    sStr += "��";
            }
            return sStr + sName;
        }

        /// <summary>
        /// ��ȡquerystring
        /// </summary>
        /// <param name="s">������</param>
        /// <returns>����ֵ</returns>
        public string q(string s)
        {
            if (Request.QueryString[s] != null)
            {
                return Request.QueryString[s].ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// ��ȡpost�õ��Ĳ���
        /// </summary>
        /// <param name="s">������</param>
        /// <returns>����ֵ</returns>
        protected string f(string s)
        {
            if (Request.Form[s] == null) return string.Empty;
            return Request.Form[s];
        }

        /// <summary>
        /// ΪGridView����ӹ���
        /// </summary>
        /// <param name="e">��</param>
        /// <param name="c">"ɾ��"LinkButton���ڵ�Ԫ��</param>
        /// <param name="w">"ɾ��"LinkButtonΪ��Ԫ���еڼ����ؼ�</param>
        protected void GvRowAddFun(ref GridViewRowEventArgs e, int c, int w)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='E4E8EF'");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=''");
                if (w > 0)
                    ((LinkButton)(e.Row.Cells[c].Controls[w])).Attributes.Add("onclick", "return confirm('ȷ��Ҫɾ����?')");
            }
        }
    }
}
