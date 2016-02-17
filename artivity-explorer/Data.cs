using Artivity.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artivity.Explorer
{
    public sealed class Models
    {
        public ModelProvider Provider;

        private Models()
        {

        }

        public static Models Instance { get { return Nested.instance; } }

        private class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }

            internal static readonly Models instance = new Models();
        }

        public string ConnectionString { get; set; }

        public void InitializeStore()
        {
            Provider = ModelProviderFactory.CreateModelProvider(ConnectionString, null, Environment.UserName);
        }

    } 
}
