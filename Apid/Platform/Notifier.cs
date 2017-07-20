using Artivity.Api.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Apid.Platform
{
    class DummyNotifier : INotifier
    {
        public void PublishEntityAdded(Api.Parameters.Parameter parameter) {}
        public void PublishEntityUpdated(Api.Parameters.Parameter parameter) {}
        public void PublishEntityRemoved(Semiodesk.Trinity.UriRef parameter) {}
    }
}
