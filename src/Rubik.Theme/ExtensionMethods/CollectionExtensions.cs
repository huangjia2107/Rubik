using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Rubik.Theme.ExtensionMethods
{
    public static class CollectionExtensions
    {
        public static Collection<T> AddRange<T>(this Collection<T> collection, IEnumerable<T> items)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            foreach (var each in items)
            {
                collection.Add(each);
            }

            return collection;
        }
    }
}
