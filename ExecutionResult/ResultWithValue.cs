using System.Diagnostics.CodeAnalysis;
using ExecutionResult.Exceptions;

namespace ExecutionResult;

/// <summary>
/// The result of the operation with value.
/// </summary>
/// <typeparam name="TValue"> Result value type.</typeparam>
/// <typeparam name="TError"> Result error type.</typeparam>
public readonly record struct Result<TValue, TError>
    where TError : Error
{
    /// <summary>
    /// Result value.
    /// </summary>
    public TValue? Value { get; }
    
    /// <summary>
    /// Result error.
    /// </summary>
    public TError? Error { get; }
    
    /// <summary>
    /// Result is success.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess { get; }
    
    /// <summary>
    /// Result is failure.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Error))]
    [MemberNotNullWhen(false, nameof(Value))]
    public bool IsFailure => !IsSuccess;

    /// <inheritdoc cref="Result{TValue, TError}"/>
    private Result(TError? error)
    {
        IsSuccess = error != default;
        Error = error;
        Value = default;
    }
    
    /// <inheritdoc cref="Result{TValue, TError}"/>
    private Result(TValue value)
        : this(default(TError))
    {
        Value = value;
    }

    /// <summary>
    /// Unwrap the value of an result.
    /// </summary>
    /// <returns> Value of result.</returns>
    /// <exception cref="Exception"> Unable to unwrap the value because the result <see cref="IsFailure"/> is true.</exception>
    public TValue Unwrap()
    {
        if (IsFailure)
        {
            const string message = "An error occurred when unpacking the value - " +
                                   "the result does not contain a value and has a failure result.";
            throw new ResultUnwrappingException(message + $" Error: {Error.Message}.");
        }

        return Value;
    }
    
    /// <summary>
    /// Unwrap the value of an result.
    /// </summary>
    /// <param name="message"> Customized exception message.</param>
    /// <returns> Value of result.</returns>
    /// <exception cref="Exception">  Unable to unwrap the value because the result <see cref="IsFailure"/> is true.</exception>
    public TValue Unwrap(string message)
    {
        if (IsFailure)
        {
            throw new ResultUnwrappingException(message);
        }

        return Value;
    }
    
    /// <summary>
    /// Unwrap the value of an result.
    /// </summary>
    /// <param name="message"> Customized exception message.</param>
    /// <returns> Value of result.</returns>
    /// <exception cref="Exception">  Unable to unwrap the value because the result <see cref="IsFailure"/> is true.</exception>
    public TValue Unwrap(Func<TError, string> message)
    {
        if (IsFailure)
        {
            throw new Exception(message(Error));
        }

        return Value;
    }
    
    /// <summary>
    /// Unwrap the value of an result.
    /// </summary>
    /// <param name="exception"> Customized exception.</param>
    /// <returns> Value of result.</returns>
    /// <exception cref="Exception">  Unable to unwrap the value because the result <see cref="IsFailure"/> is true.</exception>
    public TValue Unwrap<TException>(Func<TError, TException> exception)
        where TException : Exception
    {
        if (IsFailure)
        {
            throw exception(Error);
        }

        return Value;
    }
    
    /// <summary>
    /// Unwrap the value of an result.
    /// </summary>
    /// <returns> Returns the result value if <see cref="IsSuccess"/>, otherwise returns the passed <see cref="defaultValue"/></returns>
    public TValue UnwrapOr(TValue defaultValue)
    {
        return IsFailure ? defaultValue : Value;
    }
    
    /// <summary>
    /// Unwrap the value of an result.
    /// </summary>
    /// <returns> Returns the result value if <see cref="IsSuccess"/>, otherwise returns the passed <see cref="defaultValue"/></returns>
    public TValue UnwrapOr(Func<Error, TValue> defaultValue)
    {
        return IsFailure ? defaultValue(Error) : Value;
    }
    
    /// <summary>
    /// Unwrap the value of an result.
    /// </summary>
    /// <returns> Returns the result value if <see cref="IsSuccess"/>, otherwise returns default of type <see cref="TValue"/>></returns>
    public TValue? UnwrapOrDefault()
    {
        return IsFailure ? default(TValue) : Value;
    }

    /// <summary>
    /// Mapping values, depending on the result.
    /// </summary>
    /// <param name="onSuccess"> Mapping the value if the result IsSuccess.</param>
    /// <param name="onFailure"> Mapping the value if the result IsFailure.</param>
    /// <typeparam name="TMappedValue"> Type of mapping result.</typeparam>
    /// <returns> Mapping result.</returns>
    public TMappedValue Map<TMappedValue>(Func<TValue, TMappedValue> onSuccess, Func<TError, TMappedValue> onFailure)
    {
        return IsSuccess ? onSuccess(Value) : onFailure(Error);
    }
    
    /// <summary>
    /// Initialize the success result with value.
    /// </summary>
    /// <param name="value"> Value of an optional result.</param>
    /// <returns> Success result.</returns>
    public static Result<TValue, TError> Ok(TValue value) => new(value);
    
    /// <summary>
    /// Initialize the failure result.
    /// </summary>
    /// <param name="error"> Result error.</param>
    /// <returns> Failure result.</returns>
    public static Result<TValue, TError> Fail(TError error) => new(error);

    public static implicit operator Result<TValue, TError>(TValue value) => new(value);
    
    public static implicit operator Result<TValue, TError>(TError error) => new(error);

    /// <inheritdoc />
    public override string ToString() => $"Result is {(IsSuccess ? "Success." : $"Failure. Error: {Error.Message}.")}";
}