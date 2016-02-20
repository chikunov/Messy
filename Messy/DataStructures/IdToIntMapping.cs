using System;
using System.Collections.Generic;

namespace Messy.DataStructures
{
    /// <summary>
    ///     Identifier to integer both-ways mapper. Usefull for third-party libraries that don't support guids.
    /// </summary>
    public class IdToIntMapping
    {
        public IReadOnlyDictionary<int, Guid> IdByInt => _idByInt;
        public IReadOnlyDictionary<Guid, int> IntById => _intById;

        public int this[Guid index] => _intById[index];
        public Guid this[int index] => _idByInt[index];

        private readonly Dictionary<int, Guid> _idByInt;
        private readonly Dictionary<Guid, int> _intById;

        public IdToIntMapping()
        {
            _intById = new Dictionary<Guid, int>();
            _idByInt = new Dictionary<int, Guid>();
        }

        public IdToIntMapping(IdCollection ids)
        {
            var index = 1;
            foreach (var id in ids)
            {
                _intById.Add(id, index);
                _idByInt.Add(index, id);

                index++;
            }
        }
    }
}