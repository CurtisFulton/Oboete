using System.Runtime.Serialization;
using Oboete.Database.Entity;

namespace Oboete.Logic
{
    public partial class LogicBase<T>
        where T : BaseEntity, new()
    {
        public T Entity { get; protected set; }

        public LogicBase()
        {
            Entity = new T();
        }

        public LogicBase(T entity)
        {
            Entity = entity;
        }
    }
}