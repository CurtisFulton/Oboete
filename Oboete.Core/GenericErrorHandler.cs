using System;
using System.Collections.Generic;

namespace Oboete.Core
{
    public class GenericErrorHandler : IGenericErrorHandler
    {
        private List<string> _errors;

        public GenericErrorHandler()
        {
            _errors = new List<string>();

            Errors = _errors.AsReadOnly();
        }

        public IReadOnlyList<string> Errors { get; private set; }

        public bool HasErrors => Errors.Count > 0;

        public void AddError(string error)
        {
            _errors.Add(error);
        }
    }

    // Error handler with a return type
    public class GenericErrorHandler<T> : GenericErrorHandler, IGenericErrorHandler<T>
    {
        public T Result { get; set; }
    }
}