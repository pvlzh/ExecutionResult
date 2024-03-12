using System.Diagnostics.CodeAnalysis;
using ExecutionResult.Exceptions;

namespace ExecutionResult;

/// <summary>
/// An optional result that does not contain an error.
/// </summary>
/// <typeparam name="TValue"> Тип результата.</typeparam>
public record class Option<TValue>
{
    /// <summary>
    /// Result.
    /// </summary>
    public TValue? Value { get; }

    /// <summary>
    /// The result has been assigned and is not equal to default.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Value))]
    public bool HasValue { get; }

    /// <inheritdoc cref="Option{TValue}"/>
    private Option()
    {
        Value = default;
        HasValue = false;
    }

    /// <inheritdoc cref="Option{TValue}"/>
    private Option(TValue value)
    {
        Value = value;
        HasValue = true;
    }

    /// <summary>
    /// Unwrap the value of an optional result.
    /// </summary>
    /// <returns> Value of optional result.</returns>
    /// <exception cref="Exception"> An error occurred when unpacking the value - the result does not contain the value.</exception>
    public TValue Unwrap()
    {
        if (!HasValue)
        {
            throw new ResultUnwrappingException("An error occurred when unpacking the value - " +
                                                "the result does not contain the value.");
        }

        return Value;
    }
    
    /// <summary>
    /// Unwrap the value of an optional result.
    /// </summary>
    /// <param name="message"> Customized exception message.</param>
    /// <returns> Value of optional result.</returns>
    /// <exception cref="Exception"> An error occurred when unpacking the value - the result does not contain the value.</exception>
    public TValue Unwrap(string message)
    {
        if (!HasValue)
        {
            throw new ResultUnwrappingException(message);
        }

        return Value;
    }
    
    /// <summary>
    /// Unwrap the value of an optional result.
    /// </summary>
    /// <param name="exception"> Customized exception.</param>
    /// <returns> Value of optional result.</returns>
    /// <exception cref="Exception"> An error occurred when unpacking the value - the result does not contain the value.</exception>
    public TValue Unwrap<TException>(Func<TException> exception)
        where TException : Exception
    {
        if (!HasValue)
        {
            throw exception();
        }

        return Value;
    }
    
    /// <summary>
    /// Unwrap the value of an optional result.
    /// </summary>
    /// <returns> Returns the result value if <see cref="HasValue"/>, otherwise returns the passed <see cref="defaultValue"/></returns>
    public TValue UnwrapOr(TValue defaultValue)
    {
        return !HasValue ? defaultValue : Value;
    }
    
    /// <summary>
    /// Unwrap the value of an optional result.
    /// </summary>
    /// <returns> Returns the result value if <see cref="HasValue"/>, otherwise returns default of type <see cref="TValue"/>></returns>
    public TValue? UnwrapOrDefault()
    {
        return !HasValue ? default(TValue) : Value;
    }
    
    /// <summary>
    /// Mapping values, depending on the result.
    /// </summary>
    /// <param name="someMapping"> Mapping the value if the optional result has value.</param>
    /// <param name="noneMapping"> Mapping the value if the optional result not contain value.</param>
    /// <typeparam name="TMappedValue"> Type of mapping result.</typeparam>
    /// <returns> Mapping result.</returns>
    public TMappedValue Map<TMappedValue>(Func<TValue, TMappedValue> someMapping, Func<TMappedValue> noneMapping)
    {
        return HasValue ? someMapping(Value) : noneMapping();
    }

    /// <summary>
    /// Initialize the result with value.
    /// </summary>
    /// <param name="value"> Value of an optional result.</param>
    /// <returns> Optional result with value.</returns>
    public static Option<TValue> Some(TValue value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return new Option<TValue>(value);
    }

    /// <summary>
    /// Initialize the result without value.
    /// </summary>
    /// <returns> Optional result without value.</returns>
    public static Option<TValue> None()
    {
        return new Option<TValue>();
    }
    
    public static implicit operator Option<TValue>(TValue value) => new (value);

    /// <inheritdoc />
    public override string ToString() => $"Optional result {(HasValue ? "has value." : "not contain value.")}";
}