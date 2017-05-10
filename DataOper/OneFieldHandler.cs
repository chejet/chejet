using System;
using System.Web.UI.WebControls;

namespace XkCms.DataOper.Data
{
	public class DbOperEventArgs:System.EventArgs
	{
		public int id;
		public DbOperEventArgs(int _id)
		{
			id=_id;
		}
	}
}
