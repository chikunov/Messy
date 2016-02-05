using System;
using System.Collections.Generic;

namespace Messy.DataStructures
{
    /// <summary>
    ///     Read-only collection of identifiers
    /// </summary>
    public interface IReadOnlyIdCollection : IReadOnlyCollection<Guid>
    {
    }
}
