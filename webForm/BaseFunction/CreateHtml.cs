using System;
using System.Data;
using System.Collections;
using System.IO;
using System.Web;
using XkCms.Common.Utils;
using XkCms.DataOper.Data;
using XkCms.Common.Web;
using System.Text.RegularExpressions;

namespace XkCms.WebForm.BaseFunction
{
    public class CreateHtml
    {
        #region 定义构造方法和共用属性
        //系统信息
        private SysInfo Cms;
        //当前频道信息
        private ChannelInfo Channel;
        //临时频道信息
        private ChannelInfo _Channel;
        private ColumnInfo Column;
        private string PageNav = string.Empty;
        private string PageTitle = string.Empty;
        private bool IsHtml = false;
        private DbOperHandler doh;
        private string PageStr = string.Empty;
        private string TemplateName = string.Empty;
        private string TemplatePath = string.Empty;
        private int PageCount = 0;

        public CreateHtml(bool ishtm, DbOperHandler _doh, string _ChannelId)
        {
            this.IsHtml = ishtm;
            doh = _doh;
            Cms = new SysInfo(doh);
            if (_ChannelId == string.Empty || _ChannelId == "0")
                Channel = new ChannelInfo();
            else
                Channel = new ChannelInfo(_ChannelId, doh);
            _Channel = Channel;
        }

        private void SetNavAndTitle(string _nav, string _title)
        {
            this.PageNav = "<a href=\"" + Cms.Dir + "\">" + Cms.Name + "</a> → " + _nav;
            this.PageTitle = _title + " -- " + Cms.Name;
        }

        #endregion

        #region 提取公共列表标签
        /// <summary>
        /// 替换公共标签
        /// </summary>
        private void ReplacePublicTab()
        {
            PageStr = PageStr.Replace("{$PageNav$}", PageNav);
            PageStr = PageStr.Replace("{$PageTitle$}", PageTitle);
            PageStr = PageStr.Replace("{$SiteUrl$}", Cms.Url);
            PageStr = PageStr.Replace("{$SiteName$}", Cms.Name);
            PageStr = PageStr.Replace("{$LoginBar$}", GetLoginBar());
            PageStr = PageStr.Replace("{$InstallDir$}", Cms.Dir);

            #region 调查
            string TempStr = string.Empty;
            doh.Reset();
            doh.SqlCmd = "SELECT ID,TITLE,VOTETEXT,TYPE FROM XK_Vote WHERE IsPass=1 and ChannelId=" + Channel.Id + " order by id desc";
            DataTable dt = doh.GetDataTable();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TempStr += CreateVote(Convert.ToInt32(dt.Rows[i][0].ToString()), dt.Rows[i][1].ToString(), dt.Rows[i][2].ToString(), Convert.ToInt32(dt.Rows[i][3].ToString()));
                }
            }
            else
                TempStr = "&nbsp;·暂无站内调查";
            PageStr = PageStr.Replace("{$Vote$}", TempStr);
            #endregion

            #region 公告
            TempStr = "公告： ";
            doh.Reset();
            doh.SqlCmd = "Select Id,Title,AddTime From [XK_Placard] where ChannelId=" + Channel.Id + " Or ChannelId=0 Order By Id Desc";
            dt = doh.GetDataTable();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TempStr = "<a href=\"" + Cms.Dir + "ViewPlacard.aspx?id=" + dt.Rows[i][0].ToString() + "\" target=\"_blank\">" + dt.Rows[i][1].ToString() + "</a>&nbsp;" + Convert.ToDateTime(dt.Rows[i][2].ToString()).ToShortDateString() + "&nbsp;&nbsp;&nbsp;&nbsp;";
                }
            }
            else
                TempStr += "欢迎光临" + Cms.Name + "。";
            PageStr = PageStr.Replace("{$Placard$}", TempStr);
            #endregion

            ReplaceFriendTag();
            PageStr = PageStr.Replace("</title>", "</title>\r<meta name=\"description\" content=\"" + Cms.Description + "\">\r<meta name=\"keywords\" content=\"" + Cms.KeyWords + "\">\r<meta name=\"generator\" content=\"小孔子文章管理系统\">");
            if (IsHtml)
                PageStr = PageStr.Replace("{$ChannelMenu$}", "<script type=\"text/JavaScript\" src=\"" + Cms.Dir + Channel.Dir + "/js/menu.js\"></script>");
            else
                PageStr = PageStr.Replace("{$ChannelMenu$}", "<script type=\"text/JavaScript\" src=\"" + Cms.Dir + Channel.Dir + "/js/menu1.js\"></script>");
            ReplaceDissTab();
            ReplaceFlashImg();
            ReplaceListTag();
        }

        /// <summary>
        /// 提取列表标签
        /// </summary>
        private void ReplaceListTag()
        {
            string TempStr;
            int ReplaceLen;
            int CurrentTag, StartTag, EndTag;
            string ReplaceStr;
            string[] ParameterArray;
            string tagName = "{$GetList(";

            CurrentTag = 1;
            StartTag = 0;

            while (CurrentTag > -1)
            {
                CurrentTag = PageStr.IndexOf(tagName);
                if (CurrentTag > -1)
                {
                    StartTag = CurrentTag;
                    EndTag = PageStr.IndexOf(")$}", StartTag);
                    TempStr = PageStr.Substring(StartTag + tagName.Length, EndTag - (StartTag + tagName.Length));

                    ParameterArray = TempStr.Split(',');
                    ReplaceStr = GetContentList(ParameterArray);

                    ReplaceLen = ReplaceStr.Length;

                    PageStr = PageStr.Substring(0, StartTag) + ReplaceStr + PageStr.Substring(EndTag + 3);

                    StartTag = StartTag + ReplaceLen;
                }
            }
        }

        /// <summary>
        /// 提取列表供列表标签使用
        /// </summary>
        /// <param name="Parameter">列表标签参数</param>
        /// <returns></returns>
        private string GetContentList(string[] Parameter)
        {
            //参数一：频道id，如在频道中使用，设为{$ChannelId$}即可
            //参数二：栏目id, 0=上面指定频道的所有栏目,如在栏目中使用,设为{$ColumnId$}即可
            //参数三：类型，0=最新，1=推荐，2=图片，3=阅读排行
            //参数四：文章数
            //参数五：标题长度
            //参数六：是否显示栏目名称，0=否，1=是
            //参数七：发布日期样式,-1为不显示
            //参数八：是否新窗口打开，0=否，1=是
            //参数九：显示方式，0=列表,1=图片
            //参数十：图片宽度，数字
            //参数十一：图片高度，数字

            if (Parameter.Length < 11)
                return "<li>标签参数应该有11个，即有10个英文逗号</li>";
            for (int i = 0; i < 11; i++)
            {
                if (!Validator.IsNumberId(Parameter[i]) && Parameter[i] != "-1")
                    return "<li>第 " + (i + 1) + " 个参数应为数字</li>";
            }

            if (Parameter[0] == "0")
                return "<li>第一个参数(频道id)应大于零</li>";
            else
                _Channel = new ChannelInfo(Parameter[0], doh);
            if (_Channel.Id == 0)
                return "<li>id为" + Parameter[0] + "的频道不存在</li>";

            string sql = "SELECT TOP " + Parameter[3] + " ID,COLUMNID,COLUMNNAME,TITLE,TCOLOR,AddDate,img,isTop FROM [XK_" + _Channel.Type + "] WHERE ISPASS=1";
            if (Parameter[0] != "0")
                sql += " And ChannelId=" + Parameter[0];
            if (Parameter[1] != "0")
                sql += " And ColumnCode Like (Select Code From [xk_Column] Where Id=" + Parameter[1] + ")+'%'";

            switch (Parameter[2])
            {
                case "1":
                    sql += " And IsTop=1";
                    break;
                case "2":
                    sql += " And IsImg=1";
                    break;
            }

            if (Parameter[2] == "3")
                sql += " Order By ViewNum Desc,AddDate Desc";
            else
                sql += " Order By AddDate Desc";
            sql += ",Id Desc";

            doh.Reset();
            doh.SqlCmd = sql;
            DataTable dt = doh.GetDataTable();
            if (dt.Rows.Count > 0)
            {
                string TempStr = "";
                int titleLen;
                int teLen = int.Parse(Parameter[4]);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    titleLen = teLen;
                    if (dt.Rows[i][7].ToString() == "1")
                        TempStr += "<li class=\"good\">";
                    else
                        TempStr += "<li>";
                    if (Parameter[8] == "0")
                    {
                        if (Parameter[5] != "0")
                        {
                            titleLen = titleLen - Tools.StrLength(dt.Rows[i][2].ToString()) - 2;
                            TempStr += getViewLink(getListLink(dt.Rows[i][1].ToString(), dt.Rows[i][2].ToString()), dt.Rows[i][1].ToString(), false, "", dt.Rows[i][3].ToString(), dt.Rows[i][4].ToString(), dt.Rows[i][0].ToString(), dt.Rows[i][5].ToString(), Parameter[6], Parameter[7], titleLen);
                        }
                        else
                            TempStr += getViewLink("", dt.Rows[i][1].ToString(), false, "", dt.Rows[i][3].ToString(), dt.Rows[i][4].ToString(), dt.Rows[i][0].ToString(), dt.Rows[i][5].ToString(), Parameter[6], Parameter[7], titleLen);
                    }
                    else
                    {
                        string picurl = dt.Rows[i][6].ToString();
                        if (picurl == "") picurl = Cms.Dir + TemplatePath + "images/nopic.gif";
                        TempStr += getViewLink("", dt.Rows[i][1].ToString(), true, "<img width=\"" + Parameter[9] + "\" height=\"" + Parameter[10] + "\" border=\"0\" src=\"" + picurl + "\" />", dt.Rows[i][3].ToString(), "", dt.Rows[i][0].ToString(), "", "-1", Parameter[7], 0);
                    }
                    TempStr += "</li>\r";
                }
                return TempStr;
            }
            return "<li>暂无内容</li>";
        }

        /// <summary>
        /// Flash广告的新闻
        /// </summary>
        private void ReplaceFlashImg()
        {
            string TempStr;
            int CurrentTag, StartTag, EndTag;
            string[] ParameterArray;

            CurrentTag = PageStr.IndexOf("{$FlashNews(");
            if (CurrentTag > -1)
            {
                StartTag = CurrentTag;
                EndTag = PageStr.IndexOf(")$}", StartTag);
                TempStr = PageStr.Substring(StartTag + 12, EndTag - (StartTag + 12));
                ParameterArray = TempStr.Split(',');
                doh.Reset();
                doh.SqlCmd = "SELECT TOP 5 a.ID,COLUMNID,COLUMNNAME,a.TITLE,TCOLOR,AddDate,IsImg,IsTop,Img,b.dir,a.viewNum,a.summary FROM [XK_" + ParameterArray[1] + "] a left join [xk_Channel] b on a.channelid=b.id WHERE ISPASS=1 and isimg=1 and right(img,3)='jpg'";
                if (ParameterArray[0] != "0")
                    doh.SqlCmd += " and channelId=" + ParameterArray[0];
                doh.SqlCmd += " order by a.AddDate desc";
                DataTable dt = doh.GetDataTable();
                TempStr = "";

                #region 得到Flash代码
                string pics = "", txt = "", link = "";
                int j = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    j = i + 1;
                    TempStr += "var imgUrl" + j.ToString() + "='" + dt.Rows[i][8].ToString() + "';\r";
                    TempStr += "var imgtext" + j.ToString() + "='" + Tools.ClipString(dt.Rows[i][3].ToString(), int.Parse(ParameterArray[2])) + "';\r";
                    TempStr += "var imgLink" + j.ToString() + "= escape('" + Cms.Dir + dt.Rows[i][9].ToString() + "/";
                    if (IsHtml)
                        TempStr += "View/" + dt.Rows[i][1].ToString() + "/" + dt.Rows[i][0].ToString() + ".Htm";
                    else
                        TempStr += "View.aspx?id=" + dt.Rows[i][0].ToString();
                    TempStr += "');\r";
                    if (i == 0)
                    {
                        pics = "imgUrl" + j.ToString();
                        txt = "imgtext" + j.ToString();
                        link = "imgLink" + j.ToString();
                    }
                    else
                    {
                        pics += "+\"|\"+imgUrl" + j.ToString();
                        txt += "+\"|\"+imgtext" + j.ToString();
                        link += "+\"|\"+imgLink" + j.ToString();
                    }
                }
                TempStr += "var focus_width=" + ParameterArray[3] + ";\r";
                TempStr += "var focus_height=" + ParameterArray[4] + ";\r";
                TempStr += "var text_height=18;\r";
                TempStr += "var swf_height=" + Convert.ToString(int.Parse(ParameterArray[4]) + 18) + ";\r";
                TempStr += "var pics=" + pics + ";\r";
                TempStr += "var links=" + link + ";\r";
                TempStr += "var texts=" + txt + ";\r";
                TempStr += "document.write('<object classid=\"clsid:d27cdb6e-ae6d-11cf-96b8-444553540000\" codebase=\"http://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,0,0\" width=\"'+ focus_width +'\" height=\"'+ swf_height +'\">');\r";
                TempStr += "document.write('<param name=\"allowScriptAccess\" value=\"sameDomain\"><param name=\"movie\" value=\"" + Cms.Dir + "images/news.swf\"><param name=\"quality\" value=\"high\"><param name=\"bgcolor\" value=\"#F0F0F0\">');\r";
                TempStr += "document.write('<param name=\"menu\" value=\"false\"><param name=wmode value=\"opaque\">');\r";
                TempStr += "document.write('<param name=\"FlashVars\" value=\"pics='+pics+'&links='+links+'&texts='+texts+'&borderwidth='+focus_width+'&borderheight='+focus_height+'&textheight='+text_height+'\">');";
                TempStr += "document.write('<embed src=\"" + Cms.Dir + "images/news.swf\" wmode=\"opaque\" FlashVars=\"pics='+pics+'&links='+links+'&texts='+texts+'&borderwidth='+focus_width+'&borderheight='+focus_height+'&textheight='+text_height+'\" menu=\"false\" bgcolor=\"#F0F0F0\" quality=\"high\" width=\"'+ focus_width +'\" height=\"'+ focus_height +'\" allowScriptAccess=\"sameDomain\" type=\"application/x-shockwave-flash\" pluginspage=\"http://www.macromedia.com/go/getflashplayer\" />');  document.write('</object>');";
                #endregion

                PageStr = PageStr.Substring(0, StartTag) + "\r<script>\r" + TempStr + "\r</script>\r" + PageStr.Substring(EndTag + 3);
            }
        }

        private string GetLoginBar()
        {
            string temp = "<ul id=\"loginBarContent\">\r<li><span class=\"left\">用户名：</span>"
                + "<input type=\"text\" id=\"loginBarName\" class='txtInput'></li>\r"
                + "<li><span class=\"left\">密 &nbsp;码：</span>"
                + "<input type=\"password\" id=\"loginBarPass\" class='txtInput'></li>\r"
                + "<li><span class=\"left\">验证码：</span>"
                + "<input type=\"text\" id=\"txtCheckCode\" class='txtInput'>"
                + "<img src=\"" + Cms.Dir + "ValidateImg.aspx\" id=\"ValidateCode\" align=\"absmiddle\" /></li>\r"
                + "<li><span class=\"registerButton\">"
                + "<a href=\"" + Cms.Dir + "UserBox/Register.aspx\">注 册</a></span>"
                + "<span class=\"loginButton\"><input type=\"button\" onclick=\"getLoginBar(1)\" id=\"btnLoginBarBtn\" value=\"登 陆\">"
                + "</span></li>\r<script>getLoginBar(0)</script>\r</ul>";
            return temp;
        }

        /// <summary>
        /// 创建投票项目
        /// </summary>
        /// <param name="ID">id</param>
        /// <param name="Title">标题</param>
        /// <param name="VoteText">内容</param>
        /// <param name="Mtype">类型</param>
        /// <returns></returns>
        private string CreateVote(int ID, string Title, string VoteText, int Mtype)
        {
            string[] content = VoteText.Split('|');
            string TempStr = "<ul class=\"showVote\"><form name=\"voteform" + ID + "\"><li class=\"title\">" + Title + "</li>\r";
            if (Mtype == 0)
            {
                for (int i = 0; i < content.Length; i++)
                {
                    TempStr += "<li class=\"body\">\r";
                    TempStr += "<input type='radio' name='vote' value=" + i + "> " + content[i].ToString();
                    TempStr += "\r</li>\r";
                }
            }
            else
            {
                for (int i = 0; i < content.Length; i++)
                {
                    TempStr += "<li class=\"body\">\r";
                    TempStr += "<input type='checkbox' name='vote' value=" + i + "> " + content[i].ToString();
                    TempStr += "\r</li>\r";
                }
            }

            TempStr += "<li class=\"button\">\r<input type=button onclick=\"addVote(" + ID + "," + Mtype + ",this)\" value=投票>"
                + "&nbsp;&nbsp;<input type=button value=查看 onclick=\"getVote(" + ID + ",this)\">\r</li>\r</form></ul>";

            return TempStr;
        }

        /// <summary>
        /// 友情链接
        /// </summary>
        private void ReplaceFriendTag()
        {
            string TempStr = "";
            int CurrentTag, StartTag, EndTag;
            string[] ParameterArray;
            string TagStr = "";

            CurrentTag = 1;
            StartTag = 0;

            while (CurrentTag > -1)
            {
                CurrentTag = PageStr.IndexOf("{$FriendLink(");
                if (CurrentTag > -1)
                {
                    TempStr = "<ul>\r";
                    StartTag = CurrentTag;
                    EndTag = PageStr.IndexOf(")$}", StartTag);
                    TagStr = PageStr.Substring(StartTag + 13, EndTag - (StartTag + 13));
                    ParameterArray = TagStr.Split(',');

                    bool isParaOk = true;
                    if (ParameterArray.Length == 2)
                    {
                        if (!Validator.IsNumberId(ParameterArray[0]))
                        {
                            isParaOk = false;
                            TempStr += "<li>第 1 个参数应为数字</li>";
                        }
                        if (!Validator.IsNumberId(ParameterArray[1]))
                        {
                            isParaOk = false;
                            TempStr += "<li>第 2 个参数应为数字</li>";
                        }
                        else if (int.Parse(ParameterArray[1]) < 1)
                        {
                            isParaOk = false;
                            TempStr += "<li>第 2 个参数必须大于 0</li>";
                        }
                    }
                    else
                    {
                        isParaOk = false;
                        TempStr += "<li>参数个数应为2个</li>";
                    }
                    if (isParaOk)
                    {
                        doh.Reset();
                        doh.SqlCmd = "SELECT Top " + ParameterArray[1] + " LINKNAME,LINKURL,LINKIMGPATH,linkinfo,style FROM [XK_FriendLink] where IsPass=1";
                        if (int.Parse(ParameterArray[0]) < 3)
                            doh.SqlCmd += " and ChannelId=" + Channel.Id;
                        if (ParameterArray[0] == "1" || ParameterArray[0] == "4")
                            doh.SqlCmd += " and style=0";
                        else if (ParameterArray[0] == "2" || ParameterArray[0] == "5")
                            doh.SqlCmd += " and style=1";
                        doh.SqlCmd += " order by ordernum Desc,Id desc";
                        DataTable dt = doh.GetDataTable();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            TempStr += "<li><a href=\"" + dt.Rows[i][1].ToString() + "\" target=\"_blank\" title=\"网站名称:" + dt.Rows[i][0].ToString() + "\r网站简介:" + dt.Rows[i][3].ToString() + "\">";
                            if (dt.Rows[i][4].ToString() == "1")
                                TempStr += "<img src=\"" + dt.Rows[i][2].ToString() + "\" border=0 align=\"absmiddle\" width=88 height=31>";
                            else
                                TempStr += dt.Rows[i][0].ToString();
                            TempStr += "</a></li>\r";
                        }
                    }
                    TempStr += "</ul>\r";
                    PageStr = PageStr.Substring(0, StartTag) + TempStr + PageStr.Substring(EndTag + 3);
                }
            }
        }

        #region 列表辅助
        /// <summary>
        /// 获取内容标题
        /// </summary>
        /// <param name="ColumnTitle">栏目名链接</param>
        /// <param name="ViewTitle">标题</param>
        /// <param name="ViewTcolor">标题颜色</param>
        /// <param name="ViewId">内容id</param>
        /// <param name="DateStr">日期</param>
        /// <param name="DateMode">日期模式</param>
        /// <param name="Target">是否新窗口打开</param>
        /// <param name="TitleLen">标题长度</param>
        /// <returns></returns>
        private string getViewLink(string ColumnLink, string ColumnId, bool isPic, string pic, string ViewTitle, string ViewTcolor, string ViewId, string DateStr, string DateStyle, string Target, int TitleLen)
        {
            string TempStr = "";
            if (!isPic)
            {
                if (DateStyle != "-1")
                    TempStr = ShowDateTime(DateStr, DateStyle);
                TempStr += ColumnLink;
            }
            if (Target == "1")
                Target = " target=\"_blank\"";
            else
                Target = "";
            TempStr += "<a href=\"" + getLinkUrl(ColumnId, ViewId) + "\"" + Target;
            TempStr += " title=\"" + ViewTitle + "\">";
            if (isPic)
                TempStr += pic;
            else
            {
                ViewTitle = Tools.ClipString(ViewTitle, TitleLen);
                if (ViewTcolor.Trim() != "")
                    TempStr += "<font color='" + ViewTcolor + "'>" + ViewTitle + "</font>";
                else
                    TempStr += ViewTitle;
            }
            TempStr += "</a>";
            return TempStr;
        }

        private string getLinkUrl(string ColumnId, string ViewId)
        {
            if (IsHtml)
                return Cms.Dir + _Channel.Dir + "/View/" + ColumnId + "/" + ViewId + ".htm";
            else
                return Cms.Dir + _Channel.Dir + "/View.aspx?id=" + ViewId;
        }

        private string getListLink(string ColumnId, string ColumnTitle)
        {
            if (ColumnId != "0")
            {
                if (IsHtml)
                    return "[<a href='" + Cms.Dir + _Channel.Dir + "/List/" + ColumnId + "_1.htm' target='_blank'>" + ColumnTitle + "</a>]";
                else
                    return "[<a href='" + Cms.Dir + _Channel.Dir + "/List.aspx?id=" + ColumnId + "' target='_blank'>" + ColumnTitle + "</a>]";
            }
            else
                return "";
        }
        #endregion

        /// <summary>
        /// 专题列表
        /// </summary>
        private void ReplaceDissTab()
        {
            string TempStr = "";
            int CurrentTag, StartTag, EndTag;
            string[] ParameterArray;
            string TagStr = "";

            CurrentTag = 1;
            StartTag = 0;

            while (CurrentTag > -1)
            {
                CurrentTag = PageStr.IndexOf("{$DisList(");
                if (CurrentTag > -1)
                {
                    StartTag = CurrentTag;
                    EndTag = PageStr.IndexOf(")$}", StartTag);
                    TempStr = string.Empty;
                    TagStr = PageStr.Substring(StartTag + 10, EndTag - StartTag - 10);
                    ParameterArray = TagStr.Split(',');
                    if (ParameterArray.Length == 4)
                    {
                        doh.Reset();
                        doh.SqlCmd = "select top " + ParameterArray[1] + " a.id,a.title,b.title,b.dir,a.istop from xk_diss a left join xk_channel b on a.channelid=b.id where a.id>0";
                        if (ParameterArray[0] != "0")
                            doh.SqlCmd += " and a.ChannelId=" + ParameterArray[0] + " and b.id=" + ParameterArray[0];
                        doh.SqlCmd += " order by a.id desc";
                        DataTable dt = doh.GetDataTable();

                        int titleLen = 0;
                        int teLen = int.Parse(ParameterArray[2]);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            titleLen = teLen;
                            if (dt.Rows[i][4].ToString() == "1")
                                TempStr += "<li class='good'>";
                            else
                                TempStr += "<li>";
                            if (ParameterArray[3] == "1")
                            {
                                TempStr += "[<a href=\"" + Cms.Dir + dt.Rows[i][3].ToString() + "/default.htm\" target=_blank>" + dt.Rows[i][2].ToString() + "</a>] ";
                                titleLen = titleLen - Tools.StrLength(dt.Rows[i][2].ToString()) - 2;
                            }
                            TempStr += "<a href=\"" + Cms.Dir + dt.Rows[i][3].ToString() + "/";
                            if (IsHtml)
                                TempStr += "List/Diss_" + dt.Rows[i][0].ToString() + "_1.htm\" target=_blank>";
                            else
                                TempStr += "Diss.aspx?id=" + dt.Rows[i][0].ToString() + "\" target=_blank>";
                            TempStr += Tools.ClipString(dt.Rows[i][1].ToString(), titleLen) + "</a>";
                            TempStr += "</li>";
                        }
                    }
                    else
                        TempStr += "<li>标签错误，应该为4个参数</li>";
                    PageStr = PageStr.Substring(0, StartTag) + TempStr + PageStr.Substring(EndTag + 3);
                }
            }
        }

        #endregion

        #region 加载各页面
        #region 首页
        /// <summary>
        /// 加载首页
        /// </summary>
        /// <returns></returns>
        public string LoadIndex()
        {
            LoadTemplate("System", "Index");
            SetNavAndTitle("首页", "首页");
            ReplacePublicTab();
            return PageStr;
        }

        /// <summary>
        /// 加载频道首页
        /// </summary>
        /// <returns></returns>
        public string LoadChannelIndex()
        {
            if (Channel.IsOut)
                return JumpOutUrl(Channel.OutUrl);
            if (Channel.TemplateId > 0)
                LoadTemplate(Channel.TemplateId.ToString());
            else
                LoadTemplate(Channel.Type, "Channel");
            SetNavAndTitle(Channel.Title, Channel.Title);
            ReplaceChannelTab();
            GetChannelLoop();
            ReplacePublicTab();
            return PageStr;
        }
        #endregion

        #region Column
        /// <summary>
        /// 动态加载列表页
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public string LoadList(string _id, int pageIndex)
        {
            ArrayList ContentList = LoadListArray(_id, pageIndex);
            if (ContentList.Count == 1)
                return ContentList[0].ToString();
            else
            {
                string TempStr = ContentList[0].ToString().Replace(ContentList[1].ToString(), ContentList[2].ToString());
                return TempStr.Replace("{$PageNumNav$}", HtmlPager.GetPager(PageCount, pageIndex, new string[] { "id" }, new string[] { _id }));
            }
        }

        /// <summary>
        /// 生成静态页,加载列表页
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public ArrayList LoadList(string _id)
        {
            return LoadListArray(_id, 0);
        }

        private ArrayList LoadListArray(string _id, int pageIndex)
        {
            ArrayList ContentList = new ArrayList();
            Column = new ColumnInfo(_id, doh);
            if (Column.Id == 0)
            {
                ContentList.Add("id为" + _id + "的栏目不存在!");
                return ContentList;
            }
            if (Column.IsOut)
            {
                ContentList.Add(JumpOutUrl(Column.OutUrl));
                return ContentList;
            }
            if (Column.TemplateId > 0)
                LoadTemplate(Column.TemplateId.ToString());
            else
                LoadTemplate(Channel.Type, "Column");
            SetNavAndTitle("<a href=\"" + Cms.Dir + Channel.Dir + "/\">" + Channel.Title + "</a> → " + Column.Title + " → " + Channel.ItemName + "列表", Column.Title);

            ReplaceChannelTab();
            ReplaceColumnTab();
            ReplacePublicTab();
            ContentList.Add(PageStr);
            GetListBody(ref ContentList, Column.Id.ToString(), Column.Code, Column.PageSize, pageIndex);
            return ContentList;
        }

        private void GetListBody(ref ArrayList ContentList, string ColumnOrDissId, string ColumnCode, int pageSize, int pageIndex)
        {
            string TempStr = string.Empty;
            string ViewStr = string.Empty;
            int CurrentTag, StartTag, EndTag;
            string TagStr = string.Empty;
            string pStr = string.Empty;
            bool IsLoadAll = pageIndex == 0;
            if (IsLoadAll) pageIndex = 1;

            if (ColumnCode == "")
                pStr = " disid=" + ColumnOrDissId;
            else
                pStr = " ColumnCode like '" + ColumnCode + "%'";
            pStr += " and ispass=1 and ChannelId=" + Channel.Id;

            if (pageSize < 1) pageSize = 20;

            StartTag = 0;
            CurrentTag = PageStr.IndexOf("{$ListLoop$}");
            if (CurrentTag > -1)
            {
                StartTag = CurrentTag;
                EndTag = PageStr.IndexOf("{$/ListLoop$}", StartTag);
                TempStr = PageStr.Substring(StartTag, EndTag + 13 - StartTag);
                ViewStr = TempStr.Substring(12, TempStr.Length - 25);
            }
            else
            {
                TempStr = "{$TopicList$}";
                ViewStr = "";
            }

            ContentList.Add(TempStr);

            int rowCount = 0;
            doh.Reset();
            doh.ConditionExpress = pStr;
            DataTable dt = doh.GetDataTable("Xk_" + Channel.Type, "*", "AddDate", true, "id", pageIndex, pageSize, ref rowCount);
            PageCount = GetPageCount(rowCount, pageSize);
            if (rowCount == 0)
                ContentList.Add("<li>暂无内容</li>");
            else
            {
                ContentList.Add(GetContentList(dt, CurrentTag, ViewStr));
                if (IsLoadAll)
                {
                    for (int i = 2; i < PageCount + 1; i++)
                    {
                        doh.Reset();
                        doh.ConditionExpress = pStr;
                        dt = doh.GetDataTable("Xk_" + Channel.Type, "*", "AddDate", true, "id", i, pageSize, ref rowCount);
                        ContentList.Add(GetContentList(dt, CurrentTag, ViewStr));
                    }
                }
            }
        }

        private string GetContentList(DataTable dt, int IsListLoop, string ViewStr)
        {
            string ReplaceStr = string.Empty;
            if (IsListLoop > -1)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                    ReplaceStr += GetView(ViewStr, dt.Rows[i]) + "\r";
            }
            else
            {
                ReplaceStr = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["istop"].ToString() == "1")
                        ReplaceStr += "<li class=\"good\">";
                    else
                        ReplaceStr += "<li>";
                    ReplaceStr += getViewLink("", dt.Rows[i]["ColumnId"].ToString(), false, "", dt.Rows[i]["title"].ToString(), dt.Rows[i]["tcolor"].ToString(), dt.Rows[i]["id"].ToString(), dt.Rows[i]["addDate"].ToString(), "0", "1", 100) + "</li>\r";
                }
            }
            return ReplaceStr;
        }
        #endregion

        #region View
        /// <summary>
        /// 动态加载内容页
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public string LoadView(string _id)
        {
            doh.Reset();
            doh.SqlCmd = "select * from xk_" + Channel.Type + " where id=" + _id;
            DataTable dt = doh.GetDataTable();
            if (dt.Rows.Count == 0)
                return "id为" + _id + "的内容不存在!是否从外部提交了数据?";
            else
            {
                return LoadView(dt.Rows[0]);
            }
        }

        /// <summary>
        /// 生成静态页,加载内容页
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string LoadView(DataRow dr)
        {
            if (Channel.Type.ToLower() == "article" && dr["isout"].ToString() == "1")
                return JumpOutUrl(dr["outurl"].ToString());

            Column = new ColumnInfo(dr["ColumnId"].ToString(), doh);
            if (Column.ContentTempId > 0)
                LoadTemplate(Column.ContentTempId.ToString());
            else
                LoadTemplate(Channel.Type, "Content");
            if (IsHtml)
                SetNavAndTitle("<a href=\"" + Cms.Dir + Channel.Dir + "/\">" + Channel.Title + "</a> → <a href=\"" + Cms.Dir + Channel.Dir + "/List/" + Column.Id + "_1.htm\">" + Column.Title + "</a> → " + Channel.ItemName + "正文", dr["title"].ToString());
            else
                SetNavAndTitle("<a href=\"" + Cms.Dir + Channel.Dir + "/\">" + Channel.Title + "</a> → <a href=\"" + Cms.Dir + Channel.Dir + "/List.aspx?id=" + Column.Id + "\">" + Column.Title + "</a> → " + Channel.ItemName + "正文", dr["title"].ToString());

            ReplaceChannelTab();
            ReplaceColumnTab();
            ReplacePublicTab();
            PageStr = GetView(PageStr, dr);
            if (Channel.Type.ToLower() != "soft")
            {
                string ContentStr = string.Empty;
                string[] ContentArr;
                int pCount = 0;
                if (Channel.Type.ToLower() == "article")
                {
                    if (dr["content"].ToString().IndexOf("[Xkzi_PageBreak]") > -1)
                    {
                        ContentArr = dr["content"].ToString().Split(new string[] { "[Xkzi_PageBreak]" }, StringSplitOptions.RemoveEmptyEntries);
                        pCount = ContentArr.Length;
                        for (int i = 0; i < pCount; i++)
                            ContentStr += "<ul id=\"ContentBodyPart" + (i + 1) + "\" style=\"display:none;\"><!--" + ClearHtmlComment(ContentArr[i]) + "\r" + getPageNav((i + 1), pCount) + "\r--></ul>";
                    }
                    else
                        ContentStr += "<ul id=\"ContentBodyPart1\">" + ClearHtmlComment(dr["content"].ToString()) + "</ul>";
                }
                else
                {
                    int pageSize = Validator.StrToInt(dr["pageSize"].ToString(), 5);
                    ContentArr = dr["PhotoUrl"].ToString().Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
                    string TempStr = string.Empty;
                    if (ContentArr.Length > pageSize)
                    {
                        bool isChange = true;
                        pCount = ContentArr.Length % pageSize == 0 ? ContentArr.Length / pageSize : ContentArr.Length / pageSize + 1;
                        int cPage = 0;
                        for (int j = 0; j < ContentArr.Length; j++)
                        {
                            if (isChange)
                                TempStr = "<li><img src='" + ContentArr[j] + "'></li>";
                            else
                                TempStr += "<li><img src='" + ContentArr[j] + "'></li>";

                            if ((j + 1) % pageSize == 0 || (j + 1) == ContentArr.Length)
                            {
                                cPage++;
                                ContentStr += "<ul id=\"ContentBodyPart" + cPage + "\" style=\"display:none;\"><!--" + TempStr + "\r" + getPageNav(cPage, pCount) + "\r--></ul>";
                                isChange = true;
                            }
                            else
                                isChange = false;
                        }
                    }
                    else
                    {
                        pCount = 1;
                        for (int j = 0; j < ContentArr.Length; j++)
                            TempStr += "<li><img src='" + ContentArr[j] + "'></li>";
                        ContentStr = "<ul id=\"ContentBodyPart1\">" + TempStr + "</ul>";
                    }
                }
                ContentStr += "<script>GetContentPage(1," + pCount + ")</script>";
                PageStr = PageStr.Replace("{$Content$}", ContentStr);
            }

            return PageStr;
        }

        private string GetView(string ViewStr, DataRow dr)
        {
            string TempStr = string.Empty;

            #region 内容页公用标签
            if (ViewStr.IndexOf("{$LinkUrl$}") > -1)
            {
                TempStr = getLinkUrl(dr["columnid"].ToString(), dr["id"].ToString());
                ViewStr = ViewStr.Replace("{$LinkUrl$}", TempStr);
            }
            if (ViewStr.IndexOf("{$Diss$}") > -1)
            {
                TempStr = "非专题";
                if (dr["disid"].ToString() != "0")
                {
                    doh.Reset();
                    doh.ConditionExpress = "id=" + dr["disid"].ToString();
                    TempStr = doh.GetValue("xk_diss", "title").ToString();
                    if (TempStr != string.Empty)

                        TempStr = "<a href='" + Cms.Dir + Channel.Dir + "/List/Diss_" + dr["disid"].ToString() + "_1.htm' target=_blank>" + TempStr + "</a>";
                }
                ViewStr = ViewStr.Replace("{$Diss$}", TempStr);
            }
            if (ViewStr.IndexOf("{$From$}") > -1)
            {
                TempStr = "本站";
                if (dr["SourceId"].ToString() != "0")
                {
                    doh.Reset();
                    doh.ConditionExpress = "id=" + dr["SourceId"].ToString();
                    object[] _obj = doh.GetValues("xk_source", "url,title");
                    if (_obj != null)
                        TempStr = "<a href='" + _obj[0].ToString() + "' target='_blank'>" + _obj[1].ToString() + "</a>";
                }
                ViewStr = ViewStr.Replace("{$From$}", TempStr);
            }
            string titleimg = dr["img"].ToString();
            if (ViewStr.IndexOf("{$Img$}") > -1)
            {
                if (dr["isimg"].ToString() == "1")
                    ViewStr = ViewStr.Replace("{$Img$}", dr["img"].ToString());
                else
                    ViewStr = ViewStr.Replace("{$Img$}", Cms.Dir + TemplatePath + "Images/nopic.gif");
            }
            ViewStr = ViewStr.Replace("{$Id$}", dr["id"].ToString());
            ViewStr = ViewStr.Replace("{$Title$}", dr["title"].ToString());
            ViewStr = ViewStr.Replace("{$Summary$}", dr["summary"].ToString());
            int txtsum = ViewStr.IndexOf("{$TextSummary");
            if (ViewStr.IndexOf("{$TextSummary(") > -1)
            {
                txtsum += 14;
                string txtnum = ViewStr.Substring(txtsum, ViewStr.IndexOf(")$}", txtsum) - txtsum);
                TempStr = Tools.HtmlToTxt(dr["summary"].ToString());
                txtsum = Validator.StrToInt(txtnum, 60);
                TempStr = Tools.ClipString(TempStr, txtsum);
                ViewStr = ViewStr.Replace("{$TextSummary(" + txtnum + ")$}", TempStr);
            }
            ViewStr = ViewStr.Replace("{$AddDate$}", Validator.StrToDate(dr["AddDate"].ToString(), DateTime.Now).ToShortDateString());
            ViewStr = ViewStr.Replace("{$Author$}", dr["Author"].ToString());
            ViewStr = ViewStr.Replace("{$ViewNum$}", "<span id=\"getViewNum" + dr["id"].ToString() + "\"><script>getViewNum(" + dr["id"].ToString() + ")</script></span>");
            ViewStr = ViewStr.Replace("{$ReviewNum$}", "<span id=\"getReviewNum" + dr["id"].ToString() + "\"><script>getReviewNum(" + dr["id"].ToString() + ")</script></span>");
            ViewStr = ViewStr.Replace("{$KeyWord$}", dr["KeyWord"].ToString());

            //文章副标题
            if (ViewStr.IndexOf("{$SubTitle$}") > -1)
                ViewStr = ViewStr.Replace("{$SubTitle$}", dr["subtitle"].ToString());
            #endregion

            #region 软件相关标签
            if (Channel.Type.ToLower() == "soft")
            {
                ViewStr = ViewStr.Replace("{$Version$}", dr["Version"].ToString());
                ViewStr = ViewStr.Replace("{$CopyrightType$}", dr["CopyrightType"].ToString());
                ViewStr = ViewStr.Replace("{$OperatingSystem$}", dr["OperatingSystem"].ToString());
                ViewStr = ViewStr.Replace("{$SType$}", dr["SType"].ToString());
                ViewStr = ViewStr.Replace("{$SLanguage$}", dr["SLanguage"].ToString());
                ViewStr = ViewStr.Replace("{$UnZipPass$}", dr["UnZipPass"].ToString());
                ViewStr = ViewStr.Replace("{$DemoUrl$}", dr["DemoUrl"].ToString());
                ViewStr = ViewStr.Replace("{$RegUrl$}", dr["RegUrl"].ToString());

                if (ViewStr.IndexOf("{$DownUrl$}") > -1)
                {
                    string[] downArr = dr["downurl"].ToString().Split(new string[] { "|||" }, StringSplitOptions.None);
                    TempStr = "";
                    for (int i = 0; i < downArr.Length; i++)
                    {
                        TempStr += "<li><a href='" + Cms.Dir + "down.aspx?id=" + dr["id"].ToString() + "&no=" + i.ToString() + "' target=_blank>";
                        if (downArr[i].IndexOf("|") > -1)
                            TempStr += downArr[i].Substring(0, downArr[i].IndexOf('|')) + "</a></li>";
                        else
                            TempStr += "下载地址" + (i + 1) + "</a></li>";
                    }
                    ViewStr = ViewStr.Replace("{$DownUrl$}", TempStr);
                }
            }
            #endregion

            #region 相关内容
            if (ViewStr.IndexOf("{$CorList$}") > -1)
            {
                string wStr = string.Empty;
                TempStr = string.Empty;
                if (dr["Correlation"].ToString().Trim() != "")
                    wStr += "id in (" + dr["Correlation"].ToString() + ")";
                else if (dr["keyword"].ToString().Trim() != "")
                {
                    string[] keyTemp = dr["keyword"].ToString().Split(',');
                    wStr += "(1=2";
                    for (int i = 0; i < keyTemp.Length; i++)
                    {
                        if (keyTemp[i].Trim() == "") continue;
                        wStr += " or [keyword] like '%" + keyTemp[i] + "%'";
                    }
                    wStr += ")";
                }
                if (wStr != string.Empty)
                {
                    doh.Reset();
                    doh.SqlCmd = "select top 10 * from xk_" + Channel.Type + " where ChannelId=" + Channel.Id + " and Id<>" + dr["id"].ToString() + " and " + wStr + " order by addDate desc,id desc";
                    DataTable dt = doh.GetDataTable();
                    for (int i = 0; i < dt.Rows.Count; i++)
                        TempStr += "<li>" + getViewLink("", dt.Rows[i]["ColumnId"].ToString(), false, "", dt.Rows[i]["title"].ToString(), dt.Rows[i]["tcolor"].ToString(), dt.Rows[i]["id"].ToString(), "", "-1", "1", 100) + "</li>\r";
                }
                ViewStr = ViewStr.Replace("{$CorList$}", TempStr);
            }
            #endregion
            return ViewStr;
        }

        #endregion

        #region MoreDiss
        public string LoadMoreDiss(int pageIndex)
        {
            ArrayList ContentList = LoadMoreDissArray(pageIndex);
            if (ContentList.Count == 1)
                return ContentList[0].ToString();
            else
            {
                string TempStr = ContentList[0].ToString().Replace(ContentList[1].ToString(), ContentList[2].ToString());
                return TempStr.Replace("{$PageNumNav$}", HtmlPager.GetPager(PageCount, pageIndex));
            }
        }

        public ArrayList LoadMoreDiss()
        {
            return LoadMoreDissArray(0);
        }

        private ArrayList LoadMoreDissArray(int pageIndex)
        {
            ArrayList ContentList = new ArrayList();
            if (Channel.TemplateDiss > 0)
                LoadTemplate(Channel.TemplateDiss.ToString());
            else
                LoadTemplate(Channel.Type, "MoreDiss");
            SetNavAndTitle("<a href=\"" + Cms.Dir + Channel.Dir + "/\">" + Channel.Title + "</a> → 过往专题", "过往专题");
            ReplaceChannelTab();
            ReplacePublicTab();

            ContentList.Add(PageStr);

            string TempStr = string.Empty;
            string ViewStr = string.Empty;
            int CurrentTag, StartTag, EndTag;
            int pageSize = Channel.DissPageSize;
            string pStr = string.Empty;
            bool IsLoadAll = pageIndex == 0;
            if (IsLoadAll) pageIndex = 1;

            StartTag = 0;
            CurrentTag = PageStr.IndexOf("{$ListLoop$}");
            if (CurrentTag > -1)
            {
                StartTag = CurrentTag;
                EndTag = PageStr.IndexOf("{$/ListLoop$}", StartTag);
                TempStr = PageStr.Substring(StartTag, EndTag + 13 - StartTag);
                ViewStr = TempStr.Substring(12, TempStr.Length - 25);
            }
            else
                return ContentList;

            ContentList.Add(TempStr);

            int rowCount = 0;
            pStr = "ChannelId=" + Channel.Id;
            doh.Reset();
            doh.ConditionExpress = pStr;
            DataTable dt = doh.GetDataTable("Xk_Diss", "*", "id", true, "id", pageIndex, pageSize, ref rowCount);
            PageCount = GetPageCount(rowCount, pageSize);
            if (rowCount == 0)
                ContentList.Add("<ul id=\"DissListBody\"><li>暂无内容</li></ul>");
            else
            {
                ContentList.Add(GetDissListBody(dt, ViewStr));
                if (IsLoadAll)
                {
                    for (int i = 2; i < PageCount + 1; i++)
                    {
                        doh.Reset();
                        doh.ConditionExpress = pStr;
                        dt = doh.GetDataTable("Xk_Diss", "*", "id", true, "id", i, pageSize, ref rowCount);
                        ContentList.Add(GetDissListBody(dt, ViewStr));
                    }
                }
            }
            return ContentList;
        }

        private string GetDissListBody(DataTable dt, string ViewStr)
        {
            string TempStr = "<ul id=\"DissListBody\">\r";
            for (int i = 0; i < dt.Rows.Count; i++)
                TempStr += GetDiss(ViewStr, dt.Rows[i]) + "\r";
            TempStr += "</ul>\r";
            return TempStr;
        }
        #endregion

        #region Diss
        public string LoadDiss(string _id, int pageIndex)
        {
            doh.Reset();
            doh.SqlCmd = "select * from [xk_diss] where id=" + _id;
            DataTable dt = doh.GetDataTable();
            if (dt.Rows.Count == 0)
                return "id为" + _id + "的专题不存在!是否从外部提交了数据?";
            else
            {
                ArrayList ContentList = LoadDiss(dt.Rows[0], pageIndex);
                string TempStr = ContentList[0].ToString().Replace(ContentList[1].ToString(), ContentList[2].ToString());
                return TempStr.Replace("{$PageNumNav$}", HtmlPager.GetPager(PageCount, pageIndex, new string[] { "id" }, new string[] { _id }));
            }
        }

        public ArrayList LoadDiss(DataRow dr, int pageIndex)
        {
            ArrayList ContentList = new ArrayList();
            if (dr["templateId"].ToString() == "0")
                LoadTemplate(Channel.Type, "DissList");
            else
                LoadTemplate(dr["templateId"].ToString());
            if (IsHtml)
                SetNavAndTitle("<a href=\"" + Cms.Dir + Channel.Dir + "/\">" + Channel.Title + "</a> → <a href=\"" + Cms.Dir + Channel.Dir + "/List/MoreList_1.htm\">专题</a> → " + dr["title"].ToString(), dr["title"].ToString());
            else
                SetNavAndTitle("<a href=\"" + Cms.Dir + Channel.Dir + "/\">" + Channel.Title + "</a> → <a href=\"" + Cms.Dir + Channel.Dir + "/MoreList.aspx\">专题</a> → " + dr["title"].ToString(), dr["title"].ToString());

            ReplaceChannelTab();
            ReplacePublicTab();
            PageStr = GetDiss(PageStr, dr);
            ContentList.Add(PageStr);
            GetListBody(ref ContentList, dr["id"].ToString(), string.Empty, Validator.StrToInt(dr["pagesize"].ToString(), 20), pageIndex);
            return ContentList;
        }

        private string GetDiss(string ViewStr, DataRow dr)
        {
            ViewStr = ViewStr.Replace("{$Id$}", dr["id"].ToString());
            ViewStr = ViewStr.Replace("{$Title$}", dr["title"].ToString());
            if (dr["img"].ToString() != "")
                ViewStr = ViewStr.Replace("{$Img$}", dr["img"].ToString());
            else
                ViewStr = ViewStr.Replace("{$Img$}", Cms.Dir + TemplatePath + "Images/nopic.gif");
            ViewStr = ViewStr.Replace("{$Info$}", dr["info"].ToString());
            if (IsHtml)
                ViewStr = ViewStr.Replace("{$LinkUrl$}", Cms.Dir + Channel.Dir + "/List/Diss_" + dr["id"].ToString() + "_1.htm");
            else
                ViewStr = ViewStr.Replace("{$LinkUrl$}", Cms.Dir + Channel.Dir + "/Diss.aspx?id=" + dr["id"].ToString());
            return ViewStr;
        }
        #endregion

        #region 其它方法
        private void ReplaceChannelTab()
        {
            PageStr = PageStr.Replace("{$ChannelId$}", Channel.Id.ToString());
            PageStr = PageStr.Replace("{$ChannelName$}", Channel.Title);
            PageStr = PageStr.Replace("{$ChannelType$}", Channel.Type);
            PageStr = PageStr.Replace("{$ChannelDir$}", Channel.Dir);
            PageStr = PageStr.Replace("{$ChannelItemName$}", Channel.ItemName);
            PageStr = PageStr.Replace("{$ChannelItemUnit$}", Channel.ItemUnit);
            PageStr = PageStr.Replace("{$ChannelInfo$}", Channel.Info);
            PageStr = PageStr.Replace("{$ChannelTopicNum$}", Channel.TopicNum.ToString());
            PageStr = PageStr.Replace("{$ChannelReviewNum$}", Channel.ReviewNum.ToString());
        }

        private void ReplaceColumnTab()
        {
            PageStr = PageStr.Replace("{$ColumnId$}", Column.Id.ToString());
            PageStr = PageStr.Replace("{$ColumnName$}", Column.Title);
            PageStr = PageStr.Replace("{$ColumnInfo$}", Column.Info);
            PageStr = PageStr.Replace("{$ColumnChild$}", ColumnChild());
        }

        private string ColumnChild()
        {
            string ColumnCode = Column.Code;
            string SourCode = ColumnCode.Substring(0, 4).ToString();
            int Level = 0;
            string sStr = string.Empty;
            string tempCode = string.Empty;
            int tempLen = 0;
            doh.Reset();
            doh.SqlCmd = "select id,title,code from [xk_Column] Where ChannelId=" + Column.ChannelId + " And (Len(code)=4 Or left(code,4) like '" + SourCode + "')";
            doh.SqlCmd += " order by code";
            DataTable dt = doh.GetDataTable();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<ul class=\"ColumnNav\">");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                tempCode = dt.Rows[i][2].ToString();
                tempLen = tempCode.Length;
                if (tempLen > ColumnCode.Length + 4) continue;
                if (tempLen > ColumnCode.Length)
                {
                    if (tempCode.Substring(0, ColumnCode.Length) != ColumnCode) continue;
                }
                else
                {
                    if (tempCode.Substring(0, tempLen - 4) != ColumnCode.Substring(0, tempLen - 4)) continue;
                }
                Level = (tempLen / 4 - 1) * 2;
                sStr = tempLen == 4 ? "" : "├";
                if (Level > 0)
                {
                    for (int j = 0; j < Level; j++)
                        sStr += "-";
                }
                
                if (Column.Id.ToString() == dt.Rows[i][0].ToString())
                    sb.Append("<li class=\"current\">" + sStr);
                else
                    sb.Append("<li>" + sStr);
                sb.Append("<a href='" + Cms.Dir + Channel.Dir + "/List");
                if (IsHtml)
                    sb.Append("/" + dt.Rows[i][0].ToString() + "_1.Htm");
                else
                    sb.Append(".aspx?id=" + dt.Rows[i][0].ToString());
                sb.Append("' title=\"" + dt.Rows[i][1].ToString() + "\">" + dt.Rows[i][1].ToString() + "</a>");
                sb.Append("</li>");
            }
            sb.Append("</ul>");
            return sb.ToString();
        }

        private void GetChannelLoop()
        {
            string LoopBody = string.Empty;
            string TempStr = string.Empty;
            string ViewStr = string.Empty;
            string ReplaceStr = string.Empty;
            int CurrentTag, StartTag, EndTag;
            CurrentTag = 1;
            while (CurrentTag > -1)
            {
                CurrentTag = PageStr.IndexOf("{$ListLoop(");
                if (CurrentTag == -1) break;
                StartTag = CurrentTag;
                EndTag = PageStr.IndexOf("{$/ListLoop$}", StartTag);
                LoopBody = PageStr.Substring(StartTag, EndTag + 13 - StartTag);
                StartTag = LoopBody.IndexOf(")$}", 0);
                ReplaceStr = LoopBody.Substring(11, StartTag - 11);
                EndTag = LoopBody.IndexOf("{$/ListLoop$}");
                TempStr = LoopBody.Substring(StartTag + 3, EndTag - StartTag - 3);

                if (TempStr.IndexOf("{$ColumnLink$}") > -1)
                {
                    if (IsHtml)
                        TempStr = TempStr.Replace("{$ColumnLink$}", "List/{$ColumnId$}_1.htm");
                    else
                        TempStr = TempStr.Replace("{$ColumnLink$}", "List.aspx?id={$ColumnId$}");
                }

                string pStr = "select id,title,info from [xk_column] where isout=0 and channelId=" + Channel.Id;
                if (ReplaceStr == "0")
                    pStr += " and len(code)=4";
                else
                    pStr += " and id in (" + ReplaceStr + ")";
                pStr += " order by code";
                doh.Reset();
                doh.SqlCmd = pStr;
                DataTable dt = doh.GetDataTable();
                ReplaceStr = string.Empty;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ViewStr = TempStr;
                    ViewStr = ViewStr.Replace("{$ColumnId$}", dt.Rows[i][0].ToString());
                    ViewStr = ViewStr.Replace("{$ColumnName$}", dt.Rows[i][1].ToString());
                    ViewStr = ViewStr.Replace("{$ColumnInfo$}", dt.Rows[i][2].ToString());
                    ReplaceStr += ViewStr;
                }
                PageStr = PageStr.Replace(LoopBody, ReplaceStr);
            }
        }
        #endregion

        #region 用户中心
        public ArrayList LoadRegLogin()
        {
            LoadTemplate("User", "RegLogin");
            SetNavAndTitle("用户注册", "用户注册");
            ReplacePublicTab();
            ArrayList ContentList = new ArrayList();
            int StartTag = PageStr.IndexOf("{$UesrRegLogin$}");
            int EndTag = PageStr.IndexOf("{$/UserRegLogin$}");
            if (StartTag > -1 && EndTag > StartTag)
            {
                ContentList.Add(PageStr.Substring(0, StartTag));
                ContentList.Add(PageStr.Substring(EndTag + 17));
                StartTag += 16;
                string UserRegLoin = PageStr.Substring(StartTag, EndTag - StartTag);
                ContentList.Add(GetDoubleTag("{$RegAnnounce$}", "{$/RegAnnounce$}", UserRegLoin));
                ContentList.Add(GetDoubleTag("{$LoginSuccess$}", "{$/LoginSuccess$}", UserRegLoin));
                ContentList.Add(GetDoubleTag("{$RegSuccess$}", "{$/RegSuccess$}", UserRegLoin));
                ContentList.Add(GetDoubleTag("{$ErrorMsg$}", "{$/ErrorMsg$}", UserRegLoin));
            }
            else
                ContentList.Add("标签错误,请检查是否有{$UesrRegLogin$}和{$/UserRegLogin$}");
            return ContentList;
        }

        public string LoadUserBox()
        {
            LoadTemplate("User", "UserBox");
            SetNavAndTitle("用户管理", "用户管理");
            ReplacePublicTab();
            string TempStr = "";
            if (PageStr.IndexOf("{$UserBoxMenu$}") > -1)
            {
                TempStr = "<div class=\"UserBoxMenu\">\r<ul class=\"menu\">\r";
                TempStr += "<li class=\"title\">服务中心</li>\r";
                TempStr += "<li class=\"sub\"><a href=\"UserSms.aspx\" target=main>站内短信</a></li>\r";
                TempStr += "<li class=\"sub\"><a href=\"Favorite.aspx\" target=main>我的收藏夹</a></li>\r";
                TempStr += "<li class=\"sub\"><a href=\"Friend.aspx?action=add\" target=main>友情链接</a> | <a href=\"Friend.aspx\" target=main>管理</a></li>\r</ul>\r";
                TempStr += "<ul class=\"menu\">\r<li class=\"title\">发布信息</li>\r";
                doh.Reset();
                doh.SqlCmd = "select id,title,type from xk_channel where isout=0 and enabled=1 and isPost=1 order by pid";
                DataTable dt = doh.GetDataTable();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TempStr += "<li class=\"sub\"><a href=\"" + dt.Rows[i][2].ToString() + ".aspx?ChannelId=" + dt.Rows[i][0].ToString() + "&action=add\" target=main>" + dt.Rows[i][1].ToString() + "</a> | ";
                    TempStr += "<a href=\"" + dt.Rows[i][2].ToString() + ".aspx?ChannelId=" + dt.Rows[i][0].ToString() + "\" target=main>管理</a></li>\r";

                }
                TempStr += "</ul>\r<ul class=\"menu\">\r<li class=\"title\">用户设置</li>\r";
                TempStr += "<li class=\"sub\"><a href=\"UserPass.aspx\" target=main>修改密码</a></li>\r";
                TempStr += "<li class=\"sub\"><a href=\"UsersInfo.aspx\" target=main>修改资料</a></li>\r";
                TempStr += "<li class=\"sub\"><a href=# onclick=\"getLoginBar(-1)\">退出系统</a></li>\r";
                TempStr += "</ul></div>";

                PageStr = PageStr.Replace("{$UserBoxMenu$}", TempStr);
            }
            TempStr = "<iframe id=\"main\" style=\"width: 100%;\" name=\"main\" src=\"main.aspx\" scrolling=\"no\" frameborder=\"0\" marginheight=\"0\" marginwidth=\"0\" onload=\"Javascript:setFrameHeight(this)\"></iframe>";
            PageStr = PageStr.Replace("{$UserBoxPanel$}", TempStr);
            return PageStr;
        }
        #endregion

        #region Other
        public ArrayList LoadOther(string tit)
        {
            LoadTemplate("Other", "Other");
            SetNavAndTitle(tit, tit);
            ReplacePublicTab();
            ArrayList ContentList = new ArrayList();
            int StartTag = PageStr.IndexOf("{$Content$}");
            if (StartTag > -1)
            {
                ContentList.Add(PageStr.Substring(0, StartTag));
                ContentList.Add(PageStr.Substring(StartTag + 11));
            }
            else
                ContentList.Add("标签错误,请检查是否有{$Content$}");
            return ContentList;
        }

        public string LoadFriendLink(int pageIndex)
        {
            LoadTemplate("Other", "Friend");
            SetNavAndTitle("友情链接", "友情链接");
            ReplacePublicTab();
            string ViewStr = GetDoubleTag("{$ListLoop$}", "{$/ListLoop$}", PageStr);
            string TempStr = string.Empty;
            doh.Reset();
            int rowCount = 0;
            doh.ConditionExpress = "isPass=1";
            DataTable dt = doh.GetDataTable("Xk_FriendLink", "LinkName,LinkURL,LinkImgPath,LinkInfo,Style", "OrderNum", true, "id", pageIndex, 20, ref rowCount);
            string replacebody = string.Empty;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TempStr = ViewStr;
                TempStr = TempStr.Replace("{$Title$}", dt.Rows[i][0].ToString());
                TempStr = TempStr.Replace("{$Url$}", dt.Rows[i][1].ToString());
                TempStr = TempStr.Replace("{$Info$}", dt.Rows[i][3].ToString());
                if (dt.Rows[i][4].ToString() == "1")
                    TempStr = TempStr.Replace("{$Logo$}", "<img src=\"" + dt.Rows[i][2].ToString() + "\" width=88 height=31 border=0>");
                else
                    TempStr = TempStr.Replace("{$Logo$}", "文字链接");
                replacebody += TempStr;
            }
            PageCount = GetPageCount(rowCount, 20);
            PageStr = PageStr.Replace("{$PageNumNav$}", HtmlPager.GetPager(PageCount, pageIndex));
            PageStr = PageStr.Replace("{$ListLoop$}" + ViewStr + "{$/ListLoop$}", replacebody);
            return PageStr;
        }

        public string LoadViewPlacard(int _id)
        {
            LoadTemplate("Other", "Placard");
            SetNavAndTitle("站内公告", "站内公告");
            ReplacePublicTab();
            doh.Reset();
            doh.ConditionExpress = "id=" + _id;
            object[] _obj = doh.GetValues("Xk_Placard", "title,content,addTime");
            if (_obj == null)
                PageStr = "参数错误,是否从外部提交数据!";
            else
            {
                PageStr = PageStr.Replace("{$Title$}", _obj[0].ToString());
                PageStr = PageStr.Replace("{$AddDate$}", _obj[2].ToString());
                PageStr = PageStr.Replace("{$Content$}", _obj[1].ToString());
            }
            return PageStr;
        }
        #endregion
        #endregion

        #region 相关方法
        private void LoadTemplate(string tempId)
        {
            doh.Reset();
            doh.ConditionExpress = "id=" + tempId;

            object[] Temp = doh.GetValues("Xk_Template", "pid,source");
            if (Temp == null)
            {
                PageStr = "未找到 Id = " + tempId + " 的模板";
                return;
            }
            doh.Reset();
            doh.ConditionExpress = "id=" + Temp[0].ToString();
            object[] _tobj = doh.GetValues("Xk_TempProject", "title,dir");
            if (_tobj == null)
            {
                PageStr = "未找到 Id = " + Temp[0].ToString() + " 的模板项目";
                return;
            }
            else
            {
                TemplateName = _tobj[0].ToString();
                TemplatePath = "Template/" + _tobj[1].ToString() + "/";
            }
            LoadBasicTabs(Temp);
        }
        private void LoadTemplate(string ctype, string cstype)
        {
            doh.Reset();
            doh.ConditionExpress = "Type='" + ctype + "' and sType='" + cstype + "' order by isDefault desc";
            object[] Temp = doh.GetValues("Xk_Template", "pid,source");
            if (Temp == null)
            {
                PageStr = "未找到模板";
                return;
            }
            doh.Reset();
            doh.ConditionExpress = "id=" + Temp[0].ToString();
            object[] _tobj = doh.GetValues("Xk_TempProject", "title,dir");
            if (_tobj == null)
            {
                PageStr = "未找到模板项目";
                return;
            }
            else
            {
                TemplateName = _tobj[0].ToString();
                TemplatePath = "Template/" + _tobj[1].ToString() + "/";
            }
            LoadBasicTabs(Temp);
        }
        private void LoadBasicTabs(object[] Temp)
        {
            string TempStr = "<script type='text/javascript'>var InstallDir=\"" + Cms.Dir + "\";var ChannelId=" + Channel.Id + ";var ChannelType=\"" + Channel.Type + "\";</script>\r";
            PageStr = TempStr + ReadFile(TemplatePath + Temp[1].ToString());
            //替换用户自定义标签
            DataTable dt;
            if (PageStr.IndexOf("{$MY_") > -1)
            {
                doh.Reset();
                doh.SqlCmd = "select title,source from xk_tempLabel where pid=" + Temp[0].ToString() + " order by sort desc";
                dt = doh.GetDataTable();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    PageStr = PageStr.Replace("{$MY_" + dt.Rows[i][0].ToString() + "$}", ReadFile(TemplatePath + dt.Rows[i][1].ToString()));
                }
            }
            if (PageStr.IndexOf("{$System") > -1)
            {
                PageStr = PageStr.Replace("{$SystemRegUser$}", Cms.RegUser.ToString());
                PageStr = PageStr.Replace("{$SystemArticleNum$}", Cms.ArticleNum.ToString());
                PageStr = PageStr.Replace("{$SystemSoftNum$}", Cms.SoftNum.ToString());
                PageStr = PageStr.Replace("{$SystemPhotoNum$}", Cms.PhotoNum.ToString());
                PageStr = PageStr.Replace("{$SystemReviewNum$}", Cms.ReviewNum.ToString());
            }
            if (PageStr.IndexOf("{$Template") > -1)
            {
                PageStr = PageStr.Replace("{$TemplateName$}", TemplateName);
                PageStr = PageStr.Replace("{$TemplatePath$}", TemplatePath);
            }
        }
        /// <summary>
        /// 显示日期
        /// </summary>
        /// <param name="dateStr">日期字符串</param>
        /// <param name="dateMode">显示模式</param>
        /// <returns></returns>
        private string ShowDateTime(string dateStr, string dateMode)
        {
            string str = string.Empty;
            DateTime dates = Validator.StrToDate(dateStr, DateTime.Now);
            if (Tools.DateDiff(dates, DateTime.Now).Days == 0)
            {
                str = "<span class=\"newDate titleTime\">";
                str += Tools.FormatDate(dates, dateMode);
                str += "</span>";
            }
            else
            {
                str = "<span class=\"oldDate titleTime\">";
                str += Tools.FormatDate(dates, dateMode);
                str += "</span>";
            }
            return str;
        }

        private string ReadFile(string tempDir)
        {
            if (File.Exists(HttpContext.Current.Request.PhysicalApplicationPath + tempDir))
            {
                StreamReader sr = new StreamReader(HttpContext.Current.Request.PhysicalApplicationPath + tempDir, System.Text.Encoding.Default);
                string str = sr.ReadToEnd();
                sr.Close();
                return str;
            }
            return "未找到模板文件：" + tempDir;
        }

        private string JumpOutUrl(string JumpUrl)
        {
            return "<meta http-equiv=\"refresh\" content=\"0;URL=" + Cms.Dir + "Redirect.aspx?url=" + JumpUrl + "\">";
        }

        private string getPageNav(int currentPage, int pageCount)
        {
            string TempStr = "";

            TempStr = "<li class='pager'>第 ";
            for (int i = 1; i <= pageCount; i++)
            {
                if (currentPage == i)
                    TempStr += i + " ";
                else
                    TempStr += "<a href=# onclick=\"GetContentPage(" + i + "," + pageCount + ")\">" + i + "</a> ";
            }
            TempStr += "页</li>";
            return TempStr;
        }

        private int GetPageCount(int rows, int psize)
        {
            return rows % psize == 0 ? rows / psize : rows / psize + 1;
        }

        private string ClearHtmlComment(string str)
        {
            Regex myreg = new Regex(@"<!--(.|\s)*?-->", RegexOptions.IgnoreCase);
            str = myreg.Replace(str, "");
            return str;
        }

        private string GetDoubleTag(string StartTag, string EndTag, string TempStr)
        {
            int start = TempStr.IndexOf(StartTag);
            int end = TempStr.IndexOf(EndTag);
            if (start > 0 && end > start)
            {
                start += StartTag.Length;
                return TempStr.Substring(start, end - start);
            }
            return string.Empty;
        }
        #endregion
    }
}