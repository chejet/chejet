using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Web;

namespace XkCms.LargeFileUpload
{
	/// <summary>
	/// 
	/// </summary> 
	internal class WebbRequestStream : IDisposable
	{
		#region Enums
		public enum FileStatus		: byte{Close = 1,	Open = 0}
		#endregion

		#region Fields
		private long			i_boundarySize;
		private byte[]			m_boundaryFlag;	
		private byte[]			m_flag;
		private FileStream		m_fs;
		private ArrayList		m_contentTextBody;
		private ArrayList		m_tempBoundary;
		private ArrayList		m_tempContent;
		private ArrayList		m_templist1;
		private ArrayList		m_templist2;
		private FileStatus		m_fileStatus;
		private int				m_boundaryIndex;
		private int				m_contentIndex;
		private string			m_tempPath;
		private HttpContext		m_content;
		public string			m_rawFileName;
		#endregion

		#region Propetries
		public ArrayList	ContentTextBody
		{
			get { return this.m_contentTextBody; }
		}
		#endregion

		public WebbRequestStream()
		{
			this.m_fileStatus		= FileStatus.Close;
			this.m_contentTextBody	= new ArrayList();
			this.m_tempContent		= new ArrayList();
			this.m_tempBoundary		= new ArrayList();
			this.m_boundaryIndex	= 2;
			this.m_contentIndex		= 0;
			this.m_content			= null;
			this.m_flag				= Encoding.ASCII.GetBytes("\r\n\r\n");
			m_templist1				= new ArrayList();
			m_templist2				= new ArrayList();
		}
		public void SetBoundaryFlag(byte[] i_boundaryFlag)
		{
			m_boundaryFlag			= new byte[i_boundaryFlag.Length];
			i_boundaryFlag.CopyTo(m_boundaryFlag,0);
			i_boundarySize			= m_boundaryFlag.Length;
		}
		public void SetHttpContent(HttpContext i_content)
		{
			this.m_content			= i_content;
		}
		public void SetTempPath(string i_tempPath)
		{
			this.m_tempPath			= i_tempPath;
		}

		unsafe public void TransactReadData(byte[] m_readData)
		{
			///There are 5 case when the data come in.
			///1. Content data, write into content.
			///2. Upload File data, write into file.
			///3. Break point at boundary, continue to find boundary.
			///4. Break point at contentHeader, continue to find contentHeader
			#region
			if(m_readData.Length==0||this.m_content==null||this.m_boundaryFlag.Length==0)
			{
				return;
			}
			long i_currentIndex			= 0;
			long i_totalSize			= m_readData.Length;
			bool m_firstCallFunction	= true;
			#endregion
			while(i_currentIndex<i_totalSize)
			{
				if(m_readData[i_currentIndex]==13||m_firstCallFunction)
				{
					m_firstCallFunction	= false;
					while(m_boundaryIndex<i_boundarySize&&i_currentIndex<i_totalSize)
					{
						#region search for boundary flag
						if(m_readData[i_currentIndex]!=this.m_boundaryFlag[m_boundaryIndex])
						{
							break;
						}
						this.m_templist2.Add(m_readData[i_currentIndex]);
						m_boundaryIndex++;
						i_currentIndex++;
						#endregion
					}
					if(m_boundaryIndex==i_boundarySize)
					{
						#region Find a boundary and not the data end.
						if(i_currentIndex+2<i_totalSize)
						{
							this.m_templist2.Add(m_readData[i_currentIndex]);
							i_currentIndex++;
							this.m_templist2.Add(m_readData[i_currentIndex]);
							i_currentIndex++;							
						}
						this.m_contentTextBody.AddRange(m_templist2);
						this.m_templist2.Clear();
						if(this.m_fileStatus==FileStatus.Open&&this.m_fs!=null)
						{
							#region	Close the current open file.
							m_fs.Flush();
							m_fs.Close();
							m_fs					= null;
							this.m_fileStatus		= FileStatus.Close;	
							#endregion
						}
						///Search for content header.
						while(i_currentIndex<i_totalSize)
						{
							#region				
							while(i_currentIndex<i_totalSize&&m_contentIndex<4)
							{
								#region search for content header end.
								if(m_flag[m_contentIndex]!=m_readData[i_currentIndex])
								{
									break;
								}
								m_templist1.Add(m_readData[i_currentIndex]);
								m_contentIndex++;
								i_currentIndex++;
								#endregion
							}
							if(m_contentIndex==4)
							{
								#region		//Find a contend header
								this.m_tempContent.AddRange(m_flag);
								m_templist1.Clear();
								byte[] m_temp1				= new byte[m_tempContent.Count];
								m_tempContent.CopyTo(m_temp1);
								string m_contentData		= HttpContext.Current.Request.ContentEncoding.GetString(m_temp1);								
								if(m_contentData.IndexOf("\"; filename=\"")<0)
								{
									//This is other data.
									this.m_contentTextBody.AddRange(m_tempContent);
									this.m_fileStatus		= FileStatus.Close;
								}
								else
								{
									//This is a upload file data.
									string[] m_fileContent	= this.GetFileContent(m_contentData);
									string m_filePath		= this.GreateFileStream(m_fileContent[2]);									
									StringBuilder sb		= new StringBuilder();
									string[] sbArray		= new string[11];
									sbArray[0]				= m_fileContent[0];
									sbArray[1]				= ";";
									sbArray[2]				= m_fileContent[1];
									sbArray[3]				= "\r\n\r\n";
									sbArray[4]				= m_fileContent[3];
									sbArray[5]				= ";";
									sbArray[6]				= m_fileContent[2];
									sbArray[7]				= "; ";
									sbArray[8]				= "filePath=\"";
									sbArray[9]				= m_filePath;
									sbArray[10]				= "\"";
									sb.Append(string.Concat(sbArray));
									this.m_contentTextBody.AddRange(Encoding.UTF8.GetBytes(sb.ToString().ToCharArray()));
									sb.Remove(0, sb.Length);
									this.m_fileStatus		= FileStatus.Open;
								}
								m_tempContent.Clear();
								m_templist1.Clear();
								m_contentIndex				= 0;
								m_boundaryIndex				= 0;
								#endregion
								break;
							}
							else if(i_currentIndex==i_totalSize)
							{
								//The break point is in the content header, just break, the next will be go on.
								break;
							}
							else if(m_templist1.Count>0)
							{
								m_tempContent.AddRange(m_templist1);
								m_templist1.Clear();
								m_contentIndex		= 0;
							}
							m_tempContent.Add(m_readData[i_currentIndex]);
							#endregion
							i_currentIndex++;
						}
						#endregion
					}
				}
				if(i_currentIndex>=i_totalSize) break;
				if(m_readData[i_currentIndex]==13)
				{
					#region If the file data contaion \r, then we should check it.
					long m_temp			= i_currentIndex;
					int m_temp2			= 0;
					while(m_temp2<i_boundarySize&&i_currentIndex<i_totalSize)
					{						
						if(m_readData[i_currentIndex]!=m_boundaryFlag[m_temp2])
						{
							break;
						}
						m_temp2++;
						i_currentIndex++;
					}
					//However, the if the file is open, the data should writ into file.
					if(m_fileStatus==FileStatus.Open&&m_templist2.Count>0)
					{
						byte[] temp			= new byte[m_templist2.Count];
						m_templist2.CopyTo(temp);
						m_fs.Write(temp	,0,m_templist2.Count);
						m_templist2.Clear();
						temp				= null;
					}					
					if(m_temp2==i_boundarySize)
					{
						//If here is a boundary, should check the data again, so write down the "i_currentIndex", and check again.
						m_boundaryIndex		= 0;
						i_currentIndex		= m_temp;
						continue;
					}
					else
					{
						//Or recheck the data
						i_currentIndex		= m_temp;
					}
					#endregion
				}
				if(m_fileStatus==FileStatus.Open)
				{
					#region //wirte to file
					if(m_templist2.Count>0)
					{						
						byte[] temp			= new byte[m_templist2.Count];
						m_templist2.CopyTo(temp);
						m_fs.Write(temp,0,temp.Length);
						temp				= null;
					}
					m_fs.WriteByte(m_readData[i_currentIndex]);
					#endregion
				}
				else
				{
					#region write to context
					if(m_tempBoundary.Count>0)
					{
						this.m_contentTextBody.AddRange(m_templist2);
					}
					this.m_contentTextBody.Add(m_readData[i_currentIndex]);
					#endregion
				}
				this.m_templist1.Clear();
				this.m_templist2.Clear();
				this.m_boundaryIndex	= 0;
				this.m_contentIndex		= 0;
				i_currentIndex++;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="m_contentHeader"></param>
		/// <returns></returns>
		#region Some assistant functions
		private string[] GetFileContent(string m_contentHeader)
		{
			m_contentHeader			= m_contentHeader.Replace("\r\n",";");
			return m_contentHeader.Split(new char[1]{';'});
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="m_fileName"></param>
		/// <returns></returns>
	 	private string GreateFileStream(string m_fileName)
		{
			m_fileName				= m_fileName.Replace("\"","");
			if(m_fileName.Length<=0||m_fileName.IndexOf(".")<0) return string.Empty;
			string m_exten			= m_fileName.Substring(m_fileName.LastIndexOf("."));
			this.m_rawFileName		= m_fileName.Substring(m_fileName.IndexOf("=")+1);	
			string m_GUIDFileName	= Guid.NewGuid().ToString();
			if (this.m_tempPath == string.Empty||!Directory.Exists(m_tempPath))
			{
				this.m_tempPath		= Path.GetTempPath();
			}
			string m_file			= Path.Combine(m_tempPath,m_GUIDFileName+m_exten);           
			Hashtable m_ht			= (Hashtable)m_content.Items["Webb_Upload_FileList"];
			if(m_ht!=null)
			{
				m_ht.Add(Path.GetFileNameWithoutExtension(m_GUIDFileName), m_GUIDFileName+m_exten);				
			}
			else
			{
				m_ht				= new Hashtable();
				m_ht.Add(Path.GetFileNameWithoutExtension(m_GUIDFileName), m_file);	
			}
			m_content.Items["Webb_Upload_FileList"] = m_ht;
			this.m_fs								= new FileStream(m_file,FileMode.Create);
			m_content.Items["Webb_Upload_Stream"]	= this.m_fs;
			return m_GUIDFileName+m_exten;
		}
		#endregion
		
		#region IDisposable members..
		public void Dispose()
		{
			this.m_boundaryFlag		= null;	
			this.m_flag				= null;	
			this.m_fs				= null;	
			this.m_contentTextBody	= null;	
			this.m_tempBoundary		= null;	
			this.m_tempContent		= null;	
			this.m_templist1		= null;	
			this.m_templist2		= null;	
		}
		#endregion
	}
}