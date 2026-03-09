namespace WayPoint.Application.Common.Models;

/// <summary>
/// Common pagination parameters for paginated queries
/// Used in queries that return paged results
/// </summary>
public record PaginationParams
{
    private const int MaxPageSize = 100;
    private const int DefaultPageSize = 10;

    /// <summary>Page number (1-based)</summary>
    public int Page { get; init; } = 1;

    /// <summary>Number of items per page (1-100)</summary>
    public int PageSize { get; init; } = DefaultPageSize;

    /// <summary>Validated page number (minimum 1)</summary>
    public int ValidPage => Page < 1 ? 1 : Page;

    /// <summary>Validated page size (1-100)</summary>
    public int ValidPageSize => PageSize switch
    {
        < 1 => DefaultPageSize,
        > MaxPageSize => MaxPageSize,
        _ => PageSize
    };

    /// <summary>Number of items to skip (for EF Core Skip/Take)</summary>
    public int Skip => (ValidPage - 1) * ValidPageSize;

    /// <summary>Number of items to take (for EF Core Skip/Take)</summary>
    public int Take => ValidPageSize;
}