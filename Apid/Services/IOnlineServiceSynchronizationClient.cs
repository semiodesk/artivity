using Artivity.Apid.Synchronization;
using Artivity.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Apid
{
    public interface IOnlineServiceSynchronizationClient : IOnlineServiceClient
    {
        #region Methods

        Task<SynchronizationChangeset> TryGetChangesetAsync(OnlineAccount account);

        Task<ResourceSynchronizationState> TryPushAsync(OnlineAccount account, Uri uri, Uri typeUri);

        Task<ResourceSynchronizationState> TryPullAsync(OnlineAccount account, Uri uri, Uri typeUri);

        #endregion
    }
}
