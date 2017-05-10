using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.IO;
using System.Text.RegularExpressions;

namespace XkCms.DataOper.UI
{
	/// <summary>
	/// WebPage��ͨ�û��ࡣʵ����һЩ���ò�����
	/// </summary>
	public abstract class XkPage : System.Web.UI.Page
	{
        /// <summary>
        /// ����ϵͳĬ�ϵĴ���ҳ
        /// </summary>
        protected override void OnError(EventArgs e)
        {
            HttpContext ctx = HttpContext.Current;
            Exception exception = ctx.Server.GetLastError();
            string errorInfo =
                "<br>Offending URL: " + ctx.Request.Url.ToString() +
                "<br>Source: " + exception.Source +
                "<br>Message: " + exception.Message +
                "<br>Stack trace: " + exception.StackTrace;

            ctx.Response.Write(errorInfo);
            ctx.Server.ClearError();
            base.OnError(e);
        }

		/// <summary>
		/// ��ʾ���ݿ���ʶ���ͨ����Ҫ����һ��̳���ʵ��վ����ص�ͨ�ò���������ҳ����ʹ�á�
		/// </summary>
        public XkCms.DataOper.Data.DbOperHandler doh;
		/// <summary>
		/// ��ʵ�ֵ��������ݿ⺯����
		/// </summary>
		public abstract void ConnectDb();
		/// <summary>
		/// ����Sql Server���ݿ⡣
		/// </summary>
		/// <param name="serverName">��������ַ��</param>
		/// <param name="userName">�û�����</param>
		/// <param name="password">���롣</param>
		/// <param name="dataBaseName">���ݿ����ơ�</param>
		public void ConnectDb(string serverName,string userName,string password,string dataBaseName)
		{
			System.Data.SqlClient.SqlConnection sqlConn=new System.Data.SqlClient.SqlConnection("server='"+serverName+"';uid="+userName+";pwd="+password+";database=" + dataBaseName);
            doh = new XkCms.DataOper.Data.SqlDbOperHandler(sqlConn);
		}

		/// <summary>
		/// ҳ���ʼ����ͨ�ò���
		/// </summary>
		/// <param name="e"></param>
		override protected void OnInit(EventArgs e)
		{
			XkInit();
			base.OnInit(e);
		}

		/// <summary>
		/// ҳ���ʼ��
		/// </summary>
		protected virtual void XkInit()
		{
			this.Unload+=new EventHandler(Xkpage_Unload);
		}

		/// <summary>
		/// ���ӵ�һ��Access���ݿ⡣
		/// </summary>
		/// <param name="dataBase">���ݿ����ơ�</param>
		public void ConnectDb(string dataBase)
		{
			System.Data.OleDb.OleDbConnection oleConn=new System.Data.OleDb.OleDbConnection("provider=microsoft.jet.oledb.4.0;data source=" +this.Server.MapPath(dataBase));
            doh = new XkCms.DataOper.Data.OleDbOperHandler(oleConn);
		}

		/// <summary>
		/// �ڿͻ�����ʾ�����Ի���
		/// </summary>
		/// <param name="msg">Ҫ��ʾ����Ϣ��</param>
		public void Alert(string msg)
		{
            this.ClientScript.RegisterStartupScript(this.GetType(), "alert", "<script language=\"javascript\">alert('" + msg + "');</script>");
		}
		/// <summary>
		/// �ڿͻ�����ʾ�����Ի���
		/// </summary>
		/// <param name="name">�ű����ʶ����ͬһҳ��Ҫ��������������ʱ�費ͬ�ı�ʶ��������߻Ḳ��ǰ�ߡ�</param>
		/// <param name="msg">Ҫ��ʾ����Ϣ��</param>
		public void Alert(string name,string msg)
		{
            this.ClientScript.RegisterStartupScript(this.GetType(), name, "<script language=\"javascript\">alert('" + msg + "');</script>");
		}

		/// <summary>
		/// �ڿͻ���ִ��һ�νű���
		/// </summary>
		/// <param name="name">�ű����ʾ��</param>
		/// <param name="cmd">Ҫִ�е����</param>
		public void JsExe(string name,string cmd)
		{
            this.ClientScript.RegisterStartupScript(this.GetType(), name, "<script language=\"javascript\">" + cmd + ";</script>");
		}

		/// <summary>
		/// �ж���֤���Ƿ����Ҫ��
		/// </summary>
		/// <param name="code">�û��������֤��</param>
		/// <returns>������֤���Ƿ���ȷ</returns>
		public bool CheckValidateCode(string code)
		{
			try
			{
				if(code.ToUpper()!=Session["xk_validate_code"].ToString().ToUpper())return false;
				return true;
			}
			catch(Exception e)
			{
				e.ToString();
				return false;
			}
		}

		/// <summary>
		/// �滻html�е������ַ�
		/// </summary>
		/// <param name="theString">��Ҫ�����滻���ı���</param>
		/// <returns>�滻����ı���</returns>
		public string HtmlEncode(string theString)
		{
			theString=theString.Replace(">", "&gt;");
			theString=theString.Replace("<", "&lt;");
			theString=theString.Replace("  ", " &nbsp;");
			theString=theString.Replace("  ", " &nbsp;");
			theString=theString.Replace("\"", "&quot;");
			theString=theString.Replace("\'", "&#39;");
			theString=theString.Replace("\n", "<br/> ");
			return theString;
		}

		/// <summary>
		/// �ָ�html�е������ַ�
		/// </summary>
		/// <param name="theString">��Ҫ�ָ����ı���</param>
		/// <returns>�ָ��õ��ı���</returns>
		public string HtmlDiscode(string theString)
		{
			theString=theString.Replace("&gt;", ">");
			theString=theString.Replace("&lt;", "<");
			theString=theString.Replace("&nbsp;"," ");
			theString=theString.Replace(" &nbsp;","  ");
			theString=theString.Replace("&quot;","\"");
			theString=theString.Replace("&#39;","\'");
			theString=theString.Replace("<br/> ","\n");
			return theString;
		}

		/// <summary>
		/// ��ҳ����ڴ�ж��ʱ�������ر����ݿ�����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Xkpage_Unload(object sender, EventArgs e)
		{
			if(doh!=null)doh.Dispose();
		}
	}
}
