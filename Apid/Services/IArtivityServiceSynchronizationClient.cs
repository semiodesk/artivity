using Artivity.Apid.Synchronization;
using Artivity.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Apid
{
    public interface IArtivityServiceSynchronizationClient : IOnlineServiceClient
    {
        #region Methods

        bool BeginSynchronization(IPerson user, OnlineAccount account);

        IModelSynchronizationState EndSynchronization(IPerson user, OnlineAccount account);

        Task<int> TryGetSynchronizationState(IPerson user, OnlineAccount account);

        Task<SynchronizationChangeset> TryGetChangesetAsync(IPerson user, OnlineAccount account);

        Task<bool> TryPushAsync(OnlineAccount account, Uri uri, Uri typeUri, int revision);

        Task<bool> TryPullAsync(OnlineAccount account, Uri uri, Uri typeUri, int revision);

        #endregion
    }
}
