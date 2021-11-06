using BotFramework.Attributes;
using BotFramework.Handlers.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BotFramework.Extensions
{
    /// <summary>
    /// Методы-расширения для <see cref="ICommandHandler"/>
    /// </summary>
    public static class CommandHandlerExtensions
    {
        /// <summary>
        /// Возвращает атрибут команды
        /// </summary>
        /// <param name="commandHandler">Команда</param>
        /// <returns>Атрибут команды</returns>
        internal static CommandAliasesAttribute GetCommandAttribute(this ICommandHandler commandHandler)
        {
            var handlerAttribute = commandHandler.GetType()
                                                 .GetCustomAttributes(inherit: false)
                                                 .ToList()
                                                 .FirstOrDefault(attribute => attribute is CommandAliasesAttribute);

            return handlerAttribute as CommandAliasesAttribute;
        }

        /// <summary>
        /// Возвращает список псевдонимов для вызова указанной команды
        /// </summary>
        /// <param name="commandHandler">Команда</param>
        public static IEnumerable<string> GetCommandAliases(this ICommandHandler commandHandler)
        {
            var commandAttribute = commandHandler.GetCommandAttribute();
            var commandText = commandAttribute?.CommandAliases;

            if (string.IsNullOrWhiteSpace(commandText))
            {
                return Enumerable.Empty<string>();
            }

            return Regex.Split
            (
                commandText,
                pattern: @"\s*,\s*"
            )
            .ToList();
        }

        /// <summary>
        /// Проверяет, является ли заданный текст одним из псевдонимов указанной команды
        /// </summary>
        /// <param name="commandHandler">Команда</param>
        /// <param name="message">Псевдоним</param>
        /// <returns>Значение "True" - является, а "False" - не является</returns>
        public static bool TextIsCommandAlias(this ICommandHandler commandHandler, string message)
        {
            return commandHandler.GetCommandAliases()
                                 .Any
                                 (
                                     pattern => Regex.IsMatch(message, $@"^$|\{pattern}")
                                 );
        }
    }
}
