﻿using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Model.ObjectModel
{
    [RdfClass(PROV.Person)]
    public class Person : Resource
    {
		#region Members

		string GivenName { get; set; }

		string EmailAddress { get; set; }

		#endregion

        #region Constructor

        public Person(Uri uri) : base(uri) {}

        #endregion
    }
}
