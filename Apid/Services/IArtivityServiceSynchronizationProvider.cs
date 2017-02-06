using Artivity.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Apid.Synchronization
{
    public interface IArtivityServiceSynchronizationProvider
    {
        #region Methods

        void RegisterClient(IArtivityServiceSynchronizationClient client);

        IEnumerable<IModelSynchronizationState> TrySynchronize();

        #endregion
    }
}
