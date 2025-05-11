namespace BlinkHttp.Swagger;

/// <summary>
/// Represents the various schema types that can be used in Swagger documentation.
/// </summary>
public enum SchemaType
{
    /// <summary>
    /// Represents a string type.
    /// </summary>
    String,

    /// <summary>
    /// Represents an integer type.
    /// </summary>
    Integer,

    /// <summary>
    /// Represents a number type, which can include floating-point values.
    /// </summary>
    Number,

    /// <summary>
    /// Represents a boolean type.
    /// </summary>
    Boolean,

    /// <summary>
    /// Represents an array type.
    /// </summary>
    Array,

    /// <summary>
    /// Represents an object type.
    /// </summary>
    Object,

    /// <summary>
    /// Represents a null type.
    /// </summary>
    Null
}
