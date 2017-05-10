using System;
using System.Web;

namespace XkCms.LargeFileUpload
{
	/// <summary>
	/// 
	/// </summary>
	internal class UploadStatus:IDisposable
	{
		public enum UploadState : byte
		{		
			Initializing	= 0,
			Uploading		= 1,
			Uploaded		= 2,
			Moving			= 3,
			Completed		= 4,
			Error			= 5
		}

		#region Fields
		private long		m_dataLength;
		private long		m_readLength;
		private DateTime	m_beginTime;
		private int			m_fileCount;
		private string		m_fileName;		//File name on client PC when uploading.
		private bool		m_isActive;
		private string		m_uploadID;
		private UploadState	m_status;
		#endregion
        
		#region properties
		public TimeSpan LeftTime
		{
			get
			{
				if(this.Speed==0)
				{
					return System.DateTime.Now.Subtract(System.DateTime.Now);
				}
				else
				{
					long m_temp		= (this.m_dataLength-this.m_readLength)/this.Speed;
					return System.DateTime.Now.Subtract(System.DateTime.Now.AddSeconds(-m_temp));
				}
			}
		}
		public bool IsActive
		{
			get{return m_isActive;}
			set{this.m_isActive=value;}
		}
		public string FileName
		{
			get{return m_fileName;}
			set{this.m_fileName	= value;}
		}
		public long Speed
		{
			get
			{
				TimeSpan m_timeSpan		= DateTime.Now.Subtract(this.m_beginTime);
				long m_spendSec			= Convert.ToInt64(m_timeSpan.TotalSeconds);
				if(m_spendSec<=0) m_spendSec = 1;
				return this.m_readLength/m_spendSec;
			}
		}
		public int FileCount
		{
			get{return this.m_fileCount;}
			set{this.m_fileCount= value;}
		}
		public string UploadGUID
		{
			get{return this.m_uploadID;}
			set{this.m_uploadID=value;}
		}
		public UploadState Status
		{
			get{return m_status;}
			set{this.m_status	= value;}
		}
		public long Percent
		{
			get
			{
				if(m_dataLength!=0)
				{
					return (100*m_readLength/m_dataLength);
				}
				return 0;
			}
		}
		public long AllDataLength
		{
			get{return this.m_dataLength;}
			set
			{
				if(value<0)
				{
					this.m_dataLength	= 1;
				}
				else
				{
					this.m_dataLength	= value;
				}
			}
		}
		public DateTime BeginTime
		{
			get{return this.m_beginTime;}
			set{this.m_beginTime = value;}
		}
		public long ReadData
		{
			get{return this.m_readLength;}
			set{this.m_readLength	= value;}
		}
		#endregion
		
		public UploadStatus()
		{
			this.m_beginTime		= System.DateTime.Now;
			this.m_fileCount		= 0;						//1
			this.m_fileName			= string.Empty;
			this.m_dataLength		= 100L;						//100
			this.m_readLength		= 0L;						//1
			this.m_uploadID			= Guid.NewGuid().ToString();
			this.m_status			= UploadState.Initializing;
			this.m_isActive			= true;
		}

		public void ResetBeginTime()
		{
			this.m_beginTime		= System.DateTime.Now;
		}

		public void GetUploadStatus(string m_uploadGUID)
		{
			UploadStatus m_status	= HttpContext.Current.Application[("Upload_Status_"+m_uploadGUID)] as UploadStatus;
			if(m_status!=null)
			{
				this.m_beginTime	= m_status.BeginTime;
				this.m_fileCount	= m_status.FileCount;
				this.m_fileName		= m_status.FileName;
				this.m_dataLength	= m_status.AllDataLength;
				this.m_readLength	= m_status.ReadData;
				this.m_uploadID		= m_status.UploadGUID;
				this.m_status		= m_status.Status;
				this.m_isActive		= true;
			}
			else
			{
				this.m_isActive			= false;
			}
		}
		#region un-initializtion and dispose
		public void Dispose()
		{
		}

		~UploadStatus()
		{
		}
		#endregion
	}
}
