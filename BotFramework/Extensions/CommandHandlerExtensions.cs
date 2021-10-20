using BotFramework.Attributes;
using BotFramework.Handlers.Interfaces;
using System.Linq;
using System.Text.RegularExpressions;

namespace BotFramework.Extensions
{
    public static class CommandHandlerExtensions
    {
        public static CommandTextAttribute GetCommandAttribute(this ICommandHandler commandHandler)
        {
            return commandHandler.GetType()
                                 .GetCustomAttributes(inherit: false)
                                 .ToList()
                                 .FirstOrDefault(attribute => attribute is CommandTextAttribute) as CommandTextAttribute;
        }

        public static string GetCommandText(this ICommandHandler commandHandler)
        {
            if (commandHandler.TryGetCommandAttribute(out var attribute))
            {
                return attribute.CommandText;
            }

            return string.Empty;
        }

        public static bool TryGetCommandAttribute(this ICommandHandler commandHandler, out CommandTextAttribute attribute)
        {
            attribute = commandHandler.GetCommandAttribute();

            return attribute is not null;
        }

        public static bool IsCommandTextContains(this ICommandHandler commandHandler, string message)
        {
            if (commandHandler.TryGetCommandAttribute(out var attribute))
            {
                return Regex.Split
                (
                    input: attribute.CommandText,
                    pattern: @"\s*,\s*"
                )
                .Any
                (
                    pattern => Regex.IsMatch(message, $@"^$|\{pattern}")
                );
            }

            return false;
        }
    }
}
