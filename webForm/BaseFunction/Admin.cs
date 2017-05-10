using System;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;
using System.IO;
using XkCms.Common.Utils;


namespace XkCms.WebForm.BaseFunction
{
    public class Admin : BasicPage
    {
        public string ChannelId = "0";

        protected ChannelInfo Channel = new ChannelInfo();
        protected string id = "0";

        protected string Master_Power = string.Empty;
        protected string MasterName, MasterCookies;
        protected int pageSize = 20;

        #region 页面加载
        /// <summary>
        /// 加载页面数据,如js,css
        /// </summary>
        protected void Admin_Nav()
        {
            ChannelId = q("ChannelId");
            id = q("id");
            if (id == "" || !XkCms.Common.Utils.Validator.IsNumberId(id)) id = "0";
            Response.Write("<link href='images/style.css' type=text/css rel=stylesheet>");
            Response.Write("<script type='text/javascript' src='../js/prototype.js'></script>");
            Response.Write("<script type='text/javascript' src='../js/jsdate.js'></script>");
            Response.Write("<script language='JavaScript' src='../Js/admin.js'></SCRIPT>");
            if (ChannelId != "-1")
                Response.Write("<script language='JavaScript' src='../Js/toolTip.js'></SCRIPT>");
            if (Session["FCKeditor:UserFilesPath"] == null)
                Session.Add("FCKeditor:UserFilesPath", Cms.Dir + "UserFiles");
            if (Session["Xkzi_Cms:AllowUserUpload"] == null)
                Session.Add("Xkzi_Cms:AllowUserUpload", Cms.MasterUpload);
            else
                Session["Xkzi_Cms:AllowUserUpload"] = Cms.MasterUpload;
            if (ChannelId == "" || !XkCms.Common.Utils.Validator.IsNumberId(ChannelId)) ChannelId = "0";
            ChkLogin();
        }

        /// <summary>
        /// 后台管理初始
        /// </summary>
        /// <param name="powerNum">权限</param>
        protected void Xkzi_Load(string powerNum)
        {
            Admin_Nav();
            ChkPower(powerNum); 
            string Action = q("action");
            if (Action == "add")
                editBox();
            else if(Action == "")
            {
                if (!IsPostBack)
                    getListBox();
            }
        }

        /// <summary>
        /// 后台管理初始
        /// </summary>
        /// <param name="powerNum">权限</param>
        /// <param name="isChannel">是否和频道相关</param>
        /// <param name="lblChannel">显示频道名的Label</param>
        protected void Xkzi_Load(string powerNum, bool isChannel, ref Label lblChannel)
        {
            Admin_Nav();
            ChkPower(powerNum);
            if (isChannel && ChannelId == "0")
            {
                Alert("参数错误,请不要生外部提交数据!");
                return;
            }
            if (ChannelId != "0")
            {
                Channel = new ChannelInfo(ChannelId, doh);
                lblChannel.Text = Channel.Title;
                Session["FCKeditor:UserFilesPath"] = Cms.Dir + Channel.Dir + "/UserFiles";
            }
            else
            {
                lblChannel.Text = "系统管理";
                Session["FCKeditor:UserFilesPath"] = Cms.Dir + "UserFiles";
            }
            string Action = q("action");
            if (Action == "add")
                editBox();
            else
            {
                if (!IsPostBack)
                    getListBox();
            }
        }

        /// <summary>
        /// 列表内容通用方法,必须重写
        /// </summary>
        protected virtual void getListBox() { }
        /// <summary>
        /// 编辑内容通用方法,必须重写
        /// </summary>
        protected virtual void editBox() { }
        #endregion

        #region 内容编辑相关
        /// <summary>
        /// 取得文章,图片,下载列表
        /// </summary>
        /// <param name="cid">栏目Id</param>
        /// <param name="keyType">搜索关键字类型{author,title,summary}</param>
        /// <param name="keyWord">搜索关键字</param>
        /// <param name="sDate">日期{1d=今天,1w=本周,1m=本月}</param>
        /// <param name="isEdit">是否审核{0=未审核,1=已审核,-1=已删除,2=全部}</param>
        /// <param name="page">页数</param>
        /// <param name="cType">内容类型{Article,Photo,Soft}</param>
        /// <param name="gvList">显示列表的GridView</param>
        /// <param name="ddlColumn">操作用的栏目DropDownList</param>
        /// <param name="ddlKeyColumn">查询用的栏目DropDownList</param>
        /// <param name="ltContentPager">显示分页的Literal</param>
        protected void GetContentList(int cid, string keyType, string keyWord, string sDate, int isEdit, int page, ref GridView gvList, ref DropDownList ddlColumn, ref DropDownList ddlKeyColumn, ref Literal ltPager)
        {
            int countNum = 0;
            int pageCount = 1;
            page = page == 0 ? 1 : page;

            string whereStr = "";
            if (sDate == "")
            {
                if (isEdit != 2)
                    whereStr = "ispass=" + isEdit.ToString() + " and ";
            }
            else
            {
                if (dbType == "0")
                {
                    switch (sDate)
                    {
                        case "1d":
                            whereStr = "datediff('d',adddate,'" + DateTime.Now.ToShortDateString() + "')=0";
                            break;
                        case "1w":
                            whereStr = "datediff('ww',adddate,'" + DateTime.Now.ToShortDateString() + "')=0";
                            break;
                        case "1m":
                            whereStr = "datediff('m',adddate,'" + DateTime.Now.ToShortDateString() + "')=0";
                            break;
                    }
                }
                else
                {
                    switch (sDate)
                    {
                        case "1d":
                            whereStr = "datediff(d,adddate,'" + DateTime.Now.ToShortDateString() + "')=0";
                            break;
                        case "1w":
                            whereStr = "datediff(ww,adddate,'" + DateTime.Now.ToShortDateString() + "')=0";
                            break;
                        case "1m":
                            whereStr = "datediff(m,adddate,'" + DateTime.Now.ToShortDateString() + "')=0";
                            break;
                    }
                }
                whereStr += " and ";
            }

            if (cid > 0)
                whereStr = "columnid=" + cid.ToString() + " and ";
            if (keyType.Length > 0)
                whereStr += keyType + " like '%" + keyWord + "%' and ";

            doh.Reset();
            doh.ConditionExpress = whereStr + "ChannelId=" + Channel.Id;
            gvList.DataSource = doh.GetDataTable("xk_" + Channel.Type, "id,title,ChannelId,columnName,ispass,isTop,ReviewNum,viewNum", "id", true, "id", page, pageSize, ref countNum);
            gvList.DataKeyNames = new string[] { "id" };
            gvList.DataBind();
            pageCount = countNum % pageSize == 0 ? countNum / pageSize : countNum / pageSize + 1;
            string[] FiledName = new string[] { "ChannelId", "cid", "k", "w", "e", "d" };
            string[] FiledValue = new string[] { Channel.Id.ToString(), cid.ToString(), keyType, keyWord, isEdit.ToString(), sDate };
            ltPager.Text = XkCms.Common.Web.HtmlPager.GetPager(pageCount, page, FiledName, FiledValue);

            if (ddlColumn.Items.Count == 0)
            {
                doh.Reset();
                doh.SqlCmd = "select id,title,code from [xk_Column] where ChannelId=" + Channel.Id + " and isout=0 order by code";
                DataTable dt = doh.GetDataTable();
                ListItem li;
                li = new ListItem("全部", "0");
                ddlKeyColumn.Items.Add(li);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    li = new ListItem();
                    li.Value = dt.Rows[i][0].ToString();
                    li.Text = GetColumnListName(dt.Rows[i][1].ToString(), dt.Rows[i][2].ToString());
                    ddlColumn.Items.Add(li);
                    ddlKeyColumn.Items.Add(li);
                }
            }
        }

        /// <summary>
        /// 执行内容的移动,审核,删除等操作
        /// </summary>
        /// <param name="oper">操作类型{pass=审核,nopass=未审核,move=移动,sdel=移入回收站,del=直接删除}</param>
        /// <param name="ids">id字符串,以","串联起来</param>
        /// <param name="cid">要移入到栏目id</param>
        /// <param name="cType">内容类型{Article,Photo,Soft}</param>
        protected void BatchContent(string oper, string ids, int cid)
        {
            ChkPower(Channel.Id + "-2");
            string[] idValue;
            idValue = ids.Split(',');
            string ColumnId = string.Empty;
            if (oper == "pass")
            {
                for (int i = 0; i < idValue.Length; i++)
                {
                    doh.Reset();
                    doh.ConditionExpress = "id=" + idValue[i];
                    doh.AddFieldItem("ispass", 1);
                    doh.Update("Xk_" + Channel.Type);
                }
            }
            else if (oper == "nopass")
            {
                for (int i = 0; i < idValue.Length; i++)
                {
                    doh.Reset();
                    doh.ConditionExpress = "id=" + idValue[i];
                    doh.AddFieldItem("ispass", 0);
                    doh.Update("Xk_" + Channel.Type);
                }
            }
            else if (oper == "move")
            {
                doh.Reset();
                doh.ConditionExpress = "id=" + cid.ToString();
                string code = doh.GetValue("Xk_Column", "code").ToString();
                string cName = doh.GetValue("Xk_Column", "title").ToString();
                string[] oldCId = new string[idValue.Length];
                for (int i = 0; i < idValue.Length; i++)
                {
                    doh.Reset();
                    doh.ConditionExpress = "id=" + idValue[i];
                    oldCId[i] = doh.GetValue("Xk_" + Channel.Type, "ColumnId").ToString();
                    doh.Reset();
                    doh.ConditionExpress = "id=" + idValue[i];
                    doh.AddFieldItem("columnName", cName);
                    doh.AddFieldItem("columnCode", code);
                    doh.AddFieldItem("columnId", cid);
                    doh.Update("Xk_" + Channel.Type);
                }
                if (Cms.IsHtml)
                {
                    for (int i = 0; i < idValue.Length; i++)
                    {
                        if (oldCId[i] != cid.ToString())
                            DirFile.DeleteFile(Channel.Dir + "\\View\\" + oldCId[i] + "\\" + idValue[i] + ".htm");
                    }
                }
            }
            else if (oper == "sdel")
            {
                for (int i = 0; i < idValue.Length; i++)
                {
                    doh.Reset();
                    doh.ConditionExpress = "id=" + idValue[i];
                    doh.AddFieldItem("ispass", -1);
                    doh.Update("Xk_" + Channel.Type);
                }
            }
            else if (oper == "del")
            {
                int reviewnum = 0;
                for (int i = 0; i < idValue.Length; i++)
                {
                    doh.Reset();
                    doh.ConditionExpress = "id=" + idValue[i];
                    string cId = doh.GetValue("Xk_" + Channel.Type, "ColumnId").ToString();
                    doh.Reset();
                    doh.ConditionExpress = "id=" + idValue[i];
                    doh.Delete("Xk_" + Channel.Type);
                    doh.Reset();
                    doh.ConditionExpress = "cId=" + idValue[i];
                    reviewnum += doh.Delete("xk_Review");
                    DirFile.DeleteFile(Channel.Dir + "\\View\\" + cId + "\\" + idValue[i] + ".htm");
                }
                Channel.TopicNum -= idValue.Length;
                doh.Reset();
                doh.ConditionExpress = "id=" + Channel.Id;
                doh.AddFieldItem("TopicNum", Channel.TopicNum);
                doh.Update("Xk_Channel");
                SetCmsNum(0, false, idValue.Length);
                SetCmsNum(2, false, reviewnum);
            }
        }

        /// <summary>
        /// 添加编辑内容之后的操作
        /// </summary>
        /// <param name="id">被编辑内容的id</param>
        /// <param name="column">所在栏目的id</param>
        /// <param name="txtImg">标题图片textbox</param>
        /// <param name="channel">所在频道的id</param>
        /// <param name="cType">内容类型{Article,Soft,Photo}</param>
        /// <param name="e">EventArgs</param>
        protected void DataAdded(string column, TextBox txtImg, EventArgs e)
        {
            doh.Reset();
            doh.ConditionExpress = "id=" + column;
            string code = doh.GetValue("Xk_Column", "code").ToString();
            string cName = doh.GetValue("Xk_Column", "title").ToString();
            bool isAdd = false;
            if (id == "0")
            {
                doh.Reset();
                doh.ConditionExpress = "id=" + Channel.Id;
                doh.Count("Xk_Channel", "TopicNum");
                Channel.TopicNum++;
                SetCmsNum(0, true, 1);
                XkCms.DataOper.Data.DbOperEventArgs de = (XkCms.DataOper.Data.DbOperEventArgs)e;
                id = de.id.ToString();
                isAdd = true;
            }
            doh.Reset();
            doh.ConditionExpress = "id=" + id;
            doh.AddFieldItem("ColumnCode", code);
            doh.AddFieldItem("ColumnName", cName);
            if (txtImg.Text != "")
                doh.AddFieldItem("isimg", "1");
            else
                doh.AddFieldItem("isimg", "0");
            if (isAdd)
            {
                doh.AddFieldItem("UserId", 0);
                doh.AddFieldItem("ReviewNum", 0);
            }
            doh.Update("Xk_" + Channel.Type);
        }

        /// <summary>
        /// 删除内容时的操作
        /// </summary>
        /// <param name="id">被删除内容的id</param>
        /// <param name="cType">内容类型{Article,Soft,Photo}</param>
        protected void DataDeled(string sid)
        {
            ChkPower(Channel.Id + "-2");
            doh.Reset();
            doh.ConditionExpress = "id=" + sid;
            string cId = doh.GetValue("xk_" + Channel.Type, "ColumnId").ToString();
            //删除评论
            doh.Reset();
            doh.ConditionExpress = "cId=" + sid;
            int reviewnum = doh.Delete("Xk_Review");
            //删除内容
            doh.Reset();
            doh.ConditionExpress = "id=" + sid;
            doh.Delete("Xk_" + Channel.Type);
            //重计频道
            doh.Reset();
            doh.ConditionExpress = "id=" + Channel.Id;
            doh.Substract("Xk_Channel", "TopicNum");
            Channel.TopicNum--;
            Channel.ReviewNum -= reviewnum;
            doh.Reset();
            doh.ConditionExpress = "id=" + Channel.Id;
            doh.AddFieldItem("ReviewNum", Channel.ReviewNum);
            doh.Update("Xk_Channel");
            //重计系统
            SetCmsNum(0, false, 1);
            SetCmsNum(2, false, reviewnum);

            DirFile.DeleteFile(Channel.Dir + "\\View\\" + cId + "\\" + sid + ".htm");
        }

        /// <summary>
        /// 编辑内容时,向栏目专题标题颜色等DropDownList中添加内容,同时添加用于添加标题图片用的js函数
        /// </summary>
        /// <param name="ddlTcolor">颜色</param>
        /// <param name="ddlContentColumn">栏目</param>
        /// <param name="ddlSource">来源</param>
        /// <param name="ddlDiss">专题</param>
        /// <param name="txtImg">标题图片</param>
        protected void GetEditDropDownList(ref DropDownList ddlTcolor, ref DropDownList ddlContentColumn, ref DropDownList ddlSource, ref DropDownList ddlDiss, ref FredCK.FCKeditorV2.FCKeditor FCKeditor1)
        {
            ChkPower(Channel.Id + "-1");
            FCKeditor1.BasePath = Cms.EditorDir;
            FCKeditor1.ManagerPath = Cms.ManagerDir;
            DataTable dt;
            if (!IsPostBack)
            {
                string[] tcolor = new string[] { "#000000", "#FFFFFF", "#008000", "#800000", "#808000", "#000080", "#800080", "#808080", "#FFFF00", "#00FF00", "#00FFFF", "#FF00FF", "#FF0000", "#0000FF", "#008080" };
                ddlTcolor.Items.Add(new ListItem("标题颜色", ""));
                for (int i = 0; i < tcolor.Length; i++)
                {
                    ddlTcolor.Items.Add(new ListItem(tcolor[i], tcolor[i]));
                    ddlTcolor.Items[i + 1].Attributes.Add("style", "background-color: " + tcolor[i] + ";color: " + tcolor[i]);
                }
                doh.Reset();
                doh.SqlCmd = "select id,title,code from [xk_Column] where ChannelId=" + Channel.Id + " and isout=0 order by code";
                dt = doh.GetDataTable();
                if (dt.Rows.Count == 0)
                {
                    JsExe("提示", "alert('请先添加栏目!');window.location='Column.aspx?ChannelId=" + Channel.Id + "'");
                    return;
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlContentColumn.Items.Add(new ListItem(GetColumnListName(dt.Rows[i][1].ToString(), dt.Rows[i][2].ToString()), dt.Rows[i][0].ToString()));
                }
                doh.Reset();
                doh.SqlCmd = "select id,title from [xk_Source] where ChannelId=" + Channel.Id + " or ChannelId=0";
                dt = doh.GetDataTable();
                ddlSource.Items.Add(new ListItem("本站原创", "0"));
                ddlSource.AppendDataBoundItems = true;
                ddlSource.DataSource = dt;
                ddlSource.DataTextField = "title";
                ddlSource.DataValueField = "id";
                ddlSource.DataBind();

                doh.Reset();
                doh.SqlCmd = "select id,title from [xk_Diss] where ChannelId=" + Channel.Id;
                dt = doh.GetDataTable();
                ddlDiss.Items.Add(new ListItem("不属于专题", "0"));
                ddlDiss.AppendDataBoundItems = true;
                ddlDiss.DataSource = dt;
                ddlDiss.DataTextField = "title";
                ddlDiss.DataValueField = "id";
                ddlDiss.DataBind();
            }
            JsExe("AddDateReadOnly", "$('txtAddDate').readOnly=true");
        }

        protected void SetCmsNum(int fld, bool isCount, int num)
        {
            if (num < 1) return;
            if (!isCount) num -= 2 * num;
            doh.Reset();
            if (fld == 1)
            {
                Cms.RegUser += num;
                doh.AddFieldItem("RegUser", Cms.RegUser);
            }
            else if (fld == 2)
            {
                Cms.ReviewNum += num;
                doh.AddFieldItem("ReviewNum", Cms.ReviewNum);
            }
            else
            {
                if (Channel.Type.ToLower() == "article")
                {
                    Cms.ArticleNum += num;
                    doh.AddFieldItem("ArticleNum", Cms.ArticleNum);
                }
                else if (Channel.Type.ToLower() == "soft")
                {
                    Cms.SoftNum += num;
                    doh.AddFieldItem("SoftNum", Cms.SoftNum);
                }
                else if (Channel.Type.ToLower() == "photo")
                {
                    Cms.PhotoNum += num;
                    doh.AddFieldItem("PhotoNum", Cms.PhotoNum);
                }
            }
            doh.Update("Xk_System");
        }
        #endregion

        #region 其它
        /// <summary>
        /// 管理菜单
        /// </summary>
        /// <returns></returns>
        protected string[,] leftMenu()
        {
            doh.Reset();
            doh.SqlCmd = "select id,title,pId,[Type],ItemName from [xk_Channel] where Enabled=1 and isOut=0 order by pId";
            DataTable dt = doh.GetDataTable();
            //下面的'$'后面的'0'表示系统
            string[,] menu = new string[3 + dt.Rows.Count, 12];
            menu[0, 0] = "系统管理$0";
            menu[0, 1] = "<a href='ConfigSet.aspx' target='fmain'>系统设定</a> | <a href='Reload.aspx' target='fmain'>数据</a>";
            menu[0, 2] = "<a href='Friend.aspx' target='fmain'>友情链接</a> | <a href='Friend.aspx?action=add' target='fmain'>添加</a>";
            menu[0, 3] = "<a href='Placard.aspx' target='fmain'>公告管理</a> | <a href='Placard.aspx?action=add' target='fmain'>添加</a>";
            menu[0, 4] = "<a href='Vote.aspx' target='fmain'>投票管理</a> | <a href='Vote.aspx?action=add' target='fmain'>添加</a>";
            menu[0, 5] = "<a href='Source.aspx' target='fmain'>来源管理</a> | <a href='Source.aspx?action=add' target='fmain'>添加</a>";
            menu[0, 6] = "<a href='Channel.aspx' target='fmain'>频道管理</a> | <a href='Channel.aspx?action=add' target='fmain'>添加</a>";
            menu[0, 7] = "<a href='TempProject.aspx' target='fmain'>模板方案</a> | <a href='TempLabel.aspx' target='fmain'>标签</a>";
            menu[0, 8] = "<a href='ContentTemplate.aspx' target='fmain'>内容模板</a> | <a href='ContentTemplate.aspx?action=add' target='fmain'>添加</a>";
            menu[0, 9] = "<a href='MakeIndex.aspx' target='fmain'>生成各频道首页</a>";

            menu[1, 0] = "用户管理$0";
            menu[1, 1] = "<a href='Master.aspx' target='fmain'>管理员管理</a> | <a href='Master.aspx?action=add' target='fmain'>添加</a>";
            menu[1, 2] = "<a href='Users.aspx' target='fmain'>会员管理</a> | <a href='Users.aspx?action=add' target='fmain'>添加</a>";
            menu[1, 3] = "<a href='UserGroup.aspx' target='fmain'>用户组管理</a> | <a href='UserGroup.aspx?action=add' target='fmain'>添加</a>";

            menu[2, 0] = "内容采集$0";
            menu[2, 1] = "<a href='CollItem.aspx' target='fmain'>采集管理</a> | <a href='CollItem.aspx?action=add' target='fmain'>添加</a>";
            menu[2, 2] = "<a href='CollHistory.aspx' target='fmain'>数据处理</a>";
            string mChannelId = string.Empty;
            int j = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                j = i + 3;
                mChannelId = dt.Rows[i][0].ToString();
                menu[j, 0] = dt.Rows[i][1].ToString() + "管理$" + mChannelId;//这个'$'后面是频道的id
                menu[j, 1] = "<a href='" + dt.Rows[i][3].ToString() + ".aspx?action=add&ChannelId=" + mChannelId + "' target='fmain'>" + dt.Rows[i][4].ToString() + "添加</a>";
                menu[j, 2] = "<a href='" + dt.Rows[i][3].ToString() + ".aspx?ChannelId=" + mChannelId + "' target='fmain'>" + dt.Rows[i][4].ToString() + "管理</a>";
                menu[j, 3] = "<a href='Column.aspx?ChannelId=" + mChannelId + "' target='fmain'>栏目管理</a> | <a href='Column.aspx?action=add&ChannelId=" + mChannelId + "' target='fmain'>添加</a>";
                menu[j, 4] = "<a href='Channel.aspx?action=add&ChannelId=" + mChannelId + "' target='fmain'>" + dt.Rows[i][1].ToString() + "设置</a>";
                menu[j, 5] = "<a href='Review.aspx?ChannelId=" + mChannelId + "' target='fmain'>评论管理</a>";
                menu[j, 6] = "<a href='Diss.aspx?ChannelId=" + mChannelId + "' target='fmain'>专题管理</a> | <a href='Diss.aspx?action=add&ChannelId=" + mChannelId + "' target='fmain'>添加</a>";
                menu[j, 7] = "<a href='Source.aspx?ChannelId=" + mChannelId + "' target='fmain'>来源管理</a> | <a href='Source.aspx?action=add&ChannelId=" + mChannelId + "' target='fmain'>添加</a>";
                menu[j, 8] = "<a href='Friend.aspx?ChannelId=" + mChannelId + "' target='fmain'>友情链接</a> | <a href='Friend.aspx?action=add&ChannelId=" + mChannelId + "' target='fmain'>添加</a>";
                menu[j, 9] = "<a href='Placard.aspx?ChannelId=" + mChannelId + "' target='fmain'>公告管理</a> | <a href='Placard.aspx?action=add&ChannelId=" + mChannelId + "' target='fmain'>添加</a>";
                menu[j, 10] = "<a href='Vote.aspx?ChannelId=" + mChannelId + "' target='fmain'>投票管理</a> | <a href='Vote.aspx?action=add&ChannelId=" + mChannelId + "' target='fmain'>添加</a>";
                menu[j, 11] = "<a href='MakeHtml.aspx?ChannelId=" + mChannelId + "' target='fmain'>生成HTML管理</a>";
            }

            return menu;
        }

        /// <summary>
        /// 验证登陆
        /// </summary>
        public void ChkLogin()
        {
            if (!IsLogin())
                ShowErrMsg();
        }

        public bool IsLogin()
        {
            ErrMsg = string.Empty;
            if (Session["master_id"] == null || Session["master_name"] == null)
                ErrMsg = "未登陆或登陆超时!";
            else
            {
                MasterName = Session["master_name"].ToString();
                MasterCookies = Session["master_id"].ToString();

                if (MasterName.Length == 0 || MasterCookies.Length == 0)
                    ErrMsg = "未登陆或登陆超时2!";
                else
                {
                    doh.Reset();
                    doh.ConditionExpress = "master_name=@name and cookiess=@cook";
                    doh.AddConditionParameter("@name", MasterName);
                    doh.AddConditionParameter("@cook", MasterCookies);
                    Master_Power = doh.GetValue("xk_master", "setting").ToString();

                    if (Master_Power.Length == 0)
                        ErrMsg = "你没有任何权限,请重新登陆试一下!";
                }
            }
            if (ErrMsg.Length > 0)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 创建频道导航菜单
        /// </summary>
        protected void CreateMenu()
        {
            System.IO.StreamWriter sw;
            System.Text.StringBuilder sb;
            System.Text.StringBuilder sb_s;
            string ChannelMenu = "document.write(\"<a href='" + Cms.Dir + "'>首页</a>";
            string js = "";
            string siteurl = string.Empty;
            DataTable dt, dt2, dt3, dtChannel;

            doh.Reset();
            doh.SqlCmd = "select id,title,dir,target,isout,outurl from xk_channel where enabled=1";
            dtChannel = doh.GetDataTable();

            for (int c = 0; c < dtChannel.Rows.Count; c++)
            {
                string CId = dtChannel.Rows[c][0].ToString();
                siteurl = Cms.Dir + dtChannel.Rows[c][2].ToString() + "/";
                ChannelMenu += " | <a href='" + siteurl + "'";
                if (dtChannel.Rows[c][3].ToString() == "1")
                    ChannelMenu += " target='_blank'";
                ChannelMenu += ">" + dtChannel.Rows[c][1].ToString() + "</a>";
                sb = new System.Text.StringBuilder();
                sb_s = new System.Text.StringBuilder();
                sb.Append("mpmenu1=new mMenu('频道首页','" + siteurl + "Default.aspx','self','','','','');");
                sb.Append("mpmenu1.addItem(new mMenuItem('<img src=" + Cms.Dir + "images/ye.gif> 站内搜索','" + siteurl + "Search.aspx','blank',false,'站内搜索',null,'','','',''));");
                sb.Append("mpmenu1.addItem(new mMenuItem('<img src=" + Cms.Dir + "images/ye.gif> 过往专题','" + siteurl + "DissList.aspx','self',false,'过往专题',null,'','','',''));");
                sb_s.Append("mpmenu1=new mMenu('频道首页','" + siteurl + "Index.htm','self','','','','');");
                sb_s.Append("mpmenu1.addItem(new mMenuItem('<img src=" + Cms.Dir + "images/ye.gif> 站内搜索','" + siteurl + "Search.aspx','blank',false,'站内搜索',null,'','','',''));");
                sb_s.Append("mpmenu1.addItem(new mMenuItem('<img src=" + Cms.Dir + "images/ye.gif> 过往专题','" + siteurl + "List/MoreDiss_1.htm','self',false,'过往专题',null,'','','',''));");
                doh.Reset();
                doh.SqlCmd = "select id,title,code from [XK_Column] where len(code)=4 And IsTop=1 And ChannelId=" + CId + " order by code";
                dt = doh.GetDataTable();
                int j = 1;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sb.Append("mpmenu" + Convert.ToString(i + 2) + "=new mMenu('" + dt.Rows[i][1].ToString() + "','List.aspx?id=" + dt.Rows[i][0].ToString() + "','self','','','','');");
                    sb_s.Append("mpmenu" + Convert.ToString(i + 2) + "=new mMenu('" + dt.Rows[i][1].ToString() + "','" + siteurl + "List/" + dt.Rows[i][0].ToString() + "_1.Htm','self','','','','');");
                    doh.Reset();
                    doh.SqlCmd = "select id,title,code from [xk_Column] where len(code)=8 and left(code,4)='" + dt.Rows[i][2].ToString() + "' And ChannelId=" + CId + "  order by code";
                    dt2 = doh.GetDataTable();
                    for (int k = 0; k < dt2.Rows.Count; k++)
                    {
                        doh.Reset();
                        doh.SqlCmd = "select id,title from [xk_Column] where len(code)=12 and left(code,8)='" + dt2.Rows[k][2].ToString() + "' And ChannelId=" + CId + "  order by code";
                        dt3 = doh.GetDataTable();
                        if (dt3.Rows.Count < 1)
                        {
                            sb.Append("mpmenu" + Convert.ToString(i + 2) + ".addItem(new mMenuItem('<img src=" + Cms.Dir + "images/ye.gif> " + dt2.Rows[k][1].ToString() + "','List.aspx?id=" + dt2.Rows[k][0].ToString() + "','self',false,'" + dt2.Rows[k][1].ToString() + "',null,'','','',''));");
                            sb_s.Append("mpmenu" + Convert.ToString(i + 2) + ".addItem(new mMenuItem('<img src=" + Cms.Dir + "images/ye.gif> " + dt2.Rows[k][1].ToString() + "','" + siteurl + "List/" + dt2.Rows[k][0].ToString() + "_1.Htm','self',false,'" + dt2.Rows[k][1].ToString() + "',null,'','','',''));");
                        }
                        else
                        {
                            sb.Append("msub" + j.ToString() + "=new mMenuItem('<img src=" + Cms.Dir + "images/ye.gif> " + dt2.Rows[k][1].ToString() + "','List.aspx?id=" + dt2.Rows[k][0].ToString() + "','self',false,'','1','','','','');");
                            sb_s.Append("msub" + j.ToString() + "=new mMenuItem('<img src=" + Cms.Dir + "images/ye.gif> " + dt2.Rows[k][1].ToString() + "','" + siteurl + "List/" + dt2.Rows[k][0].ToString() + "_1.Htm','self',false,'','1','','','','');");
                            for (int m = 0; m < dt3.Rows.Count; m++)
                            {
                                sb.Append("msub" + j.ToString() + ".addsubItem(new mMenuItem('<img src=" + Cms.Dir + "images/doc.gif> " + dt3.Rows[m][1].ToString() + "','List.aspx?id=" + dt3.Rows[m][0].ToString() + "','self',false,'" + dt3.Rows[m][1].ToString() + "',null,'','','',''));");
                                sb_s.Append("msub" + j.ToString() + ".addsubItem(new mMenuItem('<img src=" + Cms.Dir + "images/doc.gif> " + dt3.Rows[m][1].ToString() + "','" + siteurl + "List/" + dt3.Rows[m][0].ToString() + "_1.Htm','self',false,'" + dt3.Rows[m][1].ToString() + "',null,'','','',''));");
                            }
                            sb.Append("mpmenu" + Convert.ToString(i + 2) + ".addItem(msub" + j.ToString() + ");");
                            sb_s.Append("mpmenu" + Convert.ToString(i + 2) + ".addItem(msub" + j.ToString() + ");");
                            j++;
                        }
                    }
                }
                sb.Append("mwritetodocument();");
                sb_s.Append("mwritetodocument();");

                js = Request.PhysicalApplicationPath + "\\" + dtChannel.Rows[c][2].ToString() + "\\js\\menu1.js";
                sw = new System.IO.StreamWriter(js, false, System.Text.Encoding.GetEncoding("utf-8"));
                sw.Write(sb.ToString());
                sw.Close();

                js = Request.PhysicalApplicationPath + "\\" + dtChannel.Rows[c][2].ToString() + "\\js\\menu.js";
                sw = new System.IO.StreamWriter(js, false, System.Text.Encoding.GetEncoding("utf-8"));
                sw.Write(sb_s.ToString());
                sw.Close();
            }
            ChannelMenu += "\");";
            js = Request.PhysicalApplicationPath + "\\js\\Channel.js";
            sw = new System.IO.StreamWriter(js, false, System.Text.Encoding.GetEncoding("utf-8"));
            sw.Write(ChannelMenu);
            sw.Close();
        }
        #endregion

        #region 一般工具
        /// <summary>
        /// 验证权限
        /// </summary>
        /// <param name="s"></param>
        protected void ChkPower(string s)
        {
            if (s == "") return;
            if (Master_Power.IndexOf("," + s + ",") < 0)
            {
                ErrMsg = "对不起，你没有访问该页面的权限";
                ShowErrMsg();
            }
        }

        /// <summary>
        /// 判断是否有权限
        /// </summary>
        /// <param name="s">权限</param>
        /// <returns></returns>
        protected bool IsPower(string s)
        {
            if (Master_Power.IndexOf("," + s + ",") < 0)
                return false;
            else
                return true;
        }

        protected void ProcessBar(int c, int t)
        {
            Response.Write("<script>setPgb(" + ((c + 1) * 100) / t + ");</script>");
            Response.Flush();
        }

        protected void SetProcessBar(string tit, bool IsInit)
        {
            if (IsInit)
            {
                string strFileName = Request.PhysicalApplicationPath + "\\manager\\progressbar.htm";
                System.IO.StreamReader sr = new System.IO.StreamReader(strFileName, System.Text.Encoding.Default);
                string strHtml = sr.ReadToEnd();
                Response.Write(strHtml);
                sr.Close();
                Response.Flush();
            }
            Response.Write("<script>initPgb('" + tit + "');</script>");
            Response.Flush();
        }

        public void ShowErrMsg()
        {
            if (ErrMsg.Trim() == string.Empty) return;
            string err = "<table cellpadding=3 cellspacing=1 align=center class=tableborder style='width:450px'><tr align=center><th height=25 colspan=2>系统信息 </th></tr><tr><td bgcolor='ffffff' colspan=2 align='left'> ";
            err += "<b>产生错误的可能原因：</b><br><br><li>可能您还没有登陆或者不具有使用当前功能的权限。 ";
            err += "<br><li>" + ErrMsg + "</td></tr><tr><td valign=middle colspan=2 align=center class=forumRowHighlight><a href=# onclick=history.go(-1)>返回上一页</a> <a href='#' onclick=\"top.location='Default.aspx'\">重新登陆</a></td></tr></table>";
            Response.Write(err);
            Response.End();
        }

        /// <summary>
        /// 得到频道类别
        /// </summary>
        /// <param name="isType">Soft,Photo,Index,Article</param>
        /// <returns>下载,图片,首页,文章</returns>
        protected string ChkType(string isType)
        {
            if (isType == "Soft")
                return "下载";
            else if (isType == "Photo")
                return "图片";
            else if (isType == "Index")
                return "首页";
            else
                return "文章";
        }

        /// <summary>
        /// 判断数据状态
        /// </summary>
        /// <param name="isEdit">0,1或-1</param>
        /// <returns>未审核,已审核或已删除</returns>
        protected string ChkState(string isEdit)
        {
            if (isEdit == "0")
                return "<font color='blue'>未审</font>";
            else if (isEdit == "1")
                return "已审";
            else
                return "<font color='red'>已删</font>";
        }

        protected void GetList()
        {
            Response.Redirect(Request.Url.LocalPath + "?ChannelId=" + Channel.Id);
        }
        #endregion

    }
}