namespace ProductService.DataAccess
{
    public class EntityPagination
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortField { get; set; } = "Id";
        public string SortOrder { get; set; } = "asc";
    }
}
