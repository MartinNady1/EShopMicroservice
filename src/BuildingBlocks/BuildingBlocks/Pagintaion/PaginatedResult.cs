using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingBlocks.Pagintaion
{
    public class PaginatedResult <TEntity>(List<TEntity> items, long count, int pageIndex, int pageSize) where TEntity : class
    {
        public List<TEntity> Items { get; set; }
        public long Count { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
