using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections;
using XkCms.WebForm.BaseFunction;
using XkCms.Common.Utils;

namespace XkCms.WebForm.UserBox
{
    public partial class Friend : UserCenter
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User_Nav(false);
            if (Action == "add")
            {
                if (!IsPostBack)
                {
                    doh.Reset();
                    doh.ConditionExpress = "userid=" + UserId;
                    if (doh.GetCount("xk_friendLink", "id") >= Validator.StrToInt(GroupSetting[28], 0))
                    {
                        ErrMsg = "您最多可以申请" + GroupSetting[28] + "个链接";
                        ShowErrMsg();
                    }
                    doh.Reset();
                    doh.SqlCmd = "select id,title from xk_channel where IsOut=0 And Enabled=1 order by pid";
                    DataTable dt = doh.GetDataTable();
                    ddlChannel.Items.Add(new ListItem("首页", "0"));
                    for (int i = 0; i < dt.Rows.Count; i++)
                        ddlChannel.Items.Add(new ListItem(dt.Rows[i][1].ToString(), dt.Rows[i][0].ToString()));
                    lblUserName.Text = UserName;
                }
                EditBox();
            }
        }

        protected override void GetList()
        {
            plEdit.Visible = false;
            plList.Visible = true;
            AspNetPager1.PageSize = 10;
            int rowCount = 0;
            doh.Reset();
            doh.ConditionExpress = "UserId=" + UserId;
            gvList.DataSource = doh.GetDataTable("xk_FriendLink", "id,LinkName,LinkURL,LinkImgPath,LinkInfo,IsPass", "id", true, "id", AspNetPager1.CurrentPageIndex, AspNetPager1.PageSize, ref rowCount);
            gvList.DataKeyNames = new string[] { "id" };
            gvList.DataBind();
            AspNetPager1.RecordCount = rowCount;
        }

        protected override void EditBox()
        {
            if (GroupSetting[30] != "1")
            {
                ErrMsg = "对不起，您所在的用户组不允许申请友情链接";
                ShowErrMsg();
            }
            plList.Visible = false;
            plEdit.Visible = true;
            doh.Reset();
            XkCms.DataOper.Data.WebFormHandler wh = new XkCms.DataOper.Data.WebFormHandler(doh, "Xk_FriendLink", btnSave);
            wh.AddBind(txtInfo, "LinkInfo", true);
            wh.AddBind(txtLogo, "LinkImgPath", true);
            wh.AddBind(txtName, "LinkName", true);
            wh.AddBind(txtUrl, "LinkUrl", true);
            wh.AddBind(ddlChannel, "ChannelId", false);
            wh.AddBind(rblStyle, "SelectedValue", "style", false);
            wh.AddBind(lblUserName, "username", true);
            string uid = UserId.ToString();
            wh.AddBind(uid, "userid", false);
            if (id == 0)
            {
                wh.Mode = XkCms.DataOper.OperationType.Add;
                string temp = "0";
                wh.AddBind(temp, "ispass", false);
                wh.AddBind(temp, "ordernum", false);
            }
            else
            {
                wh.ConditionExpress = "id=" + id;
                wh.Mode = XkCms.DataOper.OperationType.Modify;
            }
            wh.AddOk += new EventHandler(wh_AddOk);
            wh.ModifyOk += new EventHandler(wh_AddOk);
        }

        void wh_AddOk(object sender, EventArgs e)
        {
            GetList();
        }

        protected void gvList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            doh.Reset();
            doh.ConditionExpress = "id=" + gvList.DataKeys[e.RowIndex].Value + " and userId=" + UserId;
            doh.Delete("Xk_FriendLink");
            GetList();
        }

        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GvRowAddFun(ref e, 4, 1);
        }

        protected void AspNetPager1_PageChanged(object src, XkCms.Common.Web.PageChangedEventArgs e)
        {
            AspNetPager1.CurrentPageIndex = e.NewPageIndex;
            GetList();
        }
    }
}
