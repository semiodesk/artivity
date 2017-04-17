using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Semiodesk.TinyVirtuoso;
using System.Reflection;
using System.IO;
using Semiodesk.Trinity;

namespace ArtivityTest
{
    [SetUpFixture]
    public class SetupClass
    {
        Virtuoso instance;

        public static string ConnectionString;

        public static string NativeConnectionString;

        public static string HostAndPort;

        [OneTimeSetUp]
        public void Setup()
        {
            OntologyDiscovery.AddAssembly(Assembly.GetExecutingAssembly());
            
            MappingDiscovery.RegisterAssembly(Assembly.GetExecutingAssembly());

            Artivity.DataModel.SemiodeskDiscovery.Discover();

            FileInfo i = new FileInfo(Assembly.GetExecutingAssembly().Location);
            DirectoryInfo dir = new DirectoryInfo(Path.Combine(i.DirectoryName, "nunit"));

            TinyVirtuoso v = new TinyVirtuoso(dir);
            instance = v.GetOrCreateInstance("NUnit");
            instance.Start(true);
            ConnectionString = instance.GetTrinityConnectionString();
            NativeConnectionString = instance.GetAdoNetConnectionString();
            HostAndPort = instance.Configuration.Parameters.ServerPort;
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            instance.Stop();
        }

    }
}
