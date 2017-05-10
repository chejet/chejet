using System;
using System.Collections;

namespace XkCms.LargeFileUpload
{
	/// <summary>
	/// UploadFileCollection
	/// </summary>
	public class UploadFileCollection : ICollection, IEnumerable
	{
		#region Fields
		private ArrayList filelist;
		#endregion

		#region Properties
		public int Count
		{
			get { return this.filelist.Count; }
		}
		#endregion

		#region Methods
		public bool IsSynchronized
		{
			get { return this.filelist.IsSynchronized; }
		}
		public UploadFile this[int index]
		{
			get { return ((UploadFile) this.filelist[index]); }
		}
		public object SyncRoot
		{
			get { return this.filelist.SyncRoot; }
		}
		public UploadFileCollection()
		{
			this.filelist = new ArrayList();
		}
		public void Add(UploadFile File)
		{
			this.filelist.Add(File);
		}
		public void CopyTo(Array array, int index)
		{
			this.filelist.CopyTo(array, index);
		}
		public IEnumerator GetEnumerator()
		{
			return this.filelist.GetEnumerator();
		}
		#endregion
	}
}
