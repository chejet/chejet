using System;
using System.Web;
using XkCms.WebForm.BaseFunction;
using System.Data;

namespace XkCms.WebForm.Manager
{
    public partial class Reload : Admin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Xkzi_Load("001");
            if (dbType == "1")
            {
                Button5.Enabled = false;
                txtMdbName.Enabled = false;
                btnBackup.Enabled = false;
                btnRestore.Enabled = false;
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            SetupSystemDate();
            Alert("更新完成");
        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            doh.Reset();
            doh.SqlCmd = "select id,type from [xk_channel] where isout=0";
            DataTable dt = doh.GetDataTable();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                doh.Reset();
                doh.ConditionExpress = "ChannelId=" + dt.Rows[i][0].ToString();
                int topicnum = doh.GetCount("Xk_" + dt.Rows[i][1].ToString(), "id");
                doh.Reset();
                doh.ConditionExpress = "ChannelId=" + dt.Rows[i][0].ToString();
                int reviewnum = doh.GetCount("Xk_Review", "id");
                doh.Reset();
                doh.ConditionExpress = "id=" + dt.Rows[i][0].ToString();
                doh.AddFieldItem("TopicNum", topicnum);
                doh.AddFieldItem("ReviewNum", reviewnum);
                doh.Update("Xk_Channel");

                doh.Reset();
                doh.SqlCmd = "select id,title,code from Xk_Column where ChannelId=" + dt.Rows[i][0].ToString();
                DataTable dt2 = doh.GetDataTable();
                for (int j = 0; j < dt2.Rows.Count; j++)
                {
                    doh.Reset();
                    doh.ConditionExpress = "ColumnId=" + dt2.Rows[j][0].ToString();
                    doh.AddFieldItem("ColumnName", dt2.Rows[j][1].ToString());
                    doh.AddFieldItem("ColumnCode", dt2.Rows[j][2].ToString());
                    doh.Update("Xk_" + dt.Rows[i][1].ToString());
                }
            }
            Alert("更新完成");
        }
        protected void Button4_Click(object sender, EventArgs e)
        {
            CreateMenu();
            Alert("更新完成");
        }
        protected void Button3_Click(object sender, EventArgs e)
        {
            int UserTotal, ReviewTotal, ArticleTotal, SoftTotal, PhotoTotal;
            doh.Reset();
            UserTotal = doh.GetCount("Xk_User", "id");
            doh.Reset();
            ReviewTotal = doh.GetCount("Xk_Review", "id");
            doh.Reset();
            ArticleTotal = doh.GetCount("Xk_Article", "id");
            doh.Reset();
            SoftTotal = doh.GetCount("Xk_Soft", "id");
            doh.Reset();
            PhotoTotal = doh.GetCount("Xk_Photo", "id");

            doh.Reset();
            doh.AddFieldItem("RegUser", UserTotal);
            doh.AddFieldItem("ArticleNum", ArticleTotal);
            doh.AddFieldItem("SoftNum", SoftTotal);
            doh.AddFieldItem("PhotoNum", PhotoTotal);
            doh.AddFieldItem("ReviewNum", ReviewTotal);
            doh.Update("xk_system");

            doh.Reset();
            doh.SqlCmd = "select Grades from Xk_UserGroup";
            DataTable dt = doh.GetDataTable();
            int tmp;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                tmp = 0;
                doh.Reset();
                doh.ConditionExpress = "UserGrade=" + dt.Rows[i][0].ToString();
                tmp = doh.GetCount("Xk_User", "id");
                doh.Reset();
                doh.ConditionExpress = "Grades=" + dt.Rows[i][0].ToString();
                doh.AddFieldItem("UserTotal", tmp);
                doh.Update("Xk_UserGroup");
            }
            Alert("更新完成");
        }
        protected void Button5_Click(object sender, EventArgs e)
        {
            System.Data.OleDb.OleDbConnection strConn = (System.Data.OleDb.OleDbConnection)doh.GetConnection();
            doh.Dispose();
            object[] oParams;
            object objJRO = Activator.CreateInstance(Type.GetTypeFromProgID("JRO.JetEngine"));
            string tempDb = "CompactDatabase-temp-" + DateTime.Now.ToShortDateString() + ".mdb";
            string tempPath = strConn.ConnectionString.Substring(strConn.ConnectionString.IndexOf("Data Source=") + 12, strConn.ConnectionString.LastIndexOf("\\") - strConn.ConnectionString.IndexOf("Data Source=") - 12);
            string dbName = strConn.ConnectionString.Substring(strConn.ConnectionString.LastIndexOf("\\"));
            oParams = new object[] { strConn.ConnectionString, strConn.ConnectionString.Substring(0, strConn.ConnectionString.LastIndexOf('\\')) + "\\" + tempDb + ";Jet OLEDB:Engine Type=5" };
            try
            {
                objJRO.GetType().InvokeMember("CompactDatabase", System.Reflection.BindingFlags.InvokeMethod, null, objJRO, oParams);
                if (System.IO.File.Exists(tempPath + "\\" + tempDb))
                    System.IO.File.Copy(tempPath + "\\" + tempDb, tempPath + "\\" + dbName, true);
            }
            catch (Exception)
            {
                Alert("压缩数据库,请确保没有其它用户连接数据库!");
                return;
            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(objJRO);
            objJRO = null;
            if (System.IO.File.Exists(tempPath + "\\" + tempDb))
                System.IO.File.Delete(tempPath + "\\" + tempDb);
            Alert("操作完成!");
        }
        protected void btnBackup_Click(object sender, EventArgs e)
        {
            System.Data.OleDb.OleDbConnection strConn = (System.Data.OleDb.OleDbConnection)doh.GetConnection();
            doh.Dispose();
            string tempPath = strConn.ConnectionString.Substring(strConn.ConnectionString.IndexOf("Data Source=") + 12, strConn.ConnectionString.LastIndexOf("\\") - strConn.ConnectionString.IndexOf("Data Source=") - 12);
            string dbName = strConn.ConnectionString.Substring(strConn.ConnectionString.LastIndexOf("\\"));
            try
            {
                System.IO.File.Copy(tempPath + "\\" + dbName, tempPath + "\\" + txtMdbName.Text, true);
            }
            catch (Exception)
            {
                Alert("数据库正在被使用,请稍后再试!");
                return;
            }
            Alert("备份完成!");
        }
        protected void btnRestore_Click(object sender, EventArgs e)
        {
            System.Data.OleDb.OleDbConnection strConn = (System.Data.OleDb.OleDbConnection)doh.GetConnection();
            doh.Dispose();
            string tempPath = strConn.ConnectionString.Substring(strConn.ConnectionString.IndexOf("Data Source=") + 12, strConn.ConnectionString.LastIndexOf("\\") - strConn.ConnectionString.IndexOf("Data Source=") - 12);
            string dbName = strConn.ConnectionString.Substring(strConn.ConnectionString.LastIndexOf("\\"));
            if (System.IO.File.Exists(tempPath + "\\" + txtMdbName.Text))
            {
                try
                {
                    System.IO.File.Copy(tempPath + "\\" + txtMdbName.Text, tempPath + "\\" + dbName, true);
                    Alert("还原成功!");
                }
                catch (Exception)
                {
                    Alert("数据库正在被使用,请稍后再试!");
                    return;
                }
            }
            else
                Alert(txtMdbName.Text + "不存在!");
        }
    }
}
