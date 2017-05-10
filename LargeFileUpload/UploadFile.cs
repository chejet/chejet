using System;
using System.IO;

namespace XkCms.LargeFileUpload
{
	/// <summary>
	/// 
	/// </summary>
	public class UploadFile
	{
		#region Properties

		/// <summary>
		/// Gets the length (in bytes) of the uploaded file.
		/// </summary>
		public long FileSize
		{
			get { return this.m_filelength; }
		}

		/// <summary>
		/// Gets the MIME content type of the uploaded file.
		/// </summary>
		public string ContentType
		{
			get { return this.m_contenttype; }
		}

		/// <summary>
		/// Gets the file name(including path) of the uploaded file as it was on the client machine.
		/// </summary>
		public string FileName
		{
			get { return this.m_filename; }
		}

		/// <summary>
		/// Gets the file name and path on client.
		/// </summary>
		public string ClientFullPathName
		{
			get{return this.m_clientName;}
		}
		/// <summary>
		/// Gets the file extenion name.
		/// </summary>
		public string ExtendName
		{
			get
			{
                return this.m_filename.Substring(this.m_filename.LastIndexOf("."));
			}
		}
		#endregion

		#region Fields
		private string	m_contenttype;
		private long	m_filelength;
		private string	m_filename;
		private string	m_clientName;
		private string	m_filePath;
		#endregion
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public UploadFile(string i_name)
		{
			if(i_name == null||i_name == string.Empty)
			{
				return;
			//	throw new ArgumentNullException("i_name", "Name can not be null!");
			}

			string m_content		= string.Empty;
			this.m_clientName		= string.Empty;
			this.m_filename			= string.Empty;
			this.m_contenttype		= string.Empty;
			this.m_filelength		= 0;
			
			if(IsContentHeader(i_name))
			{
				m_content			= i_name;
			}
			else if(IsContentHeader(WebbHelper.GetContext().Request[i_name]))
			{
				m_content			= WebbHelper.GetContext().Request[i_name];
			}
			if((m_content==null)||(m_content==string.Empty))
			{
				return;
			}
			//Get file info from content.
			string[] contentArray	= m_content.Split(';');
			string m_temp			= contentArray[0];
			this.m_contenttype		= m_temp.Substring(m_temp.IndexOf(":")+1).Trim();
			m_temp					= contentArray[1];
			this.m_clientName		= m_temp.Substring(m_temp.IndexOf("\"")+1,m_temp.LastIndexOf("\"")-m_temp.IndexOf("\"")-1).Trim();
			m_temp					= contentArray[2];
			this.m_filename			= m_temp.Substring(m_temp.IndexOf("\"")+1,m_temp.LastIndexOf("\"")-m_temp.IndexOf("\"")-1).Trim();
			string uploadFolder = WebbHelper.GetUploadFolder();
			//string uploadFolder = @"C:\Inetpub\wwwroot\WebbTest\Upload";
			if(this.m_filename==null||this.m_filename==string.Empty) return;
			this.m_filePath		= Path.Combine(uploadFolder, this.m_filename);
			try
			{
				this.m_filelength = new FileInfo(this.m_filePath).Length;
			}
			catch (Exception exception)
			{
				string uploadGuid = WebbHelper.GetContext().Request["Webb_Upload_GUID"];
				if (uploadGuid != null)
				{
					WebbHelper.GetContext().Application.Remove(("Upload_Status_" + uploadGuid));
				}
				throw exception;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="line"></param>
		/// <returns></returns>
		private bool IsContentHeader(string i_line)
		{
			//WebbSystem.TraceMsg(line);
			if((i_line == null)||(i_line == String.Empty))
			{
				return false;
			}
			string[] contentArray = i_line.Split(';');

			if (contentArray.Length==3
				&& contentArray[0].IndexOf("Content-Type:")>=0
				&& contentArray[1].IndexOf("filename=\"")>=0
				&& contentArray[2].IndexOf("filePath=\"")>=0)
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// Save file to disk.
		/// </summary>
		/// <param name="filename"></param>
		public void SaveAs(string filename)
		{
			string uploadGuid = WebbHelper.GetContext().Request["Webb_Upload_GUID"];
			string m_fileName	= Path.GetFileName(filename);
			if(m_fileName==null||m_fileName==string.Empty) return;
			if(this.m_filePath==null||this.m_filePath==string.Empty) return;
			try
			{
				UploadStatus uploadStatus;
				FileInfo fileInfo = new FileInfo(this.m_filePath);
				if (uploadGuid != null)
				{
					uploadStatus = new UploadStatus();
					uploadStatus.GetUploadStatus(uploadGuid);
					uploadStatus.Status	= UploadStatus.UploadState.Moving;
					WebbHelper.GetContext().Application[("Upload_Status_" + uploadGuid)] = uploadStatus;
				}

				string directoryName = Path.GetDirectoryName(filename);
				if (!Directory.Exists(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				else if (File.Exists(filename))
				{
					File.Delete(filename);
				}
				//Move temporary file to file that client uploaded. Simply change it's name.
				fileInfo.MoveTo(filename);
				if (uploadGuid == null)
				{
					return;
				}
				uploadStatus		= new UploadStatus();
				uploadStatus.GetUploadStatus(uploadGuid);
				uploadStatus.Status	= UploadStatus.UploadState.Completed;
				WebbHelper.GetContext().Application[("Upload_Status_"+uploadGuid)]=uploadStatus;
			}
			catch (Exception exception)
			{
				if (uploadGuid != null)
				{
					WebbHelper.GetContext().Application.Remove(("Upload_Status_" + uploadGuid));
				}
				throw exception;
			}
		}
	}
}