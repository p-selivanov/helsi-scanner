using System.Collections.Generic;

namespace HelsiScanner.Dtos
{
    internal class EntityPage<T>
    {
        public IEnumerable<T> Data { get; set; }
    }
}
