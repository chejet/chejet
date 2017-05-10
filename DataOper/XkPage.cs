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
	/// WebPage的通用基类。实现了一些常用操作。
	/// </summary>
	public abstract class XkPage : System.Web.UI.Page
	{
        /// <summary>
        /// 覆盖系统默认的错误页
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
		/// 表示数据库访问对象。通常需要另外一层继承来实现站点相关的通用操作后再在页面中使用。
		/// </summary>
        public XkCms.DataOper.Data.DbOperHandler doh;
		/// <summary>
		/// 待实现的连接数据库函数。
		/// </summary>
		public abstract void ConnectDb();
		/// <summary>
		/// 连接Sql Server数据库。
		/// </summary>
		/// <param name="serverName">服务器地址。</param>
		/// <param name="userName">用户名。</param>
		/// <param name="password">密码。</param>
		/// <param name="dataBaseName">数据库名称。</param>
		public void ConnectDb(string serverName,string userName,string password,string dataBaseName)
		{
			System.Data.SqlClient.SqlConnection sqlConn=new System.Data.SqlClient.SqlConnection("server='"+serverName+"';uid="+userName+";pwd="+password+";database=" + dataBaseName);
            doh = new XkCms.DataOper.Data.SqlDbOperHandler(sqlConn);
		}

		/// <summary>
		/// 页面初始化的通用操作
		/// </summary>
		/// <param name="e"></param>
		override protected void OnInit(EventArgs e)
		{
			XkInit();
			base.OnInit(e);
		}

		/// <summary>
		/// 页面初始化
		/// </summary>
		protected virtual void XkInit()
		{
			this.Unload+=new EventHandler(Xkpage_Unload);
		}

		/// <summary>
		/// 连接到一个Access数据库。
		/// </summary>
		/// <param name="dataBase">数据库名称。</param>
		public void ConnectDb(string dataBase)
		{
			System.Data.OleDb.OleDbConnection oleConn=new System.Data.OleDb.OleDbConnection("provider=microsoft.jet.oledb.4.0;data source=" +this.Server.MapPath(dataBase));
            doh = new XkCms.DataOper.Data.OleDbOperHandler(oleConn);
		}

		/// <summary>
		/// 在客户端显示弹出对话框。
		/// </summary>
		/// <param name="msg">要显示的信息。</param>
		public void Alert(string msg)
		{
            this.ClientScript.RegisterStartupScript(this.GetType(), "alert", "<script language=\"javascript\">alert('" + msg + "');</script>");
		}
		/// <summary>
		/// 在客户端显示弹出对话框。
		/// </summary>
		/// <param name="name">脚本块标识。当同一页面要调用两个弹出框时需不同的标识，否则后者会覆盖前者。</param>
		/// <param name="msg">要显示的信息。</param>
		public void Alert(string name,string msg)
		{
            this.ClientScript.RegisterStartupScript(this.GetType(), name, "<script language=\"javascript\">alert('" + msg + "');</script>");
		}

		/// <summary>
		/// 在客户端执行一段脚本。
		/// </summary>
		/// <param name="name">脚本框表示。</param>
		/// <param name="cmd">要执行的命令。</param>
		public void JsExe(string name,string cmd)
		{
            this.ClientScript.RegisterStartupScript(this.GetType(), name, "<script language=\"javascript\">" + cmd + ";</script>");
		}

		/// <summary>
		/// 判断验证码是否符合要求
		/// </summary>
		/// <param name="code">用户输入的验证码</param>
		/// <returns>返回验证码是否正确</returns>
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
		/// 替换html中的特殊字符
		/// </summary>
		/// <param name="theString">需要进行替换的文本。</param>
		/// <returns>替换完的文本。</returns>
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
		/// 恢复html中的特殊字符
		/// </summary>
		/// <param name="theString">需要恢复的文本。</param>
		/// <returns>恢复好的文本。</returns>
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
		/// 当页面从内存卸载时发生，关闭数据库连接
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Xkpage_Unload(object sender, EventArgs e)
		{
			if(doh!=null)doh.Dispose();
		}
	}
}
