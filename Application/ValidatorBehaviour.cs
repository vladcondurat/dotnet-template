using System.Reflection;
using Domain.Common;
using FluentValidation;
using MediatR;

namespace Application;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any()) return await next();
        
        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
        var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

        if (failures.Count == 0) return await next();
            
        var errorMessages = failures.Select(f => f.ErrorMessage).ToList();

        if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
        {
            var resultType = typeof(TResponse).GetGenericArguments()[0];
            var errorResult = typeof(Result<>)
                .MakeGenericType(resultType)
                .GetMethod("ValidationError", BindingFlags.Static | BindingFlags.Public)
                ?.Invoke(null, new object[] { errorMessages });

            return (TResponse)errorResult!;
        }
        if (typeof(TResponse) == typeof(Result))
        {
            var errorResult = typeof(Result)
                .GetMethod("ValidationError", new[] { typeof(IEnumerable<string>) })
                ?.Invoke(null, new object[] { errorMessages });

            return (TResponse)errorResult!;
        }

        throw new ValidationException(failures);
    }
}