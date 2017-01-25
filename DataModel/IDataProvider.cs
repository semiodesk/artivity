using Semiodesk.Trinity;
using System;
namespace Artivity.DataModel
{
    public interface IDataProvider
    {
        string ConnectionString { get; set; }

        string NativeConnectionString { get; set; }

        string Username { get; set; }

        Uri Agents { get; set; }

        Uri Activities { get; set; }

        Uri WebActivities { get; set; }

        Uri Monitoring { get; set; }

        int Count { get; }
        void IncreaseCounter();
        void DecreaseCounter();
    }
}
