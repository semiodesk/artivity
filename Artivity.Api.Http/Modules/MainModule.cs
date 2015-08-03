using System;
using Nancy;
using System.Threading;
using Nancy.ModelBinding;

namespace Artivity.Api.Http
{
	public class MainModule : NancyModule
	{
		public MainModule()
		{
            Post["/artivity/1.0/activities"] = parameters => 
            {

                return AddActivity(Request); 
            };
		}

        protected string AddActivity(object o)
        {
            return "Hello";
        }
	}

    public class SimpleActivity
    {
        public string title { get; set; }
        public string url { get; set; }
        public string actor { get; set; }
    }
}

