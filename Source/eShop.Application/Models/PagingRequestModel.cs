using System.ComponentModel.DataAnnotations;

namespace eShop.Application.Models;

public record PagingRequestModel(int Page, int Results, string OrderBy, string SortOrder) : IValidatableObject
{
    private readonly string[] _sortOrderAllowedValues = { "asc", "desc" };

    /// <summary>
    /// Gets or sets the page number.
    /// </summary>
    /// <example>1</example>
    public int Page { get; init; } = Page;

    /// <summary>
    /// Gets or sets the number of items to retrieve from the beginning.
    /// </summary>
    /// <example>10</example>
    public int Results { get; init; } = Results;

    /// <summary>
    /// Gets or sets the field name to order by.
    /// </summary>
    /// <example>id</example>
    public string OrderBy { get; init; } = OrderBy;

    /// <summary>
    /// Gets or sets the sort order. Allowed values are 'asc' or 'desc'
    /// </summary>
    /// <example>asc</example>
    public string SortOrder { get; init; } = SortOrder;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (SortOrder is not null && !_sortOrderAllowedValues.Contains(SortOrder, StringComparer.InvariantCultureIgnoreCase))
        {
            yield return new ValidationResult(
                $"Allowed values: {string.Join(", ", _sortOrderAllowedValues)}.", new[] { nameof(SortOrder) });
        }
    }
}
