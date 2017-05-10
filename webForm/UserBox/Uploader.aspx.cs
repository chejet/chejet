using System;
using System.Data;
using System.Web;
using XkCms.WebForm.BaseFunction;
using System.Web.UI.WebControls;
using System.IO;
using XkCms.LargeFileUpload;
using XkCms.Common.Utils;

namespace XkCms.WebForm.UserBox
{
    public partial class Uploader : UserCenter
    {
        private string savePath = string.Empty;
        private string fType = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            User_Nav(false);
            ErrMsg = string.Empty;
            if (GroupSetting[20] == "0")
            {
                ErrMsg = "�������û���û���ϴ��ļ���Ȩ��!";
                ShowErrMsg();
            }
            if (Validator.StrToInt(UserToday[1], 0) >= Validator.StrToInt(GroupSetting[21], 0))
            {
                ErrMsg = "�Բ���!��ÿ��ֻ���ϴ�" + GroupSetting[21] + "���ļ�!";
                ShowErrMsg();
            }
            savePath = (string)Session["FCKeditor:UserFilesPath"];
            if (savePath == null || savePath == "")
            {
                ErrMsg = "�벻Ҫ���ⲿ�ύ����!";
                ShowErrMsg();
            }
            else
                savePath += "/" + DirFile.GetDateDir() + "/";
            Panel1.Visible = false;
            fType = q("t");
            if (fType != "Photo" && fType != "Soft" && fType != "Flash")
            {
                ErrMsg = "�벻Ҫ���ⲿ�ύ����!";
                ShowErrMsg();
            }
            if (fType == "Photo")
                Label2.Text = "�����ϴ�(jpg|jpe|jpeg|gif|bmp|png|ico|psd)";
            else if (fType == "Flash")
                Label2.Text = "�����ϴ�(swf|fla)";
            else
                Label2.Text = "�������ϴ�(asp|asa|aspx|ascx|bat|exe|dll|reg|cgi)";
            Label2.Text += "<br>�����ϴ��ļ���С<b><=" + Cms.UserUpload + " K</b><br>";
            Label2.Text += "�����컹�����ϴ�<b>" + (Validator.StrToInt(GroupSetting[21], 0) - Validator.StrToInt(UserToday[1], 0)) + "</b>���ļ�";
        }

        protected void Button1_Load(object sender, System.EventArgs e)
        {
            Button m_button = sender as Button;
            WebbUpload m_upload = new WebbUpload();
            m_upload.MaxUploadLength = Cms.UserUpload;
            m_upload.RegisterProgressBar(m_button);
        }

        protected void Button1_Click(object sender, System.EventArgs e)
        {
            WebbUpload m_upload = new WebbUpload();
            if (fType == "Photo")
                m_upload.AllowFileType = Label2.Text;
            else if (fType == "Flash")
                m_upload.AllowFileType = Label2.Text;
            else
                m_upload.DeniedFileType = Label2.Text;

            string m_filePath = string.Empty;
            Label1.Text = "�ϴ���ɣ�<br>";

            UploadFile m_file = m_upload.GetUploadFile("m_file");
            if (m_file == null || m_file.FileName == null || m_file.FileName == string.Empty)
            {
                ErrMsg = "�ϴ�ʧ�ܣ��ļ������Ƿ���ȷ!";
                ShowErrMsg();
            }
            string upedfile = Path.GetFileName(m_file.ClientFullPathName);
            if (chkReName.Checked)
                upedfile = DirFile.GetDateFile() + m_file.ExtendName;
            m_filePath = Path.Combine(MapPath(savePath), upedfile);
            m_file.SaveAs(m_filePath);
            if (fType == "Photo" && Cms.AddWater)
                XkCms.Common.Utils.PicDeal.AddWaterPic(m_filePath, Server.MapPath(Cms.WaterImage));
            Label1.Text += savePath + upedfile;
            HiddenFieldFiles.Value = savePath + upedfile;
            Panel1.Visible = true;
            plList.Visible = false;
            Button1.Enabled = false;

            UpdateUserToday(1, 0);
        }
    }
}
