using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bestellservice4.Shared.Helper
{
    public class PageOf<T> : List<T>
    {
        public PageMetaData PageMetaData { get; set; }

        public PageOf()
        {
            PageMetaData = new PageMetaData
            {
                CurrentPage = 1,
                PageSize = 2,
                TotalCount = 2,
                TotalPages = 1
            };
        }

        public PageOf(List<T> dtos, int currentPage, int pageSize, int totalCount)
        {
            PageMetaData = new PageMetaData
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };

            AddRange(dtos);
        }

        public static Task<PageOf<T>> ToPagesAsync(IQueryable<T> items, int currentPage, int pageSize)
        {
            var totalCount = items.Count();
            var pageItems = items.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            var page = new PageOf<T>(pageItems, currentPage, pageSize, totalCount);
            return Task.FromResult(page);
        }
    }
}
