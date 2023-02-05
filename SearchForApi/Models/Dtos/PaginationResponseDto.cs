using System.Collections.Generic;

namespace SearchForApi.Models.Dtos
{
    public class PaginationResponseDto<T> : ResponseDto<PaginationDto<T>>
    {
        public PaginationResponseDto(long total, IEnumerable<T> items) : base(
            new PaginationDto<T> { Total = total, Items = items })
        {

        }
    }

    public class SinglePaginationResponseDto<T> : ResponseDto<SinglePaginationDto<T>>
    {
        public SinglePaginationResponseDto(long total, T item) : base(
            new SinglePaginationDto<T> { Total = total, Item = item })
        {

        }
    }
}