using System;
using System.Data;
using System.Web;
using XkCms.WebForm.BaseFunction;
using System.Web.UI.WebControls;
using System.IO;
using XkCms.Common.Utils;

namespace XkCms.WebForm
{
    public partial class Setup : Admin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            doh.Reset();
            doh.SqlCmd = "select dir,id,type from xk_channel where isout=0";
            DataTable dt = doh.GetDataTable();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DirFile.CreateDir(dt.Rows[i][0].ToString());
                DirFile.CreateDir(dt.Rows[i][0].ToString() + "/List");
                DirFile.CreateDir(dt.Rows[i][0].ToString() + "/View");
                DirFile.CreateDir(dt.Rows[i][0].ToString() + "/UserFiles");
                DirFile.CreateDir(dt.Rows[i][0].ToString() + "/Js");

                CopyFiles(dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString());
            }
            doh.Reset();
            doh.AddFieldItem("templateid", 0);
            doh.AddFieldItem("templatediss", 0);
            doh.Update("xk_channel");
            doh.Reset();
            doh.AddFieldItem("templateid", 0);
            doh.AddFieldItem("contentTemp", 0);
            doh.Update("xk_column");

            CreateMenu();

            Response.Write("初始化完成 <a href=" + Request.ApplicationPath + ">进入</a>, 请自行删除Setup.aspx文件");
        }

        protected void CopyFiles(string cDir, string TempId)
        {
            
            string TempStr = string.Empty;
            StreamWriter sw;
            if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\Default.aspx"))
            {
                TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\Default.aspx");
                TempStr = TempStr.Replace("{$ChannelId$}", TempId);

                sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\Default.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
                sw.Write(TempStr);
                sw.Close();
            }
            if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\Column.aspx"))
            {
                TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\Column.aspx");
                TempStr = TempStr.Replace("{$ChannelId$}", TempId);

                sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\List.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
                sw.Write(TempStr);
                sw.Close();
            }
            if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\Content.aspx"))
            {
                TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\Content.aspx");
                TempStr = TempStr.Replace("{$ChannelId$}", TempId);

                sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\View.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
                sw.Write(TempStr);
                sw.Close();
            }
            if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\MoreDiss.aspx"))
            {
                TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\MoreDiss.aspx");
                TempStr = TempStr.Replace("{$ChannelId$}", TempId);

                sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\DissList.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
                sw.Write(TempStr);
                sw.Close();
            }
            if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\ShowDiss.aspx"))
            {
                TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\ShowDiss.aspx");
                TempStr = TempStr.Replace("{$ChannelId$}", TempId);

                sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\Diss.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
                sw.Write(TempStr);
                sw.Close();
            }
            if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\Review.aspx"))
            {
                TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\Review.aspx");
                TempStr = TempStr.Replace("{$ChannelId$}", TempId);

                sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\Review.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
                sw.Write(TempStr);
                sw.Close();
            }
            if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\Search.aspx"))
            {
                TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\Search.aspx");
                TempStr = TempStr.Replace("{$ChannelId$}", TempId);

                sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\Search.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
                sw.Write(TempStr);
                sw.Close();
            }
        }
    }
}
