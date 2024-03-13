# ExecutionResults
The dotnet library, which adds types of operation results and provides a convenient way to return an error instead of creating an exception.

## Introduction
The ExecutionResults library is designed to simplify error handling in your applications by providing a set of result types and a convenient way to return errors without creating exceptions. This approach can help you better control your code and make it more reliable.

## Installation
```dotnet add package ExecutionResults --version 0.1.0```
## Usage
The ExecutionResults library introduces several result types that you can use to represent the outcome of an operation. These types include:
1. `Option<TValue>`
2. `Result<TError>`
3. `Result<TValue, TError>`

To use these result types, you can include the ExecutionResults namespace in your code:
```csharp
using ExecutionResults;
```
### Using the `Option<TValue>` type
The `Option<TValue>` type must be used when the method may not return a result, while not generating any errors.
```csharp
    /// A sample  method for searching for an int type value.
    public Option<int> FindValue()
    {
        // implementation of the int type value search
        return Option<int>.Some(foundValue);
    }
    
    /// Shortened record of the return of the found value.
    public Option<int> FindValue()
    {
        // implementation of the int type value search
        return foundValue;
    }
    
    /// An example when the method could not find a value with the int type, and it is necessary to return none
    public Option<int> FindValue()
    {
        // implementation of the int type value search
        return Option<int>.None();
    }
    
    /// An example of using the received Option<int> from the FindValue method
    public void Main()
    {
        var foundValue = FindValue();
        var resultMessage = foundValue.Map(
            some: value => $"The found value is '{value}'", 
            none: () => "The value was not found");
        
        Console.WriteLine(resultMessage);
    }
```

### Using the `Result<TError>` type
The `Result<Error>` type should be used when the method should not return a result, while generating an error in case of failure.
```csharp
    /// you can declare your own error types
    public record SampleInvocationError() : Error("The value could not be calculated");

    /// An example of a method with a return success result.
    public Result<Error> Invoke()
    {
        // method implementation
        return Result<Error>.Ok();
    }

    /// An example of a method with a return error.
    public Result<Error> Invoke()
    {
        // method implementation
        return Result<Error>.Fail(new SampleInvocationError());
    }

    /// Shortened record of the return of the error.
    public Result<SampleInvocationError> Invoke()
    {
        // method implementation
        return new SampleInvocationError();
    }
    
    /// An example of using the received Result<Error> from the Invoke method
    public void Main()
    {
        var invocationResult = Invoke();
        if (invocationResult.IsFailure)
        {
            Console.WriteLine(invocationResult.Error.Message);
            return;
        }
        
        // some logic
    }
```

### Using the `Result<TValue, TError>` type
The `Result<TValue, TError>` type should be used when the method should return a result, while generating an error in case of failure.
```csharp
    /// you can declare your own error types
    public record EntityNotFoundError() : Error("Entity not found");
    public record AccessDeniedError() : Error("There are not enough permissions to perform the operation");

        
    /// An example of a method with a return custom error.
    public Result<Guid, Error> Handle()
    {
        // method implementation
        if (!entity.HasValue)
        {
            return Result<Guid, Error>.Fail(new EntityNotFoundError());
        }

        return Result<Guid, Error>.Ok(entity.Id);
    }

    /// An example of a method with a shortened return custom error.
    public Result<Guid, Error> Handle()
    {
        if (!IsAdmin())
        {
            return new AccessDeniedError();
        }
        // method implementation
    }

    /// An example of a method with a return success result.
    public Result<Guid, Error> Handle()
    {
        // method implementation
        return entity.Id;
    }
    
    /// An example of using the received Result<Value, Error> from the Handle method with http response mapping
    public IActionResult SomeMethod()
    {
        var result = Handle();
        return result.Map(
            onSuccess: id => new OkObjectResult(id),
            onFailure: error => error switch {
                EntityNotFoundError => new NotFoundObjectResult(error.Message),
                AccessDeniedError => new ForbidResult(),
            });
    }
```
## Conclusion
By using the provided result types and matching method, you can simplify error handling and make your code more reliable.
