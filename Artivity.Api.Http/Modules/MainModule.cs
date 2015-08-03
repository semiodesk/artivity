using System;
using Nancy;
using System.Threading;
using System.Collections.Generic;
using Nancy.ModelBinding;
using Semiodesk.Trinity;
using Artivity.Model.ObjectModel;

namespace Artivity.Api.Http
{
	public class MainModule : NancyModule
	{
        IStore _store;
        IModel _browserHistory;

        Dictionary<string, Application> _instruments = new Dictionary<string, Application>();

		public MainModule()
		{
            InitializeModel();

            Post["/artivity/1.0/activities"] = parameters => 
            {
                var b = this.Bind<SimpleActivity>();
                return AddActivity(b); 
            };
		}

        protected string AddActivity(SimpleActivity activity)
        {
            var view = _browserHistory.CreateResource<View>();
            view.Instrument = GetInstrument(activity.actor);
            var page = _browserHistory.CreateResource<Page>();
            page.DisplayName = activity.title;
            page.Url.Add(new Resource(activity.url));
            page.Commit();
            view.Object = page;
            var time = DateTime.Now;
            view.StartTime = time;
            view.EndTime = time;
            view.Commit();

            return "done";
        }

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

        private Application GetInstrument(string instrument)
        {
            if (_instruments.ContainsKey(instrument))
                return _instruments[instrument];

            var actorUri = new Uri(instrument);
            Application i;
            if (_browserHistory.ContainsResource(actorUri))
            {
                i = _browserHistory.GetResource<Application>(actorUri);
                _instruments[instrument] = i;
                return i;
            }

            i = _browserHistory.CreateResource<Application>(actorUri);
            i.Commit();
            _instruments[instrument] = i;
            return i;
        }

	}

    public class SimpleActivity
    {
        public string title { get; set; }
        public string url { get; set; }
        public string actor { get; set; }
        public string tab { get; set; }
    }
}

