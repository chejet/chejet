using System;
using System.Text;
using System.Web;
using System.IO;

namespace XkCms.LargeFileUpload
{
    internal class WebbUploadStatusHandler : IHttpHandler
    {
        /// <summary>
        /// Implement from IHttphanders
        /// </summary>
        public bool IsReusable
        {
            get { return true; }
        }

        public WebbUploadStatusHandler()
        {
        }

        public void InitProgress(string i_uploadGUID)
        {
            HttpContext context;
            UploadStatus m_uploadStatus = new UploadStatus();
            m_uploadStatus.GetUploadStatus(i_uploadGUID);
            StringBuilder sb = WebbHelper.GetHtml("Progress.page");
            //While files were uploading, update the state.
            if (m_uploadStatus.IsActive)
            {
                switch (m_uploadStatus.Status)
                {
                    case UploadStatus.UploadState.Initializing: sb.Replace("${status}$", "Initializtion..."); break;
                    case UploadStatus.UploadState.Uploading: sb.Replace("${status}$", "Uploading..."); break;
                    case UploadStatus.UploadState.Uploaded: sb.Replace("${status}$", "Upload completed."); break;
                    case UploadStatus.UploadState.Moving: sb.Replace("${status}$", "Moving file..."); break;
                    case UploadStatus.UploadState.Completed: sb.Replace("${status}$", "Finished."); break;
                }
                //WebbTextTrace.TraceMsg(sb.ToString());
                this.ReviewStatus(m_uploadStatus, sb, i_uploadGUID);
            }
            else
            {
                sb.Replace("${Script}$", "<script>window.opener=self;window.close();</script>");
            }
            context = WebbHelper.GetContext();
            //Clear the cache of client browser.
            context.Response.Expires = 0;
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.ContentEncoding = Encoding.Default;
            context.Response.ContentType = "text/html";
            context.Response.Clear();
            //Show page to client.
            context.Response.Write(sb.ToString());
        }

        /// <summary>
        /// Update current upload status.
        /// </summary>
        /// <param name="uploadStatus"></param>
        /// <param name="builder"></param>
        /// <param name="uploadGUID"></param>
        private void ReviewStatus(UploadStatus uploadStatus, StringBuilder builder, string uploadGUID)
        {
            if (uploadStatus.Status == UploadStatus.UploadState.Uploading)
            {
                builder.Replace("${FileName}$", Path.GetFileName(uploadStatus.FileName));
                builder.Replace("${UploadProgress}$", uploadStatus.Percent.ToString());
                builder.Replace("${SurplusProgress}$", Convert.ToString(100 - uploadStatus.Percent));
                builder.Replace("${UploadSpeed}$", (WebbHelper.GetFormatString(uploadStatus.Speed) + "/s"));
                builder.Replace("${LeftTime}$", WebbHelper.GetFormatString(uploadStatus.LeftTime));
                builder.Replace("${BtnOK}$", "disabled");
                builder.Replace("${Refresh}$", ("<meta http-equiv=\"Refresh\" content=\"2\";URL=progress.ashx?UploadGUID=" + uploadGUID + "\">"));
            }
            else if (uploadStatus.Status == UploadStatus.UploadState.Completed)
            {
                builder.Replace("${UploadProgress}$", uploadStatus.Percent.ToString());
                builder.Replace("${SurplusProgress}$", Convert.ToString(100 - uploadStatus.Percent));

                builder.Replace("${FileName}$", (uploadStatus.FileCount.ToString() + "file(s) uploaded success!"));
                builder.Replace("${UploadSpeed}$", (WebbHelper.GetFormatString(uploadStatus.Speed) + "/s"));
                builder.Replace("${LeftTime}$", "finished, no time remain!");

                uploadStatus.Dispose();
                builder.Replace("${BtnOK}$", "onclick=\"javascript:window.opener=self;window.close();return false;\"");
                builder.Replace("${Refresh}$", "");
            }
            else
            {
                builder.Replace("${FileName}$", "Loading...");
                builder.Replace("${UploadProgress}$", "0");
                builder.Replace("${SurplusProgress}$", "100");
                builder.Replace("${UploadSpeed}$", (WebbHelper.GetFormatString(uploadStatus.Speed) + "/s"));
                builder.Replace("${LeftTime}$", "0 second(s)");
                builder.Replace("${BtnOK}$", "disabled");
                builder.Replace("${Refresh}$", ("<meta http-equiv=\"Refresh\" content=\"1\";URL=progress.ashx?UploadID=" + uploadGUID + "\">"));
            }

            if (uploadStatus.Status == UploadStatus.UploadState.Completed)
            {
                builder.Replace("${BtnCancel}$", "onclick=\"javascript:window.opener=self;window.close();return false;\"");
            }
            else
            {
                if (WebbHelper.IsAccordantBrowser())
                {
                    builder.Replace("${BtnCancel}$", "onclick=\"javascript:dialogArguments.location.href=dialogArguments.location.href;window.close();\"");
                }
                else
                {
                    builder.Replace("${BtnCancel}$", "onclick=\"javascript:window.opener.opener=null;window.opener.location.href=window.opener.location.href;window.close();this.disabled=true;\"");
                }
            }

            builder.Replace("${Script}$", "");
        }

        public void ProcessRequest(HttpContext context)
        {
            string m_uploadID = context.Request.QueryString["UploadGUID"];
            string m_filePath = context.Request.FilePath;
            m_filePath = m_filePath.Substring((m_filePath.LastIndexOf("/") + 1)).ToUpper();
            //			bool isUnknownRequest = false;
            if (m_filePath == "PROGRESS.ASHX")
            {
                this.InitProgress(m_uploadID);
            }
            //			The IIS can solve this problem
            //			else
            //			{
            //				isUnknownRequest = true;
            //			}
            //
            //			if (isUnknownRequest)
            //			{
            //				throw new HttpException(500, "unknown request");
            //			}
        }
    }
}