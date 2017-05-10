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
    public partial class MutilFilesUpload : Admin
    {
        private string savePath = string.Empty;
        private string fType = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            Admin_Nav();
            savePath = (string)Session["FCKeditor:UserFilesPath"];
            if (savePath == null)
            {
                ErrMsg = "请不要从外部提交数据!";
                ShowErrMsg();
            }
            else
                savePath += "/" + DirFile.GetDateDir() + "/";
            Panel1.Visible = false;
            fType = q("t");
            if (fType != "Photo" && fType != "Soft")
            {
                ErrMsg = "请不要从外部提交数据!";
                ShowErrMsg();
            }
            if (fType == "Photo")
                Label2.Text = "允许上传(jpg|jpe|jpeg|gif|bmp|png|ico|psd)";
            else
                Label2.Text = "不允许上传(asp|asa|aspx|ascx|bat|exe|dll|reg|cgi)";
        }

        protected void Button1_Load(object sender, System.EventArgs e)
		{
			Button	m_button		= sender as Button;
			WebbUpload m_upload		= new WebbUpload();
            m_upload.MaxUploadLength = Cms.MasterUpload;
			m_upload.RegisterProgressBar(m_button);
		}

        protected void Button1_Click(object sender, System.EventArgs e)
        {
            WebbUpload m_upload = new WebbUpload();
            if (fType == "Photo")
                m_upload.AllowFileType = Label2.Text;
            else
                m_upload.DeniedFileType = Label2.Text;
            UploadFileCollection m_files = m_upload.GetUploadFileList("m_file");
            string m_filePath = string.Empty;
            Label1.Text = "上传完成：";
            string files = string.Empty;
            string hasfiles = string.Empty;
            foreach (UploadFile m_file in m_files)
            {
                if (m_file == null || m_file.FileName == null || m_file.FileName == string.Empty) continue;
                string upedfile = Path.GetFileName(m_file.ClientFullPathName);
                if (hasfiles.IndexOf(upedfile) > -1) continue;
                hasfiles += upedfile;
                if (chkReName.Checked)
                    upedfile = DirFile.GetDateFile() + m_file.ExtendName;
                m_filePath = Path.Combine(MapPath(savePath), upedfile);
                m_file.SaveAs(m_filePath);
                if (fType == "Photo" && Cms.AddWater)
                    XkCms.Common.Utils.PicDeal.AddWaterPic(m_filePath, Server.MapPath(Cms.WaterImage));
                Label1.Text += "<br>" + savePath + upedfile;
                files += "|||" + savePath + upedfile;
            }
            if (files.Length > 3)
            {
                HiddenFieldFiles.Value = files.Substring(3);
                Panel1.Visible = true;
                plList.Visible = false;
                Button1.Enabled = false;
            }
        }
    }
}
