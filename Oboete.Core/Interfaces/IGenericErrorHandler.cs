using System;
using System.Collections.Generic;

namespace Oboete.Core
{
    public interface IGenericErrorHandler
    {
        IReadOnlyList<string> Errors { get; }
        bool HasErrors { get; }
    }

    public interface IGenericErrorHandler<T> : IGenericErrorHandler
    {
        T Result { get; }
    }
}