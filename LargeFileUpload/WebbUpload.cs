using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace XkCms.LargeFileUpload
{
	/// <summary>
	/// WebbUpload 的摘要说明。
	/// </summary>
	public class WebbUpload
	{
		string m_GUID;
		string m_script;
        int maxUploadLength = int.Parse(System.Configuration.ConfigurationSettings.AppSettings["maxUploadLength"]);
        string allowFileType = "*";
        string deniedFileType = "";
        /// <summary>
        /// 允许上传的文件类型，如"jpg|jepg|png|bmp|gif"， *=全部，不区分大小写
        /// </summary>
        public string AllowFileType
        {
            set { allowFileType = value; }
            get { return allowFileType.ToLower(); }
        }
        /// <summary>
        /// 不允许上传的文件类型，空为允许所有，不区分大小写
        /// </summary>
        public string DeniedFileType
        {
            set { deniedFileType = value; }
            get { return deniedFileType.ToLower(); }
        }

        public int MaxUploadLength
        {
            set { maxUploadLength = value; }
            get { return maxUploadLength; }
        }
		public WebbUpload()
		{
			this.m_GUID = string.Empty;
			#region Script m_script;
			this.m_script	= String.Empty;
			if(WebbHelper.IsAccordantBrowser())
			{
				this.m_script = @"
					<script language=javascript>
					<!--
					url='${url}$';
					var submited = false;
					function openProgress()
					{
						if(!submited)
						{
							var ary = document.getElementsByTagName('INPUT');
							var openBar = false;
							for(var i=0;i<ary.length;i++)
							{
								var obj = ary[i];
								if(obj.type  == 'file')
								{
									if(obj.value != '')
									{
										openBar = true;
										break;
									}
								}
							}
							if(openBar)
							{
								window.showModelessDialog(url, window, 'status:no;help:no;resizable:no;scroll:no;dialogWidth:398px;dialogHeight:200px');
								submited = true;
							}
							return true;
						}
						else
						{
							event.srcElement.disabled = true;
							return false;
						}
					}
					//-->
					</script>";
			}
			else
			{
				this.m_script = @"
					<script language=javascript>
					<!--
					url='${url}$';
					var submited = false;
					function openProgress()
					{
						if(!submited)
						{
							var ary = document.getElementsByTagName('INPUT');
							var openBar = false;
							for(var i=0;i<ary.length;i++)
							{
								var obj = ary[i];
								if(obj.type  == 'file')
								{
									if(obj.value != '')
									{
										openBar = true;
										break;
									}
								}
							}
							if(openBar)
							{
								var swd = window.screen.availWidth;
								var sht = window.screen.availHeight;
								var wd = 398;
								var ht =170;
								var left = (swd-wd)/2;
								var top = (sht-ht)/2;
								window.open(url,'_blank','status=no,toolbar=no,menubar=no,location=no,height='+ht+',width='+wd+',left='+left+',top='+top, true);
								submited = true;
							}
							return true;
						}
						else
						{
							event.srcElement.disabled = true;
							return false;
						}
					}
					//-->
					</script>";
			}
			#endregion						
		}

		/// <summary>
		/// Register progress bar to a button.
		/// </summary>
		/// <param name="uploadButton"></param>
		/// <param name="causesValidation"></param>
		public void RegisterProgressBar(Button uploadButton, bool causesValidation)
		{
			if (causesValidation)
			{
				uploadButton.CausesValidation = false;
				uploadButton.Attributes["onclick"] = "if (typeof(Page_ClientValidate) == 'function') Page_ClientValidate();if(!Page_BlockSubmit){openProgress();}";
			}
			else
			{
				uploadButton.Attributes["onclick"] = "openProgress();";
			}
			UploadStatus uploadStatus	= new UploadStatus();
			uploadStatus.Status			= UploadStatus.UploadState.Uploading;
			this.m_GUID					= uploadStatus.UploadGUID;
			HttpContext.Current.Application[("Upload_Status_"+this.m_GUID)]	= uploadStatus;		
			string progressUrl			= "progress.ashx?UploadGUID=" + this.m_GUID;
			this.m_script				= this.m_script.Replace("${url}$", progressUrl);
			Page page					= ((Page) WebbHelper.GetContext().Handler);
            page.RegisterHiddenField("Webb_Upload_GUID", this.m_GUID);
            page.RegisterHiddenField("Webb_Upload_MAX", this.maxUploadLength.ToString());
			page.RegisterStartupScript("ProgressScript", this.m_script);
			page.Application.Add(("Webb_Upload_GUID"), this.m_GUID);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="uploadButton"></param>
		/// <param name="causesValidation"></param>
		public void RegisterProgressBar(LinkButton uploadButton, bool causesValidation)
		{
			if (causesValidation)
			{
				uploadButton.CausesValidation = false;
				uploadButton.Attributes["onclick"] = "if (typeof(Page_ClientValidate) == 'function') Page_ClientValidate();if(!Page_BlockSubmit){openProgress();}";
			}
			else
			{
				uploadButton.Attributes["onclick"] = "openProgress();";
			}
			UploadStatus uploadStatus	= new UploadStatus();
			uploadStatus.Status			= UploadStatus.UploadState.Uploading;
			this.m_GUID					= uploadStatus.UploadGUID;
			HttpContext.Current.Application[("Upload_Status_"+this.m_GUID)]	= uploadStatus;		
			string progressUrl			= "progress.ashx?UploadGUID=" + this.m_GUID;
			this.m_script				= this.m_script.Replace("${url}$", progressUrl);
			Page page					= ((Page) WebbHelper.GetContext().Handler);	
			page.RegisterHiddenField("Webb_Upload_GUID", this.m_GUID);
            page.RegisterHiddenField("Webb_Upload_MAX", this.maxUploadLength.ToString());
			page.RegisterStartupScript("ProgressScript", this.m_script);
			page.Application.Add(("Webb_Upload_GUID"), this.m_GUID);
		}

		/// <summary>
		/// Register progress bar to a button.
		/// </summary>
		/// <param name="uploadButton"></param>
		/// <param name="causesValidation"></param>
		public void RegisterProgressBar(WebControl m_controls)
		{
			//m_controls.Attributes["onclick"] = "if (typeof(Page_ClientValidate) == 'function') { if (Page_ClientValidate() == false) { return false; }} this.disabled = true;openProgress();__doPostBack('btn_upload','');if (typeof(Page_ClientValidate) == 'function') Page_ClientValidate();";
			m_controls.Attributes["onclick"] = "openProgress();";
			UploadStatus uploadStatus	= new UploadStatus();
			uploadStatus.Status			= UploadStatus.UploadState.Uploading;
			this.m_GUID					= uploadStatus.UploadGUID;
			HttpContext.Current.Application[("Upload_Status_"+this.m_GUID)]	= uploadStatus;		
			string progressUrl			= "progress.ashx?UploadGUID=" + this.m_GUID;
			this.m_script				= this.m_script.Replace("${url}$", progressUrl);
			Page page					= ((Page) WebbHelper.GetContext().Handler);	
			page.RegisterHiddenField("Webb_Upload_GUID", this.m_GUID);
            page.RegisterHiddenField("Webb_Upload_MAX", this.maxUploadLength.ToString());
			page.RegisterStartupScript("ProgressScript", this.m_script);
			page.Application.Add(("Webb_Upload_GUID"), this.m_GUID);
		}

		public void EnableWebbUpload(bool i_enable)
		{
			Page page					= ((Page) WebbHelper.GetContext().Handler);	
			page.RegisterHiddenField("Webb_Upload_Enable", i_enable.ToString());
		}

		/// <summary>
		/// Get a uploaded file.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public UploadFile GetUploadFile(string name)
		{
            string content = WebbHelper.GetContext().Request.Form[name];
            if ((content == null) || (content == string.Empty)) return null;
            if (!IsEmptyFileContent(content)) return null;
			UploadFile uploadFile = new UploadFile(name);
			return (uploadFile.FileName == string.Empty) ? null : uploadFile;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="m_path"></param>
		public void SetTempPath(string m_path)
		{
			if(Directory.Exists(m_path))
			{
				Page page	= ((Page) WebbHelper.GetContext().Handler);	
				page.RegisterHiddenField("Webb_Upload_TempPath",m_path);
			}
		}

		/// <summary>
		/// Get all uploaded files.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public UploadFileCollection GetUploadFileList(string name)
		{
			UploadFileCollection uploadFiles = new UploadFileCollection();
			string content = WebbHelper.GetContext().Request.Form[name];
			if ((content == null) || (content == string.Empty))
			{
				return uploadFiles;
			}
			else
			{
				string[] contentArray = content.Split(',');
				for (int i = 0; i < contentArray.Length; i++)
				{
					string curContent = contentArray[i];
					if(IsEmptyFileContent(curContent)) uploadFiles.Add(new UploadFile(curContent));
				}
			}
			return uploadFiles;
		}
		private bool IsEmptyFileContent(string m_content)
		{
			string[] m_contents = m_content.Split(';');
			if(m_contents.Length<3)return true;
			int m_start	= m_contents[1].IndexOf("=");
			if(m_start<0)	return false;
			string m_fileName	= m_contents[1].Substring(m_start+1).Replace("\"",string.Empty);
			if(m_fileName==string.Empty)return false;
            if (!IsAllowFileType(m_fileName)) return false;
			return true;
		}

        private bool IsAllowFileType(string name)
        {
            string ext = name.Substring(name.LastIndexOf('.') + 1).ToLower();
            if (allowFileType != "*" && allowFileType.IndexOf(ext) == -1) return false;
            if (deniedFileType != "" && deniedFileType.IndexOf(ext) > -1) return false;
            return true;
        }
	}
}
