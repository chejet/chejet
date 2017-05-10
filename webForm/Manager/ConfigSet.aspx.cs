using System;
using System.Data;
using System.Web;
using System.IO;
using XkCms.WebForm.BaseFunction;
using XkCms.Common.Utils;

namespace XkCms.WebForm.Manager
{
    public partial class ConfigSet : Admin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Admin_Nav();
            ChkPower("001");
            if (!IsPostBack)
            {
                txtName.Text = Cms.Name;
                txtUrl.Text = Cms.Url;
                txtInstallDir.Text = Cms.Dir;
                if (Cms.IsOpen)
                    rblOpen.Items.FindByValue("1").Selected = true;
                else
                    rblOpen.Items.FindByValue("0").Selected = true;
                txtMessage.Text = Cms.CloseMessage;
                txtKeys.Text = Cms.KeyWords;
                txtDescription.Text = Cms.Description;
                if (Cms.IsTiming)
                    rblClose.Items.FindByValue("1").Selected = true;
                else
                    rblClose.Items.FindByValue("0").Selected = true;
                txtOpenTime.Text = Cms.OpenTime;
                if (Cms.IsHtml)
                    rblSkin.Items.FindByValue("0").Selected = true;
                else
                    rblSkin.Items.FindByValue("1").Selected = true;
                txtHtmlIndex.Text = Cms.IndexName;
                if (Cms.AllowReg)
                    rblReg.Items.FindByValue("1").Selected = true;
                else
                    rblReg.Items.FindByValue("0").Selected = true;
                if (Cms.OnlyEmail)
                    rblEmail.Items.FindByValue("1").Selected = true;
                else
                    rblEmail.Items.FindByValue("0").Selected = true;
                if (Cms.CheckUser)
                    rblAdmin.Items.FindByValue("1").Selected = true;
                else
                    rblAdmin.Items.FindByValue("0").Selected = true;
                txtUserPoint.Text = Cms.UserPoint.ToString();
                txtUserUpload.Text = Cms.UserUpload.ToString();
                txtMasterUpload.Text = Cms.MasterUpload.ToString();
                txtManagerDir.Text = Cms.ManagerDir.Substring(Cms.Dir.Length, Cms.ManagerDir.Length - Cms.Dir.Length -1);
                txtEditorDir.Text = Cms.EditorDir.Substring(Cms.Dir.Length, Cms.EditorDir.Length - Cms.Dir.Length - 1);
                if (Cms.AddWater)
                    rblIsWater.Items.FindByValue("1").Selected = true;
                else
                    rblIsWater.Items.FindByValue("0").Selected = true;
                txtImagePath.Text = Cms.WaterImage.Substring(Cms.Dir.Length);
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string setting = "";
            string installdir = txtInstallDir.Text;
            if (installdir.Substring(0, 1) != "/")
                installdir = "/" + installdir;
            if (installdir.Substring(installdir.Length - 1) != "/")
                installdir += "/";
            setting = txtName.Text + "|" + txtUrl.Text + "|" + installdir + "|"
                + rblOpen.SelectedValue + "|" + txtMessage.Text + "|" + txtKeys.Text + "|"
                + txtDescription.Text + "|" + rblClose.SelectedValue + "|" + txtOpenTime.Text + "|"
                + rblSkin.SelectedValue + "|" + txtHtmlIndex.Text.Trim() + "|" + rblReg.SelectedValue + "|"
                + rblEmail.SelectedValue + "|" + rblAdmin.SelectedValue + "|"
                + txtManagerDir.Text + "|" + txtEditorDir.Text + "|"
                + rblIsWater.SelectedValue + "|" + txtImagePath.Text + "|" + txtUserPoint.Text + "|"
                + txtUserUpload.Text + "|" + txtMasterUpload.Text;

            doh.Reset();
            doh.AddFieldItem("info", setting);
            doh.Update("XK_System");
            if (rblSkin.SelectedValue == "1")
            {
                DirFile.DeleteFile(Cms.IndexName);
                doh.Reset();
                doh.SqlCmd = "select dir from [Xk_Channel] where isout=0";
                DataTable dt = doh.GetDataTable();
                for (int i = 0; i < dt.Rows.Count; i++)
                    DirFile.DeleteFile(dt.Rows[i][0].ToString() + "\\" + Cms.IndexName);
            }
            SetupSystemDate();
            Alert("保存成功,已更新缓存");
        }
    }
}
