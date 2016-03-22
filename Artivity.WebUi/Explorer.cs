using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.WebUi
{
    public class Explorer : NancyModule
    {
        public Explorer()
        {
            Get["/explorer"] = parameters => { return View["home.sshtml"]; };
        }

    }
}
