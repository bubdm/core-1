using System;

namespace WebApplication1.Domain.WebModel.Admin
{
    public class PageWebModel
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public PageWebModel(int count, int page, int pageSize)
        {
            Page = page;
            PageSize = pageSize;
            TotalItems = count;
        }
        public int TotalPages => PageSize == 0 ? 0 : (int) Math.Ceiling((double) TotalItems / PageSize);
        public bool HasPreviousPage => Page > 1;
        public bool HasNextPage => (Page < TotalPages);
    }
}
