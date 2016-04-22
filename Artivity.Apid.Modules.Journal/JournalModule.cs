using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Apid.Modules
{
    public class JournalModule : NancyModule, IViewModule
    {
        #region Members

        public string Path
        {
            get { return "/artivity/app/journal/1.0/"; }
        }

        private string _namespace;

        public string Namespace
        {
            get { return _namespace; }
        }

        public string DocumentRoot
        {
            get { return "app/"; }
        }

        public string DocumentIndex
        {
            get { return "index.html"; }
        }

        #endregion

        #region Constructors

        public JournalModule()
        {
            _namespace = GetType().Assembly.GetName().Name + '.' + DocumentRoot.TrimEnd('/').Replace('/', '.');
        }

        #endregion
    }
}
