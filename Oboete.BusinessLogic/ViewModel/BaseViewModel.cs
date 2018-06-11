using System.Runtime.Serialization;
using Oboete.Database.Entity;

namespace Oboete.BusinessLogic.ViewModel
{
    public partial class BaseViewModel<T>
        where T : BaseEntity, new()
    {
        public T Entity { get; protected set; }

        public BaseViewModel()
        {
            Entity = new T();
        }

        public BaseViewModel(T entity)
        {
            Entity = entity;
        }
    }
}