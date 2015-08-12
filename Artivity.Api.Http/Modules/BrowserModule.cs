// LICENSE:
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// AUTHORS:
//
//  Moritz Eberl <moritz@semiodesk.com>
//  Sebastian Faubel <sebastian@semiodesk.com>
//
// Copyright (c) Semiodesk GmbH 2015

using Artivity.Model.ObjectModel;
using Artivity.Api.Http.Parameters;
using Semiodesk.Trinity;
using System;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using Nancy;
using Nancy.ModelBinding;

namespace Artivity.Api.Http
{
	public class BrowserModule : NancyModule
	{
		#region Members

        private IModel _activities;

        private static Dictionary<string, bool> _actors = new Dictionary<string, bool>();

        private static Dictionary<string, Application> _instruments = new Dictionary<string, Application>();

		#endregion

		#region Constructors

		public BrowserModule()
		{
            Post["/artivity/1.0/activities"] = parameters => 
            {
                InitializeModel();

                return AddActivity(this.Bind<ActivityParameters>()); 
            };

            Get["/artivity/1.0/status"] = parameters =>
            {
                return GetStatus(this.Bind<ActorParameters>());
            };

            Post["/artivity/1.0/status"] = parameters =>
            {
                return SetStatus(this.Bind<ActorParameters>());
            };
		}

		#endregion

		#region Methods

		private void InitializeModel()
		{
			IStore store = StoreFactory.CreateStoreFromConfiguration("virt0");

			Uri browserHistory = new Uri("http://semiodesk.com/artivity/browserHistory/");

			if (store.ContainsModel(browserHistory))
			{
				_activities = store.GetModel(browserHistory);
			}
			else
			{
				_activities = store.CreateModel(browserHistory);
			}
		}

        protected HttpStatusCode AddActivity(ActivityParameters p)
        {
            if (string.IsNullOrEmpty(p.actor)
                || string.IsNullOrEmpty(p.title)
                || string.IsNullOrEmpty(p.url)
                || !Uri.IsWellFormedUriString(p.url, UriKind.Absolute))
            {
                return HttpStatusCode.BadRequest;
            }

            if(!_actors.ContainsKey(p.actor) || !_actors[p.actor])
            {
                return HttpStatusCode.Locked;
            }

			DateTime now = DateTime.Now;

			Console.WriteLine("[{0}] {1}#{2}: {3}", now, p.actor, p.tab, p.url);

			Page page = _activities.CreateResource<Page>();
			page.DisplayName = p.title;
			page.Urls.Add(new Resource(p.url));
			page.Commit();

            View view = _activities.CreateResource<View>();
            view.Instrument = GetInstrument(p.actor);
            view.Object = page;
            view.StartTime = now;
            view.EndTime = now;
            view.Commit();

            return HttpStatusCode.OK;
        }

        protected Response GetStatus(ActorParameters p)
        {
            ActorParameters result = new ActorParameters() { actor = p.actor, enabled = false };

            if(p.actor != null && _actors.ContainsKey(p.actor))
            {
                result.enabled = _actors[p.actor];
            }

            return Response.AsJson(result);
        }

        protected Response SetStatus(ActorParameters p)
        {
            if (p.actor == null) return HttpStatusCode.BadRequest;

            if (p.enabled != null)
            {
                _actors[p.actor] = Convert.ToBoolean(p.enabled);
            }
            else if(_actors.ContainsKey(p.actor))
            {
                p.enabled = _actors[p.actor];
            }

            // We return the request so that the plugin can set the server's enabled status.
            return Response.AsJson(p);
        }

        private Application GetInstrument(string actorId)
        {
			if (_instruments.ContainsKey(actorId))
			{
				return _instruments[actorId];
			}

            Uri actorUri = new Uri(actorId);

            Application instrument;

            if (_activities.ContainsResource(actorUri))
            {
                instrument = _activities.GetResource<Application>(actorUri);
            }
			else
			{
				instrument = _activities.CreateResource<Application>(actorUri);
				instrument.Commit();
			}

            _instruments[actorId] = instrument;

            return instrument;
        }

		#endregion
	}
}

