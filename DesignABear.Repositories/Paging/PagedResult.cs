using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignABear.Repositories.Paging
{
    public class PagedResult<T>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
        public List<T> Items { get; set; } = new List<T>();
    }
}
