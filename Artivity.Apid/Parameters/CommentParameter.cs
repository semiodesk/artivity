using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Api.Parameters
{
    public class CommentParameter
    {
        public string activity { get; set; }

        public string agent { get; set; }

        public DateTime creationTime { get; set; }

        public string text { get; set; }
    }
}
