using System;
using Nancy;

namespace Artivity.Api.Http
{
	public class MainModule : NancyModule
	{
		public MainModule()
		{
			Get["/"] = parameters => { return "Hello World."; };
		}
	}
}

