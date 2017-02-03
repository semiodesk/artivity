using Artivity.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Apid.Synchronization
{
    public interface IOnlineServiceSynchronizationProvider
    {
        #region Methods

        void RegisterServiceClient(IOnlineServiceSynchronizationClient client);

        List<OnlineAccountSynchronizationState> TrySynchronize();

        #endregion
    }
}
