namespace PrimeFlix.Catalog.Domain.Exceptions;
public static class ExceptionMessage
{
    public static readonly string CATEGORY_NAME_NULL_OR_EMPTY = "Name Should be empty or null";
    public static readonly string CATEGORY_DESCRIPTION_NULL = "Description Should be null";
    public static readonly string CATEGORY_NAME_LESS_3_CHARS = "Name must not contain less than 3 characters ";
    public static string CATEGORY_NAME_MORE_255_CHARS = "Name must not contain more than 255 characters";
    public static string CATEGORY_DESCRIPTION_MORE_10_000_CHARS = "Description must not contain more 10.000 characters";
}
