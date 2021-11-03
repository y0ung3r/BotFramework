﻿using BotFramework.Interfaces;

namespace BotFramework.Extensions
{
    /// <summary>
    /// Методы-расширения для <see cref="IRequestHandler"/>
    /// </summary>
    public static class RequestHandlerExtensions
    {
        /// <summary>
        /// Преобразует <see cref="IRequestHandler"/> в <see cref="RequestDelegate"/>
        /// </summary>
        /// <param name="requestHandler">Обработчик запроса</param>
        /// <param name="nextRequestDelegate">Следующий за текущим обработчик запроса</param>
        public static RequestDelegate ToRequestDelegate(this IRequestHandler requestHandler, RequestDelegate nextRequestDelegate)
        {
            return request => requestHandler.HandleAsync(request, nextRequestDelegate);
        }
    }
}
