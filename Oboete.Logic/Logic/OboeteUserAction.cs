using Oboete.Database;
using System;

namespace Oboete.Logic.Logic
{
    public class OboeteUserAction : ActionBase
    {
        #region Constructors

        public OboeteUserAction(OboeteContext context) : base(context) { }

        #endregion

        #region Public Functions

        public bool UserTokenIsValid()
        {
            // TODO Actual logic
            return true;
        }

        #endregion
    }
}