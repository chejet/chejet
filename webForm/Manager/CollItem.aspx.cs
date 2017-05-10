using System;
using System.Data;
using System.Web;
using XkCms.WebForm.BaseFunction;
using System.Web.UI.WebControls;
using System.Collections;

namespace XkCms.WebForm.Manager
{
    public partial class CollItem : Admin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 99999999;
            if (!IsPostBack)
            {
                plAdd.Visible = false;
                plList.Visible = false;
                plSetLink.Visible = false;
                plSetList.Visible = false;
                plSetContent.Visible = false;
                plTest.Visible = false;
            }
            Admin_Nav();
            ChkPower("021");
            string Action = q("action");
            switch (Action)
            {
                case "add":
                    plAdd.Visible = true;
                    editBox();
                    break;
                case "setList":
                    plSetList.Visible = true;
                    setList();
                    break;
                case "setLink":
                    plSetLink.Visible = true;
                    setLink();
                    break;
                case "setContent":
                    plSetContent.Visible = true;
                    setContent();
                    break;
                case "test":
                    plTest.Visible = true;
                    if (GetTest(2))
                    {
                        doh.Reset();
                        doh.ConditionExpress = "id=" + id;
                        doh.AddFieldItem("flag", 1);
                        doh.Update("xk_CollItem");
                    }
                    break;
                case "start":
                    GetTest(3);
                    break;
                default:
                    plList.Visible = true;
                    getListBox();
                    break;
            }
        }

        protected override void getListBox()
        {
            doh.Reset();
            doh.SqlCmd = "select a.id as id,a.ItemName as title,webname,b.Title as ChannelName,ColumnName,flag from xk_CollItem a left join xk_channel b on a.channelid=b.id";
            gvList.DataSource = doh.GetDataTable();
            gvList.DataKeyNames = new string[] { "id" };
            gvList.DataBind();
        }
        public string chkFlag(string flag)
        {
            if (flag == "1")
                return "√";
            else
                return "<font color=red>×</font>";

        }
        protected override void editBox()
        {
            XkCms.DataOper.Data.WebFormHandler wh = new XkCms.DataOper.Data.WebFormHandler(doh, "xk_CollItem", btnStep1);
            wh.AddBind(txtTitle, "ItemName", true);
            wh.AddBind(ddlChannel, "ChannelId", false);
            wh.AddBind(txtPageSize, "pagesize", false);
            wh.AddBind(ddlColumn, "ColumnId", false);
            wh.AddBind(ddlDiss, "DisId", false);
            wh.AddBind(ddlEncode, "WebEncode", false);
            wh.AddBind(txtWebName, "WebName", true);
            wh.AddBind(txtWebUrl, "WebUrl", true);
            wh.AddBind(txtItemDemo, "ItemDemo", true);

            wh.AddBind(txtViewNum, "hits", false);
            wh.AddBind(txtAuthor, "AuthorStr", true);
            wh.AddBind(ddlFrom, "SourceId", false);
            wh.AddBind(chkA, "1", "Script_A", false);
            wh.AddBind(chkTable, "1", "Script_Table", false);
            wh.AddBind(chkDiv, "1", "Script_Div", false);
            wh.AddBind(chkFont, "1", "Script_Font", false);
            wh.AddBind(chkHtml, "1", "Script_Html", false);
            wh.AddBind(chkIframe, "1", "Script_Iframe", false);
            wh.AddBind(chkImg, "1", "Script_Img", false);
            wh.AddBind(chkObject, "1", "Script_Object", false);
            wh.AddBind(chkScript, "1", "Script_Script", false);
            wh.AddBind(chkSpan, "1", "Script_Span", false);
            wh.AddBind(chkIsDesc, "1", "CollecOrder", false);
            wh.AddBind(chkSaveImg, "1", "SaveFiles", false);
            wh.AddBind(txtTopNum, "CollecNewsNum", false);
            if (id == "0")
                wh.Mode = XkCms.DataOper.OperationType.Add;
            else
            {
                wh.ConditionExpress = "id=" + id;
                wh.Mode = XkCms.DataOper.OperationType.Modify;
            }
            wh.AddOk += new EventHandler(wh_AddOk);
            wh.ModifyOk += new EventHandler(wh_AddOk);
            wh.validator = chkStep1;
            wh.BindBeforeModifyOk += new EventHandler(wh_BindBeforeModifyOk);
            if (!IsPostBack)
            {
                ddlChannel.Items.Clear();
                ddlChannel.Items.Add(new ListItem("请选择", "0"));
                doh.Reset();
                doh.SqlCmd = "select id,title from [xk_channel] where enabled=1 and isout=0 and type<>'Soft' order by pid";
                ddlChannel.DataSource = doh.GetDataTable();
                ddlChannel.AutoPostBack = true;
                ddlChannel.AppendDataBoundItems = true;
                ddlChannel.DataTextField = "title";
                ddlChannel.DataValueField = "id";
                ddlChannel.DataBind();
            }
        }

        void wh_BindBeforeModifyOk(object sender, EventArgs e)
        {
            ddlChannel.SelectedItem.Selected = false;
            ddlChannel.Items.FindByValue("0").Selected = true;
        }

        void wh_AddOk(object sender, EventArgs e)
        {
            doh.Reset();
            doh.ConditionExpress = "id=" + ddlColumn.SelectedValue;
            string code = doh.GetValue("Xk_Column", "code").ToString();
            string cName = doh.GetValue("Xk_Column", "title").ToString();
            doh.Reset();
            if (id == "0")
            {
                XkCms.DataOper.Data.DbOperEventArgs de = (XkCms.DataOper.Data.DbOperEventArgs)e;
                id = de.id.ToString();
                doh.AddFieldItem("Flag", 0);
            }
            doh.ConditionExpress = "id=" + id.ToString();
            doh.AddFieldItem("ColumnCode", code);
            doh.AddFieldItem("ColumnName", cName);
            doh.Update("xk_CollItem");
            Response.Redirect("CollItem.aspx?action=setList&id=" + id);
        }
        private bool chkStep1()
        {
            if (ddlChannel.SelectedValue == "0" || ddlColumn.SelectedValue == "0")
            {
                Alert("请选择频道和栏目!");
                return false;
            }
            return true;
        }
        protected void ddlChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlColumn.Items.Clear();
            ddlColumn.Items.Add(new ListItem("请选择", "0"));
            ddlColumn.AppendDataBoundItems = true;
            doh.Reset();
            doh.SqlCmd = "select id,title,code from xk_column where channelid=" + ddlChannel.SelectedValue + " and isout=0 order by code";
            DataTable dt = doh.GetDataTable();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ListItem li = new ListItem();
                li.Value = dt.Rows[i][0].ToString();
                li.Text = GetColumnListName(dt.Rows[i][1].ToString(), dt.Rows[i][2].ToString());
                ddlColumn.Items.Add(li);
            }
            ddlDiss.Items.Clear();
            ddlDiss.Items.Add(new ListItem("请选择", "0"));
            ddlDiss.AppendDataBoundItems = true;
            doh.Reset();
            doh.SqlCmd = "select id,title from xk_diss where channelid=" + ddlChannel.SelectedValue;
            ddlDiss.DataSource = doh.GetDataTable();
            ddlDiss.DataTextField = "title";
            ddlDiss.DataValueField = "id";
            ddlDiss.DataBind();
            ddlFrom.Items.Clear();
            ddlFrom.Items.Add(new ListItem("请选择", "0"));
            ddlFrom.AppendDataBoundItems = true;
            doh.Reset();
            doh.SqlCmd = "select id,title from [xk_source] where channelid=" + ddlChannel.SelectedValue + " or channelid=0";
            ddlFrom.DataSource = doh.GetDataTable();
            ddlFrom.DataTextField = "title";
            ddlFrom.DataValueField = "id";
            ddlFrom.DataBind();
        }
        protected void gvList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            doh.Reset();
            doh.ConditionExpress = "itemid=" + gvList.DataKeys[e.RowIndex].Value;
            doh.Delete("xk_Collection");
            doh.Reset();
            doh.ConditionExpress = "id=" + gvList.DataKeys[e.RowIndex].Value;
            doh.Delete("xk_CollItem");
            getListBox();
        }
        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GvRowAddFun(ref e, 6, 1);
        }
        private void setList()
        {
            XkCms.DataOper.Data.WebFormHandler wh = new XkCms.DataOper.Data.WebFormHandler(doh, "xk_CollItem", btnSetList);
            wh.AddBind(txtListIndex, "ListStr", true);
            wh.AddBind(txtListStart, "LsString", true);
            wh.AddBind(txtListEnd, "LoString", true);
            wh.ConditionExpress = "id=" + id;
            wh.Mode = XkCms.DataOper.OperationType.Modify;
            wh.ModifyOk += new EventHandler(setListOk);
        }

        void setListOk(object sender, EventArgs e)
        {
            Response.Redirect("CollItem.aspx?action=setLink&id=" + id);
        }

        private void setLink()
        {
            if (!GetTest(0))
                btnSetLink.Enabled = false;
            XkCms.DataOper.Data.WebFormHandler wh = new XkCms.DataOper.Data.WebFormHandler(doh, "xk_CollItem", btnSetLink);
            wh.AddBind(txtLinkStart, "HsString", true);
            wh.AddBind(txtLinkEnd, "HoString", true);
            wh.ConditionExpress = "id=" + id;
            wh.Mode = XkCms.DataOper.OperationType.Modify;
            wh.ModifyOk += new EventHandler(setLinkOk);
        }

        void setLinkOk(object sender, EventArgs e)
        {
            Response.Redirect("CollItem.aspx?action=setContent&id=" + id);
        }

        private bool GetTest(int testType)
        {
            if (id == "0")
            {
                Alert("参数错误，项目ID不能为 0!");
                return false;
            }
            doh.Reset();
            doh.SqlCmd = "select * from xk_collItem where id=" + id;
            DataTable dt = doh.GetDataTable();
            if (dt.Rows.Count == 0)
            {
                Alert("ID为 " + id + " 的项目不存在!");
                return false;
            }
            int CollecNewsNum = Convert.ToInt32("0" + dt.Rows[0]["CollecNewsNum"].ToString());
            string ListStr = dt.Rows[0]["ListStr"].ToString();
            string LsString = dt.Rows[0]["LsString"].ToString();
            string LoString = dt.Rows[0]["LoString"].ToString();
            string HsString = dt.Rows[0]["HsString"].ToString();
            string HoString = dt.Rows[0]["HoString"].ToString();
            string CollecOrder = dt.Rows[0]["CollecOrder"].ToString();
            string weburl = dt.Rows[0]["WebUrl"].ToString();
            string WebEncode = dt.Rows[0]["WebEncode"].ToString();
            string TsString = dt.Rows[0]["TsString"].ToString();
            string ToString = dt.Rows[0]["ToString"].ToString();
            string CsString = dt.Rows[0]["CsString"].ToString();
            string CoString = dt.Rows[0]["CoString"].ToString();
            string NPsString = dt.Rows[0]["NPsString"].ToString();
            string NPoString = dt.Rows[0]["NPoString"].ToString();
            string SaveFiles = dt.Rows[0]["SaveFiles"].ToString();
            string Script_Iframe = dt.Rows[0]["Script_Iframe"].ToString();
            string Script_Object = dt.Rows[0]["Script_Object"].ToString();
            string Script_Script = dt.Rows[0]["Script_Script"].ToString();
            string Script_Div = dt.Rows[0]["Script_Div"].ToString();
            string Script_Table = dt.Rows[0]["Script_Table"].ToString();
            string Script_Span = dt.Rows[0]["Script_Span"].ToString();
            string Script_Img = dt.Rows[0]["Script_Img"].ToString();
            string Script_Font = dt.Rows[0]["Script_Font"].ToString();
            string Script_A = dt.Rows[0]["Script_A"].ToString();
            string Script_Html = dt.Rows[0]["Script_Html"].ToString();

            doh.Reset();
            doh.ConditionExpress = "id=" + dt.Rows[0]["ChannelId"].ToString();
            string SaveDir = doh.GetValue("xk_Channel", "dir").ToString();
            System.Text.Encoding enType = System.Text.Encoding.Default;
            switch (WebEncode)
            {
                case "3":
                    enType = System.Text.Encoding.Unicode;
                    break;
                case "2":
                    enType = System.Text.Encoding.UTF8;
                    break;
                case "1":
                    enType = System.Text.Encoding.GetEncoding("gb2312");
                    break;
            }
            NewsCollection nc = new NewsCollection();
            string testList = nc.GetHttpPage(ListStr, 10000, enType);
            if (testList == "$UrlIsFalse$")
            {
                Alert("列表地址设置错误");
                return false;
            }
            if (testList == "$GetFalse$")
            {
                Alert("无法连接列表页或连接超时");
                return false;
            }
            testList = nc.GetBody(testList, LsString, LoString, false, false);
            if (testList == "$StartFalse$")
            {
                Alert("列表开始标记设置错误,请重新设置");
                return false;
            }
            if (testList == "$EndFalse$")
            {
                Alert("列表结束标记设置错误,请重新设置");
                return false;
            }
            if (testType == 0)
                ltListTest.Text = testList;
            else
            {
                ArrayList linkArray = nc.GetArray(testList, HsString, HoString);
                if (linkArray.Count == 0)
                {
                    Alert("未取到链接,请检查链接设置");
                    return false;
                }
                else
                {
                    if (linkArray[0].ToString() == "$StartFalse$")
                    {
                        Alert("链接开始标记设置错误,请重新设置");
                        return false;
                    }
                    if (linkArray[0].ToString() == "$EndFalse$")
                    {
                        Alert("链接开始标记设置错误,请重新设置");
                        return false;
                    }
                    if (linkArray[0].ToString() == "$NoneLink$")
                    {
                        Alert("未取到链接,请检查链接设置");
                        return false;
                    }
                    if (CollecOrder == "1")
                        linkArray.Reverse();
                    if (CollecNewsNum > 0 && linkArray.Count > CollecNewsNum)
                        linkArray.RemoveRange(CollecNewsNum, linkArray.Count - CollecNewsNum);

                    if (weburl.LastIndexOf('/') + 1 < weburl.Length)
                        weburl += "/";
                    string linkStr = string.Empty;
                    if (testType == 1)
                    {
                        for (int i = 0; i < linkArray.Count; i++)
                        {
                            linkStr = nc.DefiniteUrl(linkArray[i].ToString(), weburl);
                            if (linkStr != "$False$")
                            {
                                linkStr = "<a href='" + linkStr + "' target=_blank>" + linkStr + "</a><br>";
                                ltLinkTest.Text += linkStr;
                            }
                        }
                    }
                    else if (testType == 2)
                    {
                        linkStr = nc.DefiniteUrl(linkArray[0].ToString(), weburl);
                        if (linkStr == "$False$")
                        {
                            Alert("获取到的链接地址无效,请检查链接设置");
                            return false;
                        }
                        string newsCode = nc.GetHttpPage(linkStr, 10000, enType);
                        if (newsCode == "$UrlIsFalse$")
                        {
                            Alert("获取到的链接地址无效,请检查链接设置");
                            return false;
                        }
                        if (newsCode == "$GetFalse$")
                        {
                            Alert("无法连接内容页或连接超时");
                            return false;
                        }
                        string cTitle = nc.GetBody(newsCode, TsString, ToString, false, false);
                        string cBody = nc.GetBody(newsCode, CsString, CoString, false, false);
                        if (cTitle == "$StartFalse$" || cBody == "$StartFalse$")
                        {
                            Alert("标题或正文开始标记设置错误,请重新设置");
                            return false;
                        }
                        if (cTitle == "$EndFalse$" || cBody == "$EndFalse$")
                        {
                            Alert("标题或正文结束标记设置错误,请重新设置");
                            return false;
                        }
                        ltTestTitle.Text = cTitle;
                        ArrayList bodyArray = nc.ReplaceSaveRemoteFile(cBody, Request.PhysicalApplicationPath + "\\" + SaveDir, Cms.Dir + SaveDir, weburl, "0");
                        ltTestContent.Text = bodyArray[0].ToString();
                        if (bodyArray.Count == 3)
                        {
                            ltPhotoUrl.Text = bodyArray[2].ToString();
                        }
                    }
                    else
                    {
                        int falseNum = 0;
                        int successNum = 0;
                        SetProcessBar("从" + weburl + "采集信息", true);
                        for (int i = 0; i < linkArray.Count; i++)
                        {
                            int isImg = 0;
                            string imgUrl = string.Empty;
                            string photoUrl = string.Empty;

                            ProcessBar(i, linkArray.Count);
                            linkStr = nc.DefiniteUrl(linkArray[i].ToString(), weburl);
                            if (linkStr == "$False$")
                            {
                                falseNum++;
                                continue;
                            }
                            string newsCode = nc.GetHttpPage(linkStr, 8000, enType);
                            if (newsCode == "$UrlIsFalse$" || newsCode == "$GetFalse$")
                            {
                                falseNum++;
                                continue;
                            }
                            string cTitle = nc.GetBody(newsCode, TsString, ToString, false, false);
                            string cBody = nc.GetBody(newsCode, CsString, CoString, false, false);
                            if (cTitle == "$StartFalse$" || cBody == "$StartFalse$" || cTitle == "$EndFalse$" || cBody == "$EndFalse$")
                            {
                                falseNum++;
                                continue;
                            }
                            string NewsPaingNext = nc.GetPaing(newsCode, NPsString, NPoString);
                            int PageCount = 0;
                            while (NewsPaingNext != "$StartFalse$" && NewsPaingNext != "$EndFalse$" && NewsPaingNext.Length > 0 && PageCount < 20)
                            {
                                string NewsPaingNextCode = string.Empty;
                                string ContentTemp = string.Empty;
                                NewsPaingNext = nc.DefiniteUrl(NewsPaingNext, weburl);
                                NewsPaingNextCode = nc.GetHttpPage(NewsPaingNext, 10000, enType);
                                ContentTemp = nc.GetBody(NewsPaingNextCode, CsString, CoString, false, false);
                                if (ContentTemp == "$StartFalse$" || ContentTemp == "$EndFalse$")
                                    break;
                                else
                                {
                                    cBody = cBody + "<br>[Xkzi_PageBreak]<br>" + ContentTemp;
                                    NewsPaingNext = nc.GetPaing(NewsPaingNextCode, NPsString, NPoString);
                                }
                                PageCount++;
                            }

                            ArrayList bodyArray = nc.ReplaceSaveRemoteFile(cBody, Request.PhysicalApplicationPath + "\\" + SaveDir, Cms.Dir + SaveDir, weburl, SaveFiles);
                            if (bodyArray.Count == 3)
                            {
                                isImg = 1;
                                imgUrl = bodyArray[1].ToString();
                                photoUrl = bodyArray[2].ToString();
                            }
                            cBody = bodyArray[0].ToString();

                            //过滤开始
                            if (Script_Iframe == "1")
                                cBody = nc.ScriptHtml(cBody, "Iframe", 1);
                            if (Script_Object == "1")
                                cBody = nc.ScriptHtml(cBody, "Object", 2);
                            if (Script_Script == "1")
                                cBody = nc.ScriptHtml(cBody, "Script", 2);
                            if (Script_Div == "1")
                                cBody = nc.ScriptHtml(cBody, "Div", 3);
                            if (Script_Table == "1")
                                cBody = nc.ScriptHtml(cBody, "Table", 2);
                            if (Script_Span == "1")
                                cBody = nc.ScriptHtml(cBody, "Span", 3);
                            if (Script_Img == "1")
                                cBody = nc.ScriptHtml(cBody, "Img", 3);
                            if (Script_Font == "1")
                                cBody = nc.ScriptHtml(cBody, "Font", 3);
                            if (Script_A == "1")
                                cBody = nc.ScriptHtml(cBody, "A", 3);
                            if (Script_Html == "1")
                                cBody = nc.NoHtml(cBody);
                            //过滤结束

                            doh.Reset();
                            doh.AddFieldItem("id", i + 1);
                            doh.AddFieldItem("title", cTitle);
                            doh.AddFieldItem("content", cBody);
                            doh.AddFieldItem("isimg", isImg);
                            doh.AddFieldItem("img", imgUrl);
                            doh.AddFieldItem("ItemId", id);
                            doh.AddFieldItem("photoUrl", photoUrl);
                            doh.Insert("Xk_Collection");
                            successNum++;
                        }
                        JsExe("ok", "alert('采集完成,成功 " + successNum + " ,失败 " + falseNum + "');window.location='CollHistory.aspx'");
                    }
                }
            }
            return true;
        }

        private void setContent()
        {
            if (!GetTest(1))
                btnSetContent.Enabled = false;
            XkCms.DataOper.Data.WebFormHandler wh = new XkCms.DataOper.Data.WebFormHandler(doh, "xk_CollItem", btnSetContent);
            wh.AddBind(txtTitleStart, "TsString", true);
            wh.AddBind(txtTitleEnd, "ToString", true);
            wh.AddBind(txtContentStart, "CsString", true);
            wh.AddBind(txtContentEnd, "CoString", true);
            wh.AddBind(txtNPageStart, "NPsString", true);
            wh.AddBind(txtNPageEnd, "NPoString", true);
            wh.ConditionExpress = "id=" + id;
            wh.Mode = XkCms.DataOper.OperationType.Modify;
            wh.ModifyOk += new EventHandler(setContentOk);
        }

        void setContentOk(object sender, EventArgs e)
        {
            Response.Redirect("CollItem.aspx?action=test&id=" + id);
        }

        protected void LinkButton2_Command(object sender, CommandEventArgs e)
        {
            doh.Reset();
            doh.SqlCmd = "select * from xk_collitem where id=" + e.CommandArgument.ToString();
            DataTable dt = doh.GetDataTable();
            if (dt.Rows.Count > 0)
            {
                doh.Reset();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName.ToLower() != "id")
                    {
                        doh.AddFieldItem(dt.Columns[i].ColumnName, dt.Rows[0][i].ToString());
                    }
                }
                doh.Insert("xk_CollItem");
            }
            Response.Redirect("CollItem.aspx");
        }
    }
}
