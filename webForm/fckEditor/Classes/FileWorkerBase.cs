using System;
using XkCms.WebForm.BaseFunction;

namespace XkCms.WebForm.Editor
{
	public abstract class FileWorkerBase : Admin
	{
		private const string DEFAULT_USER_FILES_PATH = "/UserFiles/" ;

		private string sUserFilesPath ;
		private string sUserFilesDirectory ;

		protected string UserFilesPath
		{
			get
			{
				if ( sUserFilesPath == null )
				{
					
                    sUserFilesPath = (string)Session["FCKeditor:UserFilesPath"];

                    // Try to get from the Web.config file.
                    if (sUserFilesPath == null || sUserFilesPath.Length == 0)
                    {
                        sUserFilesPath = System.Configuration.ConfigurationManager.AppSettings["FCKeditor:UserFilesPath"];

                        // Otherwise use the default value.
                        if (sUserFilesPath == null || sUserFilesPath.Length == 0)
                            sUserFilesPath = DEFAULT_USER_FILES_PATH;

                        // Try to get from the URL.
                        if (sUserFilesPath == null || sUserFilesPath.Length == 0)
                        {
                            sUserFilesPath = Request.QueryString["ServerPath"];
                        }
                    }

					// Check that the user path ends with slash ("/")
					if ( ! sUserFilesPath.EndsWith("/") )
						sUserFilesPath += "/" ;
				}
				return sUserFilesPath ;
			}
		}

		/// <summary>
		/// The absolution path (server side) of the user files directory. It 
		/// is based on the <see cref="FileWorkerBase.UserFilesPath"/>.
		/// </summary>
		protected string UserFilesDirectory
		{
			get	
			{
				if ( sUserFilesDirectory == null )
				{
					// Get the local (server) directory path translation.
					sUserFilesDirectory = Server.MapPath( this.UserFilesPath ) ;
				}
				return sUserFilesDirectory ;
			}
		}
	}
}
