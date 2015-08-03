using System;
using Nancy.Hosting.Self;

namespace Artivity.Api.Http
{
	class HttpService
	{
		public const int Port = 8272;

		public static void Main(string[] args)
		{

			HostConfiguration config = new HostConfiguration();
			config.UrlReservations.CreateAutomatically = true;

			using (var host = new NancyHost(new Uri("http://localhost:" + Port)))
			{
				host.Start();

				Console.WriteLine("Artivity HTTP REST API, Version 1.1");
				Console.WriteLine();
				Console.WriteLine("Listening on port {0}..", Port);

				Console.ReadLine();
			}
		}
	}
}
