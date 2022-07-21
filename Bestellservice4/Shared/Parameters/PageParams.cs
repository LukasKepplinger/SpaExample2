using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bestellservice4.Shared.Params
{
    public class PageParams
    {
        [Required]
        public int CurrentPage { get; set; } = 1;

        const int maxPageSize = 20;
        private int pageSize = 10;

        [Required]
        public int PageSize
        {
            get { return pageSize; }
            set 
            {
                if (value < maxPageSize && value > 0)
                    pageSize = value;
                else
                    pageSize = 10;
            }
        }

        //public static ValueTask<PageParams?> BindAsync(HttpContext context)
        //{

        //    int.TryParse(context.Request.Query["page"], out var currentPage);
        //    int.TryParse(context.Request.Query["page-size"], out var pageSize);

        //    var result = new PageParams
        //    {
        //        CurrentPage = currentPage,
        //        PageSize = pageSize
        //    };

        //    return ValueTask.FromResult<PageParams?>(result);
        //}

    }
}
