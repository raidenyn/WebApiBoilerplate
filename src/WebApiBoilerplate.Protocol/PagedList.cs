using System.Collections.Generic;
using JetBrains.Annotations;

namespace WebApiBoilerplate.Protocol
{
    /// <summary>
    /// List of object with additional paging information
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    [PublicAPI]
    public class PagedList<TItem>
    {
        /// <summary>
        /// List items
        /// </summary>
        [NotNull, ItemNotNull]
        public List<TItem> Items { get; set; }

        /// <summary>
        /// Total count of objects
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Maximum object count in the list
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Shift of page number
        /// </summary>
        public int PageIndex { get; set; }
    }

    /// <summary>
    /// Standard part of request
    /// </summary>
    [PublicAPI]
    public interface IPagedRequest
    {
        /// <summary>
        /// Maximum object count in the list
        /// </summary>
        int? PageSize { get; }

        /// <summary>
        /// Shift of page number
        /// </summary>
        int? PageIndex { get; }
    }
}
