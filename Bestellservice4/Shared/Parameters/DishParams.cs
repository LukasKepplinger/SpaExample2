using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bestellservice4.Shared.Params
{
    public class DishParams
    {
        public int CurrentPage { get; set; } = 1;
        const int maxPageSize = 20;
        private int pageSize = 10;
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
    }
}
