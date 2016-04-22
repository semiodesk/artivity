using Semiodesk.Trinity;
using System;
namespace Artivity.DataModel
{
    public interface IModelProvider
    {
        IStore Store { get; }

        string ConnectionString { get; set; }

        string NativeConnectionString { get; set; }

        Uri Agents { get; set; }

        IModel AgentsModel { get; set; }

        Uri Activities { get; set; }

        IModel ActivitiesModel { get; set; }

        Uri WebActivities { get; set; }

        IModel WebActivitiesModel { get; set; }

        Uri Monitoring { get; set; }

        IModel MonitoringModel { get; set; }

        string Username { get; set; }

        Semiodesk.Trinity.IModel GetActivities(Semiodesk.Trinity.IStore store = null);

        Semiodesk.Trinity.IModel GetAgents(Semiodesk.Trinity.IStore store = null);

        Semiodesk.Trinity.IModelGroup GetAll();

        Semiodesk.Trinity.IModel GetAllActivities();

        Semiodesk.Trinity.IModel GetMonitoring(Semiodesk.Trinity.IStore store = null);

        Semiodesk.Trinity.IModel GetWebActivities(Semiodesk.Trinity.IStore store = null);

        void InitializeStore();
    }
}
