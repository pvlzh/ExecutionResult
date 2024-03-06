using System.Diagnostics.CodeAnalysis;

namespace ExecutionResult;

/// <summary>
/// The result of the operation without value.
/// </summary>
/// <typeparam name="TError"> Result error type.</typeparam>
public readonly record struct Result<TError> where TError : Error
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
    private Result(TError? error)
    {
        IsSuccess = error != default;
        Error = error;
    }
    
    /// <summary>
    /// Initialize the success result.
    /// </summary>
    /// <returns> Success result.</returns>
    public static Result<TError> Ok() => new(default);

    /// <summary>
    /// Initialize the success result with error.
    /// </summary>
    /// <param name="error"> Error.</param>
    /// <returns> Failure result.</returns>
    public static Result<TError> Fail(TError error) => new(error);

    public static implicit operator Result<TError>(TError error) => new(error);

    /// <inheritdoc />
    public override string ToString() => $"Result is {(IsSuccess ? "Success." : $"Failure. Error: {Error.Message}.")}";
}
