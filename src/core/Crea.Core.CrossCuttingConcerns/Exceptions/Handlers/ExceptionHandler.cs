using FluentValidation;

namespace Crea.Core.CrossCuttingConcerns.Exceptions.Handlers;

public abstract class ExceptionHandler
{
    public Task HandleExceptionAsync(Exception exception)
    {
        if (exception is BusinessException businessException)
        {
            return HandleException(businessException);
        }

        if (exception is ValidationException validationException)
        {
            return HandleException(validationException);
        }

        if (exception is UnauthorizedAccessException unauthorizedAccessException)
        {
            return HandleException(unauthorizedAccessException);
        }

        return HandleException(exception);
    }

    protected abstract Task HandleException(BusinessException exception);
    protected abstract Task HandleException(ValidationException exception);
    protected abstract Task HandleException(UnauthorizedAccessException exception);
    protected abstract Task HandleException(Exception exception);
}
