using System;
using Nancy.IO;
using System.IO;

namespace Artivity.Api.Http
{
	public static class RequestBodyExtensions
	{
		public static string ReadAsString(this RequestStream requestStream)
		{
			using (var reader = new StreamReader(requestStream))
			{
				return reader.ReadToEnd();
			}
		}
	}
}

