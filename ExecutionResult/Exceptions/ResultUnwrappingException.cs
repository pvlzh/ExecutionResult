namespace ExecutionResult.Exceptions;

/// <summary>
/// An exception that is thrown when unpacking the result.
/// </summary>
internal class ResultUnwrappingException : Exception
{
    /// <inheritdoc cref="ResultUnwrappingException"/>
    public ResultUnwrappingException(string errorMessage) : base(errorMessage)
    {
    }
}