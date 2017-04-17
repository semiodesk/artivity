using NUnit.Framework;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtivityTest.Api.IO.ArchiveWriter
{
    public abstract class StoreBasedTest
    {
        #region Members

        protected IStore Store;

        protected TestModelProvider ModelProvider;

        protected TestPlatformProvider PlatformProvider;

        #endregion

        #region Constructors


        #endregion

        #region Methods

        [SetUp]
        public void SetUp()
        {
            string connectionString = SetupClass.ConnectionString;

            Store = StoreFactory.CreateStore(connectionString);

            PlatformProvider = new TestPlatformProvider();
            ModelProvider = new TestModelProvider(Store, connectionString, SetupClass.NativeConnectionString);

        }

        [TearDown]
        public void TearDown()
        {
            Store.Dispose();
        }

        #endregion
    }
}
