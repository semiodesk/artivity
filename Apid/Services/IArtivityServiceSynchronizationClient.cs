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

        bool BeginSynchronization(IUserAgent user, OnlineAccount account);

        IModelSynchronizationState EndSynchronization(IUserAgent user, OnlineAccount account);

        Task<int> TryGetSynchronizationState(IUserAgent user, OnlineAccount account);

        Task<SynchronizationChangeset> TryGetChangesetAsync(IUserAgent user, OnlineAccount account);

        Task<bool> TryPushAsync(OnlineAccount account, Uri uri, Uri typeUri, int revision);

        Task<bool> TryPullAsync(OnlineAccount account, Uri uri, Uri typeUri, int revision);

        #endregion
    }
}
