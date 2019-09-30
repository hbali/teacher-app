using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    public static class EnumerableExtensions
    {
        public static bool HasSameElement<T>(this IEnumerable<T> list, IEnumerable<T> other)
        {
            return list.Intersect(other).Count() > 0;
        }
    }
}
