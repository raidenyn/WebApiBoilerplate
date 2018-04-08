using System.Collections.Generic;
using JetBrains.Annotations;

namespace WebApiBoilerplate.Protocol
{
    [PublicAPI]
    public class PagedList<TItem>
    {
        [NotNull, ItemNotNull]
        public List<TItem> Items { get; set; }

        public int TotalCount { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }
    }

    [PublicAPI]
    public interface IPagedRequest
    {
        int? PageSize { get; }

        int? PageIndex { get; }
    }
}
