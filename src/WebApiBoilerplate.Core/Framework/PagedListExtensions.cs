using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JetBrains.Annotations;
using NHibernate.Linq;
using WebApiBoilerplate.Protocol;

namespace WebApiBoilerplate.Core.Framework
{
    public static class PagedListExtensions
    {
        [NotNull]
        public static Task<PagedList<TResultItem>> ToPagedListAsync<TResultItem>(
            [NotNull] this IQueryable<TResultItem> query, 
            [CanBeNull] IPagedRequest request)
        {
            return ToPagedListAsync(query, item => item, request);
        }

        [NotNull]
        public static Task<PagedList<TResultItem>> ToPagedListAsync<TQueryItem, TResultItem>(
            [NotNull] this IQueryable<TQueryItem> query, 
            [NotNull] Expression<Func<TQueryItem, TResultItem>> converter,
            [CanBeNull] IPagedRequest request)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }
            if (converter == null)
            {
                throw new ArgumentNullException(nameof(converter));
            }

            if (request == null ||
                request.PageIndex == null &&
                request.PageSize == null)
            {
                return GetAllAsync(query, converter);
            }

            return GetPagedAsync(query, converter, request);
        }

        [NotNull]
        private static async Task<PagedList<TResultItem>> GetPagedAsync<TQueryItem, TResultItem>(
            [NotNull] IQueryable<TQueryItem> query,
            [NotNull] Expression<Func<TQueryItem, TResultItem>> converter,
            [NotNull] IPagedRequest request)
        {
            var totalCount = query.ToFutureValue(q => q.Count());

            int? pageSize = request.PageSize != null && request.PageSize > 0
                ? request.PageSize.Value
                : (int?)null;

            int pageNumber = 0;

            if (pageSize != null)
            {
                query = query.Take(pageSize.Value);

                if (request.PageIndex != null && request.PageIndex > 0)
                {
                    var skip = pageSize.Value * request.PageIndex.Value;
                    query = query.Skip(skip);
                }
            }

            var items = await query.Select(converter).ToListAsync().ConfigureAwait(false);

            return new PagedList<TResultItem>
            {
                Items = items,
                TotalCount = totalCount.Value,
                PageIndex = pageNumber,
                PageSize = pageSize ?? totalCount.Value
            };
        }

        [NotNull]
        private static async Task<PagedList<TResultItem>> GetAllAsync<TQueryItem, TResultItem>(
            [NotNull] IQueryable<TQueryItem> query, 
            [NotNull] Expression<Func<TQueryItem, TResultItem>> converter)
        {
            var items = await query.Select(converter).ToListAsync().ConfigureAwait(false);

            return new PagedList<TResultItem>
            {
                Items = items,
                TotalCount = items.Count,
                PageIndex = 0,
                PageSize = items.Count
            };
        }
    }
}
