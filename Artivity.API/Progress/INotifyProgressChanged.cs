using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Apid.Helpers
{
    public interface INotifyProgressChanged
    {
        #region Events

        event ProgressChangedEventHandler ProgressChanged;

        #endregion
    }
}
