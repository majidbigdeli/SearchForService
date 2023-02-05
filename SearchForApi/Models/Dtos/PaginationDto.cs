using System.Collections.Generic;

namespace SearchForApi.Models.Dtos
{
    public class PaginationDto<T>
    {
        public long Total { get; set; }
        public IEnumerable<T> Items { get; set; }
    }

    public class SinglePaginationDto<T>
    {
        public long Total { get; set; }
        public T Item { get; set; }
    }
}