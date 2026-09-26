namespace SharedLibrary.Commons.Results
{
    public class PaginationResult<T>(int total, int pageSize, List<T> collection)
    {
        public int TotalCount { get; } = total;
        public int TotalPage { get; } = (int)Math.Ceiling(total / (double)pageSize);
        public List<T> Collection { get; } = collection;
    }
}
