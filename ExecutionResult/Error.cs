namespace ExecutionResult;

/// <summary>
/// The basic abstract error type for the result.
/// </summary>
/// <param name="Message"> Error message.</param>
public abstract record Error(string Message);