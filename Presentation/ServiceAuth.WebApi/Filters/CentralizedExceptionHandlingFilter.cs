using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using ServiceAuth.WebApi.Models.Responses;
using ServiceAuth.Domain.Exceptions;

namespace ServiceAuth.WebApi.Filters
{
    public class CentralizedExceptionHandlingFilter : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var (message, statusCode) = TryGetUserMessageFromException(context);

            if (message != null && statusCode != 0)
            {
                context.Result = new ObjectResult(new ErrorResponse(message, statusCode))
                {
                    StatusCode = statusCode
                };
                context.ExceptionHandled = true;
            }
        }

        private (string?, int) TryGetUserMessageFromException(ExceptionContext context)
        {
            return context.Exception switch
            {
                EmailAlreadyExistsException => ("Аккаунт с таким email уже зарегистрирован", StatusCodes.Status400BadRequest),
                PasswordNotChangedException => ("Новый пароль не должен совпадать со старым", StatusCodes.Status400BadRequest),
                AccountNotFoundException => ("Аккаунт с таким e-mail не найден", StatusCodes.Status400BadRequest),
                InvalidPasswordException => ("Неверный пароль", StatusCodes.Status400BadRequest),
                InvalidEmailException => ("Некорректный адрес e-mail адреса", StatusCodes.Status400BadRequest),
                Exception => ("Внутренняя ошибка сервера", StatusCodes.Status500InternalServerError),
                _ => (null, 0)
            };
        }
    }
}