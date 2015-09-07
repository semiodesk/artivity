using System;
using Semiodesk.Trinity;

namespace Artivity.Model.ObjectModel
{
	[RdfClass(NFO.FileDataObject)]
	public class File : Entity
	{
		#region Members

		[RdfProperty(NFO.fileUrl)]
		public string Url { get; set; }

		[RdfProperty(NFO.fileSize)]
		public long ByteSize { get; set; }

		[RdfProperty(NFO.fileCreated)]
		public DateTime CreationTime { get; set; }

		[RdfProperty(NFO.fileLastAccessed)]
		public DateTime LastAccessTime { get; set; }

		[RdfProperty(NFO.fileLastModified)]
		public DateTime LastModificationTime { get; set; }

		[RdfProperty(PROV.value)]
		public string RevisedValue { get; set; }

		#endregion

		#region Constructors

		public File(Uri uri) : base(uri) {}

		#endregion
	}
}

