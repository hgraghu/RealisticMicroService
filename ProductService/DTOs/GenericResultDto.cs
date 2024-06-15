namespace ProductService.DTOs
{
    public class GenericResultDto<T>
    {
        public int CurrentPage { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<T> ?Items { get; set; }
    }
}
