using System;
using System.Reflection;
using Semiodesk.Trinity;

namespace Artivity.Model
{
    public static class SemiodeskDiscovery
    {
        public static void Discover()
        {
            OntologyDiscovery.AddAssembly(Assembly.GetExecutingAssembly());
            MappingDiscovery.RegisterCallingAssembly();
        }
    }
}
