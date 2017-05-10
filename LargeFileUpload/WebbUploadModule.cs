using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Xml;

namespace XkCms.LargeFileUpload
{
    internal class WebbHttpModule : IHttpModule
    {
        DateTime m_beginTime = DateTime.Now;

        public WebbHttpModule()
        {
        }

        #region Init
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m_application"></param>
        public void Init(HttpApplication m_application)
        {
            m_application.BeginRequest += new EventHandler(WebbUpload_BeginRequest);
            m_application.EndRequest += new EventHandler(WebbUpload_EndRequest);
            m_application.Error += new EventHandler(WebbUpload_Error);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WebbUpload_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication m_application = sender as HttpApplication;
            HttpWorkerRequest m_workRequest = GetWorkerRequest();
            try
            {
                #region function WebbUpload_BeginRequest
                string m_contentType = m_application.Request.ContentType.ToLower();
                //
                if (!m_contentType.StartsWith("multipart/form-data")) return;
                if (!m_workRequest.HasEntityBody()) return;
                TimeSpan m_timeSpan = DateTime.Now.Subtract(m_beginTime);
                //
                int m_currentPoint = 0;				//A flag to signal the current data possition.
                string m_boundaryStr = "\r\n--" + m_contentType.Substring(m_contentType.IndexOf("boundary=") + 9);
                byte[] m_boundaryData = Encoding.ASCII.GetBytes(m_boundaryStr);
                long m_requestTotalSize = Convert.ToInt64(m_workRequest.GetKnownRequestHeader(11));
                long m_MaxSize = this.GetUpLoadFileLength();
                if (m_requestTotalSize > m_MaxSize)
                {
                    m_application.Response.Write("<script>alert('文件大小超出限制!')</script>");
                    return;
                }
                byte[] m_preloadedEntityBody = m_workRequest.GetPreloadedEntityBody();
                m_currentPoint += m_preloadedEntityBody.Length;
                
                string m_uploadMax = this.AnalysePreLoadedData(m_preloadedEntityBody, "Webb_Upload_MAX");
                if (m_uploadMax != null && m_uploadMax != string.Empty)
                {
                    int maxleng = int.Parse(m_uploadMax) * 1024;
                    if (m_requestTotalSize > maxleng)
                    {
                        m_application.Response.Write("<script>alert('文件大小超出限制!')</script>");
                        return;
                    }
                }
                string m_uploadGUID = this.AnalysePreLoadedData(m_preloadedEntityBody, "Webb_Upload_GUID");
                string m_tempPath = this.AnalysePreLoadedData(m_preloadedEntityBody, "Webb_Upload_TempPath");
                string m_enamgeUpload = this.AnalysePreLoadedData(m_preloadedEntityBody, "Webb_Upload_Enable");
                if (m_enamgeUpload != null && m_enamgeUpload.ToLower() == "false")
                {
                    //	m_isEnable					= false;
                    return;
                }
                //WebbTextTrace.TraceMsg(m_uploadGUID);
                UploadStatus m_uploadStatus = new UploadStatus();
                WebbRequestStream m_stream = new WebbRequestStream();
                if (m_uploadGUID != string.Empty)
                {
                    m_uploadStatus.GetUploadStatus(m_uploadGUID);
                }
                else
                {
                    m_uploadGUID = m_application.Application["Webb_Upload_GUID"].ToString();
                    m_uploadStatus.GetUploadStatus(m_uploadGUID);
                }
                if (m_uploadStatus.IsActive)
                {
                    m_uploadStatus.ResetBeginTime();
                    m_uploadStatus.FileName = m_stream.m_rawFileName;
                    m_uploadStatus.AllDataLength = m_requestTotalSize;
                    m_uploadStatus.ReadData = m_preloadedEntityBody.Length;
                    //WebbTextTrace.TraceMsg(m_uploadStatus.ReadData.ToString());
                    m_uploadStatus.Status = UploadStatus.UploadState.Uploading;
                    HttpContext.Current.Application[("Upload_Status_" + m_uploadStatus.UploadGUID)] = m_uploadStatus;
                }
                if (m_tempPath != null && Directory.Exists(m_tempPath))
                {
                    m_stream.SetTempPath(m_tempPath);
                }
                m_stream.SetBoundaryFlag(m_boundaryData);
                m_stream.SetHttpContent(WebbHelper.GetContext());
                m_stream.TransactReadData(m_preloadedEntityBody);
                //Is all of the data have uploaded.
                if (!m_workRequest.IsEntireEntityBodyIsPreloaded())
                {
                    #region
                    int m_bufferSize = 655350;			//640K for test.
                    int m_readSize = 0;
                    byte[] m_readBuffer = new byte[m_bufferSize];
                    while (m_requestTotalSize - m_currentPoint > m_bufferSize)
                    {
                        if (!m_application.Context.Response.IsClientConnected)
                        {
                            this.ReleaseResource(m_application);
                            this.ClearApplication(m_application);
                            break;
                        }
                        #region
                        m_readSize = m_workRequest.ReadEntityBody(m_readBuffer, m_bufferSize);
                        m_currentPoint += m_readSize;
                        if (m_uploadStatus.IsActive)
                        {
                            m_uploadStatus.GetUploadStatus(m_uploadGUID);
                            m_uploadStatus.FileName = m_stream.m_rawFileName;
                            m_uploadStatus.ReadData = m_currentPoint;
                            //m_uploadStatus.AllDataLength	= m_requestTotalSize;
                            //WebbTextTrace.TraceMsg(m_uploadStatus.ReadData.ToString());
                            m_uploadStatus.Status = UploadStatus.UploadState.Uploading;
                            HttpContext.Current.Application[("Upload_Status_" + m_uploadGUID)] = m_uploadStatus;
                        }
                        if (m_stream != null)
                        {
                            m_stream.TransactReadData(m_readBuffer);
                        }
                        #endregion
                    }
                    int m_leftBufferSize = (int)m_requestTotalSize - m_currentPoint;
                    byte[] m_leftBuffer = new byte[m_leftBufferSize];
                    if (!m_application.Context.Response.IsClientConnected)
                    {
                        this.ReleaseResource(m_application);
                        this.ClearApplication(m_application);
                        return;
                    }
                    m_readSize = m_workRequest.ReadEntityBody(m_leftBuffer, m_leftBufferSize);
                    //WebbTextTrace.TraceMsg(m_readSize.ToString());
                    m_currentPoint += m_readSize;
                    if (m_uploadStatus.IsActive)
                    {
                        //	WebbTextTrace.TraceMsg(m_uploadDtatus.LeftTime.ToString());
                        m_uploadStatus.GetUploadStatus(m_uploadGUID);
                        m_uploadStatus.ReadData = m_currentPoint;
                        //	WebbTextTrace.TraceMsg(m_uploadStatus.ReadData.ToString());
                        m_uploadStatus.Status = UploadStatus.UploadState.Uploaded;
                        HttpContext.Current.Application[("Upload_Status_" + m_uploadGUID)] = m_uploadStatus;
                    }
                    if (m_stream != null)
                    {
                        m_stream.TransactReadData(m_leftBuffer);
                    }
                    #endregion
                }
                #endregion
                byte[] readedBodyBuffer = new byte[m_stream.ContentTextBody.Count];
                m_stream.ContentTextBody.CopyTo(readedBodyBuffer);
                m_stream.Dispose();
                this.AddTextPartToRequest(m_workRequest, readedBodyBuffer);
                //WebbTextTrace.TraceMsg(HttpContext.Current.Request.ContentEncoding.GetString(readedBodyBuffer));
            }
            catch (Exception)
            {
                this.ReleaseResource(m_application);
                this.ClearApplication(m_application);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WebbUpload_EndRequest(object sender, EventArgs e)
        {
            #region function WebbUpload_EndRequest
            #endregion

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WebbUpload_Error(object sender, EventArgs e)
        {
            #region function WebbUpload_Error
            this.ReleaseResource(sender as HttpApplication);
            this.ClearApplication(sender as HttpApplication);
            #endregion
            this.Dispose();
        }

        #region Assistant functions
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        private void ReleaseResource(HttpApplication application)
        {
            if (application.Context.Items["Webb_Upload_Stream"] != null)
            {
                FileStream m_fs = ((FileStream)application.Context.Items["Webb_Upload_Stream"]);
                m_fs.Close();
                m_fs = null;
            }
            if (application.Context.Items["Webb_Upload_FileList"] != null)
            {
                Hashtable fileList = ((Hashtable)application.Context.Items["Webb_Upload_FileList"]);

                foreach (object obj in fileList.Values)
                {
                    if (!File.Exists(obj.ToString()))
                    {
                        continue;
                    }
                    File.Delete(obj.ToString());
                }
            }
            application.Context.Items.Clear();
        }
        private void ClearApplication(HttpApplication m_application)
        {
            //Webb_Upload_Stream;
            if (m_application.Context.Items["Webb_Upload_Stream"] != null)
            {
                FileStream m_fs = ((FileStream)m_application.Context.Items["Webb_Upload_Stream"]);
                m_fs.Close();
                m_fs = null;
            }
            if (m_application.Context.Items["Webb_Upload_GUID"] != null)
            {
                string uploadGuid = ((string)m_application.Context.Items["Web_Upload_UploadGUID"]);
                m_application.Application.Remove(("Upload_Status_" + uploadGuid));
            }
        }
        /// <summary>
        /// Get the upload file max size from web.config.
        /// </summary>
        /// <returns></returns>
        private long GetUpLoadFileLength()
        {
            int m_MaxLength = 0;
            string maxlen = System.Configuration.ConfigurationSettings.AppSettings["maxUploadLength"];
            if (maxlen != null)
                m_MaxLength = int.Parse(maxlen);
            else
                m_MaxLength = 10240;

            return (m_MaxLength * 1024);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="textParts"></param>
        /// <returns></returns>
        private byte[] AddTextPartToRequest(HttpWorkerRequest m_request, byte[] m_textData)
        {
            Type m_type;
            BindingFlags m_flags = (BindingFlags.NonPublic | BindingFlags.Instance);
            //Is there application host IIS6.0?
            if (HttpContext.Current.Request.ServerVariables["SERVER_SOFTWARE"].Equals("Microsoft-IIS/6.0"))
            {
                m_type = m_request.GetType().BaseType.BaseType;
            }
            else
            {
                m_type = m_request.GetType().BaseType;
            }
            //Set values of working request
            m_type.GetField("_contentAvailLength", m_flags).SetValue(m_request, m_textData.Length);
            m_type.GetField("_contentTotalLength", m_flags).SetValue(m_request, m_textData.Length);
            m_type.GetField("_preloadedContent", m_flags).SetValue(m_request, m_textData);
            m_type.GetField("_preloadedContentRead", m_flags).SetValue(m_request, true);
            return m_textData;
        }

        /// <summary>
        /// Get value from preloaded entity body. Identified by name. You can get any value in the form. 
        /// But you can not get the file data by this funcion.
        /// </summary>
        /// <param name="preloadedEntityBody"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private string AnalysePreLoadedData(byte[] m_preLoadedData, string m_fiedName)
        {
            string preloadedContent = HttpContext.Current.Request.ContentEncoding.GetString(m_preLoadedData);
            if (preloadedContent.Length > 0)
            {
                string m_temp = "name=\"" + m_fiedName + "\"\r\n\r\n";
                int startIndex = preloadedContent.IndexOf(m_temp) + m_temp.Length;
                int endIndex = preloadedContent.IndexOf("\r\n--", startIndex);
                return preloadedContent.Substring(startIndex, endIndex - startIndex);
            }
            else
            {
                return string.Empty;
            }
        }
        //
        private HttpWorkerRequest GetWorkerRequest()
        {
            IServiceProvider m_provider = HttpContext.Current;
            return ((HttpWorkerRequest)m_provider.GetService(typeof(HttpWorkerRequest)));
        }

        #endregion

    }
}
