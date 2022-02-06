using MediatR;
using Microsoft.Extensions.Logging;

namespace Birthday.Telegram.Bot.ApplicationServices.Handlers;

/// <summary>
/// Базовый класс для обработчика команд
/// </summary>
/// <typeparam name="TRequest">Тип запроса. Должен реализовывать IRequest</typeparam>
/// <typeparam name="TService">Тип сервиса выполняющий действия</typeparam>
public abstract class BaseHandler<TRequest, TService> : IRequestHandler<TRequest>
    where TRequest : class, IRequest
    where TService : class
{
    /// <summary>
    /// Сервис обобщенный сервис
    /// </summary>
    protected readonly TService Service;

    /// <summary>
    /// Объект логгера
    /// </summary>
    protected readonly ILogger Logger;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="service">Объект обобщенного сервиса</param>
    /// <param name="logger">Объект логгера</param>
    protected BaseHandler(TService service, ILogger logger)
    {
        Service = service;
        Logger = logger;
    }

    /// <summary>
    /// Обработчик
    /// </summary>
    /// <param name="request">Объект запроса</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    public abstract Task<Unit> Handle(TRequest request, CancellationToken cancellationToken);
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="TRequest">Тип запроса. Должен реализовывать IRequest{TResponse}</typeparam>
/// <typeparam name="TResponse">Тип возвращаемого значения</typeparam>
/// <typeparam name="TService">Тип сервиса выполняющий действия</typeparam>
public abstract class BaseHandler<TRequest, TResponse, TService> : IRequestHandler<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
    where TService : class
{
    /// <summary>
    /// Сервис обобщенный сервис
    /// </summary>
    protected readonly TService Service;

    /// <summary>
    /// Объект логгера
    /// </summary>
    protected readonly ILogger Logger;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="service">Объект обобщенного сервиса</param>
    /// <param name="logger">Объект логгера</param>
    protected BaseHandler(TService service, ILogger logger)
    {
        Service = service;
        Logger = logger;
    }

    /// <summary>
    /// Обработчик
    /// </summary>
    /// <param name="request">Объект запроса</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Объект ответа после обработки запроса</returns>
    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}