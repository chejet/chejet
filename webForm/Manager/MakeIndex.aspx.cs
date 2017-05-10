using System;
using System.Data;
using System.Web.UI.WebControls;
using XkCms.WebForm.BaseFunction;
using XkCms.Common.Utils;

namespace XkCms.WebForm.Manager
{
    public partial class MakeIndex : Admin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Xkzi_Load("009");
            DirFile.CreateFile(Cms.IndexName, new CreateHtml(true, doh, string.Empty).LoadIndex());
            doh.Reset();
            doh.SqlCmd = "select id,dir from xk_channel where IsOut=0 And Enabled=1";
            DataTable dt = doh.GetDataTable();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DirFile.CreateFile(dt.Rows[i][1].ToString() + "/" + Cms.IndexName, new CreateHtml(true, doh, dt.Rows[i][0].ToString()).LoadChannelIndex());
            }
            Response.Write("生成首页及各频道首页成功");
        }
    }
}
