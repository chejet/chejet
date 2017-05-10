using System;
using System.Data;
using System.Web;
using XkCms.WebForm.BaseFunction;
using System.Web.UI.WebControls;
using System.Collections;
using XkCms.Common.Utils;
using XkCms.Common.Web;

namespace XkCms.WebForm.Manager
{
    public partial class MakeHtml : Admin
    {
        private int total, current;
        private CreateHtml Html;
        protected void Page_Load(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 9999999;
            Xkzi_Load("", true, ref lblChannel);
            ChkPower(Channel.Id + "-11");

            if (!IsPostBack)
            {
                doh.Reset();
                doh.SqlCmd = "select id,title,code from [xk_Column] where ChannelId=" + Channel.Id + " order by code";
                DataTable dt = doh.GetDataTable();
                ListItem li;
                li = new ListItem("全部", "0");
                ddlColumn.Items.Add(li);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    li = new ListItem();
                    li.Value = dt.Rows[i][2].ToString();
                    li.Text = GetColumnListName(dt.Rows[i][1].ToString(), dt.Rows[i][2].ToString());
                    ddlColumn.Items.Add(li);
                }
            }
            else
                Html = new CreateHtml(true, doh, Channel.Id.ToString());
        }
        protected void btnMakeColumn_Click(object sender, EventArgs e)
        {
            SetProcessBar("生成列表页", true);
            makeColumn();
            ShowOk();
        }

        protected void btnMakeIndex_Click(object sender, EventArgs e)
        {
            makeIndex();
            ShowOk();
        }

        protected void btnMakeView_Click(object sender, EventArgs e)
        {
            SetProcessBar("生成内容页", true);
            makeView();
            ShowOk();
        }
        protected void btnId_Click(object sender, EventArgs e)
        {
            string Sid = txtId1.Text, Eid = txtId2.Text;
            string wSql = string.Empty;
            doh.Reset();
            doh.SqlCmd = "select * from xk_" + Channel.Type + " where channelid=" + Channel.Id + " and ispass=1";
            if (Sid != "" && Sid != "0")
            {
                if (Eid == "0" || Eid.Trim() == "")
                    wSql = " And id>=" + Sid;
                else
                    wSql = " And id between " + Sid + " and " + Eid;
            }
            else
                return;
            doh.SqlCmd += wSql;
            SetProcessBar("生成内容页", true);
            makeViewTemp(doh.GetDataTable());
            txtId1.Text = "";
            txtId2.Text = "";
            ShowOk();
        }

        protected void btnMakeMoreDiss_Click(object sender, EventArgs e)
        {
            SetProcessBar("生成过往专题页", true);
            makeMoreDiss();
            ShowOk();
        }
        protected void btnMakeDissList_Click(object sender, EventArgs e)
        {
            SetProcessBar("生成专题页", true);
            makeDissList();
            ShowOk();
        }

        protected void btnMakeAll_Click(object sender, EventArgs e)
        {
            makeIndex();
            SetProcessBar("生成过往专题页", true);
            makeMoreDiss();
            SetProcessBar("生成专题页", false);
            makeDissList();
            SetProcessBar("生成列表页", false);
            makeColumn();
            SetProcessBar("生成内容页", false);
            makeView();
            JsExe("ok", "alert('生成成功');$('progressbar').style.display='none'");
        }

        private void makeIndex()
        {
            DirFile.CreateFile(Channel.Dir + "/" + Cms.IndexName, Html.LoadChannelIndex());
        }
        private void makeMoreDiss()
        {
            ArrayList lst = Html.LoadMoreDiss();
            if (lst.Count > 2)
            {
                total = lst.Count - 2;
                for (int i = 2; i < lst.Count; i++)
                {
                    ProcessBar(i - 1, total);
                    string TempStr = lst[0].ToString().Replace(lst[1].ToString(), lst[i].ToString());
                    TempStr = TempStr.Replace("{$PageNumNav$}", HtmlPager.GetHtmlPager(lst.Count - 2, i - 1, "MoreDiss_", ".htm"));
                    DirFile.CreateFile(Channel.Dir + "/List/MoreDiss_" + (i - 1) + ".htm", TempStr);
                }
            }
        }
        private void makeDissList()
        {
            doh.Reset();
            doh.SqlCmd = "select * from [xk_diss] where channelid=" + Channel.Id;
            DataTable dt = doh.GetDataTable();
            total = dt.Rows.Count;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ArrayList lst = Html.LoadDiss(dt.Rows[i], 0);
                if (lst.Count > 2)
                {
                    int ct = (lst.Count - 2) * total;
                    current = i * (lst.Count - 2) + 1;
                    for (int j = 2; j < lst.Count; j++)
                    {
                        ProcessBar(current + j, ct);
                        string TempStr = lst[0].ToString().Replace(lst[1].ToString(), lst[j].ToString());
                        TempStr = TempStr.Replace("{$PageNumNav$}", HtmlPager.GetHtmlPager(lst.Count - 2, j - 1, "Diss_" + dt.Rows[i]["id"].ToString() + "_", ".htm"));
                        DirFile.CreateFile(Channel.Dir + "/List/Diss_" + dt.Rows[i]["id"].ToString() + "_" + (j - 1) + ".htm", TempStr);
                    }
                }
            }
        }
        private void makeColumn()
        {
            current = 0;
            doh.Reset();
            doh.SqlCmd = "select id from xk_column where channelid=" + Channel.Id;
            if (ddlColumn.SelectedValue != "0")
                doh.SqlCmd += " and code like '" + ddlColumn.SelectedValue + "%'";
            doh.SqlCmd += " order by code";
            DataTable dt = doh.GetDataTable();
            total = dt.Rows.Count;
            ArrayList lst;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lst = Html.LoadList(dt.Rows[i][0].ToString());
                if (lst.Count == 1)
                {
                    ProcessBar(i, total);
                    DirFile.CreateFile(Channel.Dir + "/List/" + dt.Rows[i][0].ToString() + "_1.htm", lst[0].ToString());
                }
                else
                {
                    int ct = (lst.Count - 2) * total;
                    current = i * (lst.Count - 2) + 1;
                    for (int j = 2; j < lst.Count; j++)
                    {
                        ProcessBar(current + j, ct);
                        string TempStr = lst[0].ToString().Replace(lst[1].ToString(), lst[j].ToString());
                        TempStr = TempStr.Replace("{$PageNumNav$}", HtmlPager.GetHtmlPager(lst.Count - 2, j - 1, dt.Rows[i][0].ToString() + "_", ".htm"));
                        DirFile.CreateFile(Channel.Dir + "/List/" + dt.Rows[i][0].ToString() + "_" + (j - 1) + ".htm", TempStr);
                    }
                }
            }
        }
        private void makeView()
        {
            doh.Reset();
            doh.SqlCmd = "select * from xk_" + Channel.Type + " where channelid=" + Channel.Id + " and ispass=1";
            if (ddlColumn.SelectedValue != "0")
                doh.SqlCmd += " and Columncode like '" + ddlColumn.SelectedValue + "%'";
            makeViewTemp(doh.GetDataTable());
        }

        private void makeViewTemp(DataTable dt)
        {
            total = dt.Rows.Count;
            for (int i = 0; i < total; i++)
            {
                ProcessBar(i + 1, total);
                DirFile.CreateFile(Channel.Dir + "/View/" + dt.Rows[i]["ColumnId"].ToString() + "/" + dt.Rows[i]["id"].ToString() + ".htm", Html.LoadView(dt.Rows[i]));
            }
        }

        private void ShowOk()
        {
            JsExe("成功", "alert('生成成功');$('progressbar').style.display='none'");
        }
    }
}
