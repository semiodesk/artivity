using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Artivity.Apid
{
    public interface IViewModule
    {
        string Path { get; }

        string Namespace { get; }

        string DocumentRoot { get; }

        string DocumentIndex { get; }
    }
}
