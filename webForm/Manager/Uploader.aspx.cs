using System;
using System.Data;
using System.Web;
using XkCms.WebForm.BaseFunction;
using System.Web.UI.WebControls;
using System.IO;
using XkCms.LargeFileUpload;
using XkCms.Common.Utils;

namespace XkCms.WebForm.Manager
{
    public partial class Uploader : Admin
    {
        private string savePath = string.Empty;
        private string fType = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            Admin_Nav();
            savePath = (string)Session["FCKeditor:UserFilesPath"];
            if (savePath == null)
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
        }

        protected void Button1_Load(object sender, System.EventArgs e)
        {
            Button m_button = sender as Button;
            WebbUpload m_upload = new WebbUpload();
            m_upload.MaxUploadLength = Cms.MasterUpload;
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
        }
    }
}
