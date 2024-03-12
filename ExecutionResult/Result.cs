using System.Diagnostics.CodeAnalysis;

namespace ExecutionResult;

/// <summary>
/// The result of the operation without value.
/// </summary>
/// <typeparam name="TError"> Result error type.</typeparam>
public record class Result<TError> where TError : Error
{
    /// <summary>
    /// Result error.
    /// </summary>
    public TError? Error { get; }

    /// <summary>
    /// Result is success.
    /// </summary>
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess { get; }

    /// <summary>
    /// Result is failure.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Error))]
    public bool IsFailure => !IsSuccess;

    /// <inheritdoc cref="Result{TError}"/>
    public Result()
    {
        IsSuccess = true;
        Error = default;
    }
    
    /// <inheritdoc cref="Result{TError}"/>
    private Result(TError error)
    {
        IsSuccess = false;
        Error = error;
    }
    
    /// <summary>
    /// Mapping values, depending on the result.
    /// </summary>
    /// <param name="onSuccess"> Mapping the value if the result IsSuccess.</param>
    /// <param name="onFailure"> Mapping the value if the result IsFailure.</param>
    /// <typeparam name="TMappedValue"> Type of mapping result.</typeparam>
    /// <returns> Mapping result.</returns>
    public TMappedValue Map<TMappedValue>(Func<TMappedValue> onSuccess, Func<TError, TMappedValue> onFailure)
    {
        return IsSuccess ? onSuccess() : onFailure(Error);
    }

    /// <summary>
    /// Initialize the success result.
    /// </summary>
    /// <returns> Success result.</returns>
    public static Result<TError> Ok()
    {
        return new Result<TError>();
    }

    /// <summary>
    /// Initialize the success result with error.
    /// </summary>
    /// <param name="error"> Error.</param>
    /// <returns> Failure result.</returns>
    public static Result<TError> Fail(TError error)
    {
        if (error == default)
        {
            throw new ArgumentNullException(nameof(error));
        }

        return new Result<TError>(error);
    }

    public static implicit operator Result<TError>(TError error) => new(error);

    /// <inheritdoc />
    public override string ToString() => $"Result is {(IsSuccess ? "Success." : $"Failure. Error: {Error.Message}.")}";
}
