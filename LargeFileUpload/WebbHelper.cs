using System;
using System.IO;
using System.Text;
using System.Web;

namespace XkCms.LargeFileUpload
{
	internal class WebbHelper
	{
		#region Fields

		private static string version;
		private static string RESOURCE_FILE_PREFIX = "XkCms.LargeFileUpload.Resources.";

		#endregion

		#region Constructors

		static WebbHelper()
		{
		}

		public WebbHelper()
		{
		}

		#endregion

		/// <summary>
		/// return the current httpcontext object.
		/// </summary>
		/// <returns></returns>
		public static HttpContext GetContext()
		{
			HttpContext context = HttpContext.Current;
			if (context == null)
			{
				throw new Exception("HttpContext not found");
			}
			return context;
		}

		/// <summary>
		/// return the current version of assembly.
		/// </summary>
		/// <returns></returns>
		public static string GetVersion()
		{
			if (WebbHelper.version == null)
			{
				int majorVersion = typeof (WebbHelper).Assembly.GetName().Version.Major;
				WebbHelper.version = majorVersion.ToString();
			}
			return WebbHelper.version;
		}

		/// <summary>
		/// Return the path of upload folder.
		/// </summary>
		/// <returns></returns>
		public static string GetUploadFolder()
		{
			string uploadFolder = GetContext().Request["Webb_Upload_TempPath"];
			//If upload folder deos not exist, use system temporary folder to hold the file.
			if ((uploadFolder == null) || (uploadFolder == string.Empty)||!Directory.Exists(uploadFolder))
			{
				uploadFolder = Path.GetTempPath();
			}
			return uploadFolder;
		}

		/// <summary>
		/// Load file from chache.
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		private static byte[] LoadFileFromCache(string i_filename)
		{
			byte[] m_buffer;
			HttpContext m_context = GetContext();	
			if(m_context.Cache[RESOURCE_FILE_PREFIX + i_filename] == null)
			{
				m_context.Cache[RESOURCE_FILE_PREFIX + i_filename] = LoadAssemblyFiles(i_filename);
			}	
			m_buffer = (byte[])m_context.Cache[RESOURCE_FILE_PREFIX + i_filename];
			return m_buffer;
		}

		/// <summary>
		/// Load file from assembly.
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public static byte[] LoadAssemblyFiles(string i_filename)
		{
			if (i_filename == null)
			{
				throw new ArgumentNullException("i_filename");
			}
			string fullFileName = RESOURCE_FILE_PREFIX + i_filename;
			byte[] m_fileContent;
//			using(Stream stream=typeof(WebbHelper).Assembly.GetManifestResourceStream(i_filename))
//			{
//				m_fileContent	= new byte[stream.Length];
//				stream.Read(m_fileContent, 0, m_fileContent.Length);
//			}
			Stream stream	= typeof(WebbHelper).Assembly.GetManifestResourceStream(fullFileName);
			m_fileContent	= new byte[stream.Length];
			stream.Read(m_fileContent, 0, m_fileContent.Length);
			stream.Close();
			return m_fileContent;
		}

		/// <summary>
		/// Get html template from buildin resource file
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public static StringBuilder GetHtml(string filename)
		{
			byte[] buffer = LoadFileFromCache(filename);
			if (buffer == null)
			{
				throw new ArgumentNullException("filename", ("isn't find " + filename));
			}
			if (buffer.Length == 0)
			{
				throw new ArgumentNullException("filename", ("isn't find " + filename));
			}
			return new StringBuilder(Encoding.Default.GetString(buffer));
		}

		/// <summary>
		/// Return true if client browser > IE 5.5
		/// </summary>
		/// <returns></returns>
		public static bool IsAccordantBrowser()
		{
			HttpBrowserCapabilities bc = GetContext().Request.Browser;
			if(bc.Browser != "IE" || float.Parse(bc.Version) < 5.5 )
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Turn file size into a readability format.
		/// </summary>
		/// <param name="size"></param>
		/// <returns></returns>
		public static string GetFormatString(double size)
		{
			string sizeString;
			if (size >= 1048576)
			{
				sizeString = (Math.Round(size/1048576, 2)+" MB");
			}
			else if (size >= 1024)
			{
				sizeString = (Math.Round(size/1024, 2)+" KB");
			}
			else
			{
				sizeString = (size+" B");
			}
			return sizeString;
		}
		/// <summary>
		/// Turn time string into a readability format.
		/// </summary>
		/// <param name="span"></param>
		/// <returns></returns>
		public static string GetFormatString(TimeSpan span)
		{
			string timeString=string.Empty;
			if ((span.Days>0) || (span.Hours>0))
			{
				int hours	= ((0x18*span.Days) + span.Hours);
				timeString	= (timeString + hours + "&nbsp;Hour(s)&nbsp;");
			}
			if (span.Minutes>0)
			{
				timeString	= (timeString + span.Minutes + "&nbsp;Minute(s)&nbsp;");
			}
			if (span.Seconds>0)
			{
				timeString	= (timeString + span.Seconds + "&nbsp;Second(s)&nbsp;");
			}
			return timeString;
		}
	}
}