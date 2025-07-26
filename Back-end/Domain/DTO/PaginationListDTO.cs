using System;

namespace Application.Common.DTO;

public record PaginationListDTO<TList>
{
    public int totalCount { get; set; } = 0;
    public required int pageNumber { get; set; } = 1;
    public required int pageSize { get; set; } = 10;
    public required TList listData { get; set; }
}
