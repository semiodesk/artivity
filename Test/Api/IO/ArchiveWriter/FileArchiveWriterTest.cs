using Artivity.Api.IO;
using NUnit.Framework;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtivityTest.Api.IO.ArchiveWriter
{
    [TestFixture]
    public class FileArchiveWriterTest : StoreBasedTest
    {
        #region Members

        #endregion

        #region Constructors


        #endregion

        #region Methods

        [Test]
        public void ConstructorTest()
        {
            RevisionArchiveWriter writer = new RevisionArchiveWriter(PlatformProvider, ModelProvider);
        }

        //[Test]
        public void Test()
        {
            RevisionArchiveWriter writer = new RevisionArchiveWriter(PlatformProvider, ModelProvider);

            //writer.Write();
        }

        #endregion
    }
}
