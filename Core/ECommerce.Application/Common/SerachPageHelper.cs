using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Common
{
    public static class SerachPageHelper
    {
        public static IEnumerable<T> Filter<T>(this IEnumerable<T> source, string search, Func<T, string>[] properties)
        {
            if (string.IsNullOrWhiteSpace(search))
                return source;

            return source.Where(item =>
                properties.Any(prop =>
                    (prop(item) ?? "").Contains(search, StringComparison.OrdinalIgnoreCase)
                )
            );
        }

        public static IEnumerable<T> Paginate<T>(this IEnumerable<T> source, int pageNumber, int pageSize)
        {
            return source
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
        }
    }
}
