using System;

namespace Application.Common.Shared;

public class PaginatedListAndSearchRequestDto
{
    public int PageNumber {get; set;} = 1;
    public int PageSize {get; set;} = 10;
    public string? search {get; set;}
}
