using System;
using System.Data;
using System.Web;
using XkCms.WebForm.BaseFunction;
using System.Web.UI.WebControls;

namespace XkCms.WebForm.Manager
{
    public partial class CollHistory : Admin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Xkzi_Load("002");
        }

        protected override void getListBox()
        {
            plList.Visible = true;
            plEdit.Visible = false;
            doh.Reset();
            doh.SqlCmd = "select a.id as id,a.title as title,ItemName from [xk_Collection] a left join [xk_CollItem] b on a.ItemId=b.id order by a.id desc";
            gvList.DataSource = doh.GetDataTable();
            gvList.DataKeyNames = new string[] { "id" };
            gvList.PageSize = 20;
            gvList.DataBind();
        }
        protected override void editBox()
        {
            plEdit.Visible = true;
            plList.Visible = false;
            doh.Reset();
            doh.ConditionExpress = "id=" + id;
            lblTitle.Text = doh.GetValue("xk_Collection", "title").ToString();
            Literal1.Text = doh.GetValue("xk_Collection", "content").ToString();
            Literal2.Text = doh.GetValue("xk_Collection", "photourl").ToString();
        }
        protected void gvList_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            GvRowAddFun(ref e, 3, 1);
        }
        protected void gvList_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            doh.Reset();
            doh.ConditionExpress = "id=" + gvList.DataKeys[e.RowIndex].Value;
            doh.Delete("xk_Collection");
            getListBox();
        }
        protected void btnMove_Click(object sender, EventArgs e)
        {
            doh.Reset();
            doh.SqlCmd = "select * from xk_collItem where flag=1";
            DataTable dt = doh.GetDataTable();
            DateTime AddDate = DateTime.Now;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string ChId, CoId, Ccode, CName, Author, DisId, SourceId, ViewNum, PageSize, ChType;
                ChId = dt.Rows[i]["ChannelId"].ToString();
                CoId = dt.Rows[i]["ColumnId"].ToString();
                Ccode = dt.Rows[i]["ColumnCode"].ToString();
                CName = dt.Rows[i]["ColumnName"].ToString();
                Author = dt.Rows[i]["AuthorStr"].ToString();
                SourceId = dt.Rows[i]["SourceId"].ToString();
                ViewNum = dt.Rows[i]["Hits"].ToString();
                DisId = dt.Rows[i]["disid"].ToString();
                PageSize = dt.Rows[i]["pagesize"].ToString();

                doh.Reset();
                doh.ConditionExpress = "id=" + ChId;
                ChType = doh.GetValue("xk_channel", "type").ToString();
                if ("ArticlePhoto".IndexOf(ChType) < 0)
                    continue;
                doh.Reset();
                doh.ConditionExpress = "id=" + CoId;
                if (!doh.Exist("Xk_Column"))
                    continue;
                doh.Reset();
                doh.SqlCmd = "select title,content,isimg,img,photourl from [xk_Collection] where itemid=" + dt.Rows[i]["id"].ToString();
                DataTable dt2 = doh.GetDataTable();
                if (ChType == "Article")
                {
                    for (int j = 0; j < dt2.Rows.Count; j++)
                    {
                        AddDate = AddDate.AddSeconds(1);
                        doh.Reset();
                        doh.AddFieldItem("title", dt2.Rows[j][0].ToString());
                        doh.AddFieldItem("subtitle", "");
                        doh.AddFieldItem("tcolor", "");
                        doh.AddFieldItem("outurl", "");
                        doh.AddFieldItem("ChannelId", ChId);
                        doh.AddFieldItem("ColumnId", CoId);
                        doh.AddFieldItem("ColumnCode", Ccode);
                        doh.AddFieldItem("ColumnName", CName);
                        doh.AddFieldItem("AddDate", AddDate.ToString());
                        doh.AddFieldItem("Content", dt2.Rows[j][1].ToString());
                        doh.AddFieldItem("ViewNum", ViewNum);
                        doh.AddFieldItem("ReviewNum", 0);
                        doh.AddFieldItem("Author", Author);
                        doh.AddFieldItem("UserId", 0);
                        doh.AddFieldItem("IsPass", 1);
                        doh.AddFieldItem("IsImg", dt2.Rows[j][2].ToString());
                        doh.AddFieldItem("img", dt2.Rows[j][3].ToString());
                        doh.AddFieldItem("istop", 0);
                        doh.AddFieldItem("DisId", DisId);
                        doh.AddFieldItem("isout", 0);
                        doh.AddFieldItem("SourceId", SourceId);
                        doh.AddFieldItem("KeyWord", CreateKeyWord(dt2.Rows[j][0].ToString()));
                        doh.AddFieldItem("Summary", dt2.Rows[j][0].ToString());
                        doh.Insert("Xk_Article");
                    }
                }
                else
                {
                    for (int j = 0; j < dt2.Rows.Count; j++)
                    {
                        AddDate = AddDate.AddSeconds(1);
                        doh.Reset();
                        doh.AddFieldItem("title", dt2.Rows[j][0].ToString());
                        doh.AddFieldItem("tcolor", "");
                        doh.AddFieldItem("ChannelId", ChId);
                        doh.AddFieldItem("ColumnId", CoId);
                        doh.AddFieldItem("ColumnCode", Ccode);
                        doh.AddFieldItem("ColumnName", CName);
                        doh.AddFieldItem("pagesize", PageSize);
                        doh.AddFieldItem("AddDate", AddDate.ToString());
                        doh.AddFieldItem("ViewNum", ViewNum);
                        doh.AddFieldItem("ReviewNum", 0);
                        doh.AddFieldItem("UserId", 0);
                        doh.AddFieldItem("Author", Author);
                        doh.AddFieldItem("IsPass", 1);
                        doh.AddFieldItem("IsImg", dt2.Rows[j][2].ToString());
                        doh.AddFieldItem("img", dt2.Rows[j][3].ToString());
                        doh.AddFieldItem("istop", 0);
                        doh.AddFieldItem("DisId", DisId);
                        doh.AddFieldItem("SourceId", SourceId);
                        doh.AddFieldItem("KeyWord", CreateKeyWord(dt2.Rows[j][0].ToString()));
                        doh.AddFieldItem("PhotoUrl", dt2.Rows[j][4].ToString());
                        doh.AddFieldItem("Summary", dt2.Rows[j][0].ToString());
                        doh.Insert("Xk_Photo");
                    }
                }
                doh.Reset();
                doh.ConditionExpress = "itemid=" + dt.Rows[i]["id"].ToString();
                doh.Delete("xk_collection");
            }
            Alert("²Ù×÷³É¹¦");
            getListBox();
        }

        private string CreateKeyWord(string Constr)
        {
            Constr = Constr.Trim();
            Constr = Constr.Replace("&nbsp;", "");
            Constr = Constr.Replace(" ", "");
            Constr = Constr.Replace("£¡", "");
            Constr = Constr.Replace("!", "");
            Constr = Constr.Replace("(", "");
            Constr = Constr.Replace(")", "");
            Constr = Constr.Replace("<", "");
            Constr = Constr.Replace(">", "");
            Constr = Constr.Replace("[", "");
            Constr = Constr.Replace("]", "");
            Constr = Constr.Replace("{", "");
            Constr = Constr.Replace("}", "");
            Constr = Constr.Replace(">", "");
            Constr = Constr.Replace("\"", "");
            Constr = Constr.Replace("?", "");
            Constr = Constr.Replace("*", "");
            Constr = Constr.Replace("|", "");
            Constr = Constr.Replace(",", "");
            Constr = Constr.Replace(".", "");
            Constr = Constr.Replace("'", "");
            Constr = Constr.Replace(";", "");
            Constr = Constr.Replace(":", "");
            Constr = Constr.Replace("£º", "");
            Constr = Constr.Replace("£»", "");
            Constr = Constr.Replace("£¬", "");
            Constr = Constr.Replace("¡£", "");
            Constr = Constr.Replace("£¿", "");
            Constr = Constr.Replace("¡¢", "");
            Constr = Constr.Replace("¡°", "");
            Constr = Constr.Replace("¡¯", "");
            Constr = Constr.Replace("-", "");
            Constr = Constr.Replace("¡ª¡ª", "");

            string TempStr = string.Empty;
            for (int i = 0; i < Constr.Length - 2; i++)
                TempStr += "," + Constr.Substring(i, 2);
            if (TempStr.Length > 50)
                TempStr = TempStr.Substring(1, 48);
            else
                TempStr = TempStr.Substring(1);
            return TempStr;
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            doh.Reset();
            doh.Delete("Xk_Collection");
            getListBox();
        }
    }
}
