using System;
using System.Collections.Generic;

namespace Messy.DataStructures
{
    /// <summary>
    ///     Collection of identifiers 
    /// </summary>
    public class IdCollection : HashSet<Guid>, IReadOnlyIdCollection
    {
        public IdCollection()
        {
        }

        public IdCollection(IEnumerable<Guid> collection) : base(collection)
        {
        }

        public IdCollection(Guid initialValue) : base(new [] { initialValue })
        {
        }
    }
}