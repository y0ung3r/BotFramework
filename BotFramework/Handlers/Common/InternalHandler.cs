﻿using System;
using System.Threading.Tasks;
using BotFramework.Handlers.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace BotFramework.Handlers.Common
{
    /// <summary>
    /// Представляет обработчик, который перенаправляет запрос из одной ветки обработчиков в другую
    /// </summary>
    internal sealed class InternalHandler : IRequestHandler
    {
        private readonly ILogger<InternalHandler> _logger;
        private readonly RequestDelegate _branch;
        private readonly Predicate<object> _condition;

        /// <summary>
        /// Базовый конструктор
        /// </summary>
        /// <param name="logger">Сервис логгирования</param>
        /// <param name="branch">Цепочка обработчиков</param>
        /// <param name="condition">Условие, при выполнении которого должна выполняться цепочка обработчиков</param>
        public InternalHandler(ILogger<InternalHandler> logger, RequestDelegate branch, Predicate<object> condition)
        {
            _logger = logger;
            _branch = branch;
            _condition = condition;
        }

        /// <summary>
        /// Обработать запрос
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <param name="nextHandler">Следующий обработчик по цепочке</param>
        public Task HandleAsync(object request, RequestDelegate nextHandler)
        {
            if (_condition(request))
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Текущий запрос перенаправляется во вложенную ветвь");
                }

                return _branch(request);
            }

            if (nextHandler != null) 
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Текущий запрос перенаправляется к следующему обработчику по цепочке");
                }

                return nextHandler(request);
            }

            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Запрос обработан");
            }

            return Task.CompletedTask;
        }
    }
}