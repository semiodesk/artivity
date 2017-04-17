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

        bool BeginSynchronization(IAccoutOwner user, OnlineAccount account);

        IModelSynchronizationState EndSynchronization(IAccoutOwner user, OnlineAccount account);

        Task<int> TryGetSynchronizationState(IAccoutOwner user, OnlineAccount account);

        Task<SynchronizationChangeset> TryGetChangesetAsync(IAccoutOwner user, OnlineAccount account);

        Task<bool> TryPushAsync(OnlineAccount account, Uri uri, Uri typeUri, int revision, Uri context);

        Task<bool> TryPullAsync(OnlineAccount account, Uri uri, Uri typeUri, int revision, Uri context);

        #endregion
    }
}
