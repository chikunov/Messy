using System;
using System.Collections.Generic;

namespace Messy.DataStructures
{
    public static class IdCollectionExtensions
    {
        public static IdCollection ToIdCollection(this ICollection<Guid> collection)
        {
            return new IdCollection(collection);
        }
    }
}
