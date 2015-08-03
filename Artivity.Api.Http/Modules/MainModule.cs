using System;
using System.Threading;
using System.Collections.Generic;
using Nancy;
using Nancy.ModelBinding;
using Semiodesk.Trinity;
using Artivity.Model.ObjectModel;
using System.Diagnostics;

namespace Artivity.Api.Http
{
	public class MainModule : NancyModule
	{
		#region Members

        IStore _store;

        IModel _browserHistory;

        Dictionary<string, Application> _instruments = new Dictionary<string, Application>();

		#endregion

		#region Constructors

		public MainModule()
		{
            InitializeModel();

            Post["/artivity/1.0/activities"] = parameters => 
            {
                var b = this.Bind<SimpleActivity>();
                return AddActivity(b); 
            };
		}

		#endregion

		#region Methods

		private void InitializeModel()
		{
			_store = StoreFactory.CreateStoreFromConfiguration("virt0");

			Uri browserHistory = new Uri("http://semiodesk.com/artivity/browserHistory/");

			if (_store.ContainsModel(browserHistory))
			{
				_browserHistory = _store.GetModel(browserHistory);
			}
			else
			{
				_browserHistory = _store.CreateModel(browserHistory);
			}
		}

		protected HttpStatusCode AddActivity(SimpleActivity activity)
        {
			DateTime now = DateTime.Now;

			Console.WriteLine("{0}: {1}#{2}: {3}", now, activity.actor, activity.tab, activity.url);

			Page page = _browserHistory.CreateResource<Page>();
			page.DisplayName = activity.title;
			page.Urls.Add(new Resource(activity.url));
			page.Commit();

            View view = _browserHistory.CreateResource<View>();
            view.Instrument = GetInstrument(activity.actor);
            view.Object = page;
            view.StartTime = now;
            view.EndTime = now;
            view.Commit();

            return HttpStatusCode.OK;
        }

        private Application GetInstrument(string actorId)
        {
			if (_instruments.ContainsKey(actorId))
			{
				return _instruments[actorId];
			}

            Uri actorUri = new Uri(actorId);

            Application instrument;

            if (_browserHistory.ContainsResource(actorUri))
            {
                instrument = _browserHistory.GetResource<Application>(actorUri);
            }
			else
			{
				instrument = _browserHistory.CreateResource<Application>(actorUri);
				instrument.Commit();
			}

            _instruments[actorId] = instrument;

            return instrument;
        }

		#endregion
	}

    public class SimpleActivity
    {
        public string title { get; set; }
        public string url { get; set; }
        public string actor { get; set; }
        public string tab { get; set; }
    }
}

