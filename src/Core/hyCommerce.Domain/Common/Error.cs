namespace hyCommerce.Domain.Common;

public class Error(string code, string? description = null)
{
    public string Code { get; } = code;
    public string? Description { get; } = description;

    public static readonly Error None = new(string.Empty);
    public static readonly Error Unknown = new("Errors.Unknown", "An unknown error occurred.");
    public static readonly Error NullValue = new("Errors.NullValue", "A null value occurred.");

    public static implicit operator Result(Error error) => Result.Failure(error);
}