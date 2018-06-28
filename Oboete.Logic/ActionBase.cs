using System;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oboete.Database;
using Oboete.Database.Entity;

namespace Oboete.Logic
{
    public partial class ActionBase
    {
        [JsonIgnore]
        protected OboeteContext DbContext { get; private set; }

        #region Constructors

        protected ActionBase()
        {
            DbContext = new OboeteContext(Config.ConnectionString);
        }

        protected ActionBase(OboeteContext context)
        {
            DbContext = context;
        }

        #endregion
    }
}