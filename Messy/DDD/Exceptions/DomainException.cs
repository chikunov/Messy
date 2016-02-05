using System;

namespace Messy.DDD.Exceptions
{
    public class DomainException<T> : Exception
    {
        public DomainException(string format, params object[] args)
            : base($"[{typeof (T).Name}] {string.Format(format, args)}")
        {
        }

        public DomainException(Exception ex, string format, params object[] args)
            : base($"[{typeof (T).Name}] {string.Format(format, args)}", ex)
        {
        }
    }
}