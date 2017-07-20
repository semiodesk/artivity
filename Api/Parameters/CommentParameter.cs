using Artivity.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Api.Parameters
{

    public class CommentParameter : Parameter
    {
        #region Members

        public CommentTypes type { get; set; }

        public string agent { get; set; }

        public string primarySource { get; set; }

        public DateTime startTime { get; set; }

        public DateTime endTime { get; set; }

        public string message { get; set; }

        public string[] marks { get; set; }

        public List<AssociationParameter> associations { get; set; }

        #endregion

        #region Constructors

        public CommentParameter()
        {
            associations = new List<AssociationParameter>();
        }

        #endregion

        #region Methods

        public override bool Validate()
        {
            return !string.IsNullOrEmpty(message)
                && !string.IsNullOrEmpty(agent)
                && !string.IsNullOrEmpty(primarySource)
                && DateTime.MinValue < startTime
                && startTime <= endTime;
        }

        #endregion
    }

    public class AssociationParameter
    {
        #region Members

        public string agent { get; set; }

        public string role { get; set; }

        #endregion
    }

    public enum CommentTypes
    {
        Comment,
        FeedbackRequest,
        FeedbackResponse,
        ApprovalRequest,
        ApprovalResponse
    };

}
