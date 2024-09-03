using FluentValidation;
using MediatR;

namespace Shared.Behaviors;
public class ValidationBehavior<TRequest,TResponse>
    (IEnumerable<IValidator<TRequest>> validators)
    :IPipelineBehavior<TRequest,TResponse>
    where TRequest : notnull,IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var validationResult = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResult
            .Where(result => result.Errors.Any())
            .SelectMany(result => result.Errors)
            .ToList();

        if (failures.Any())
        {
            throw new ValidationException(failures);
        }

        return await next();
    }
}
