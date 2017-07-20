using Artivity.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Api.Parameters
{
    public abstract class Parameter : IValidatable
    {
        public string MessageType { get { return this.GetType().Name; } }

        public abstract bool Validate();
    }
}
