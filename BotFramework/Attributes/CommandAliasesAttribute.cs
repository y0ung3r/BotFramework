using System;

namespace BotFramework.Attributes
{
    /// <summary>
    /// Атрибут, используемый для того, чтобы явно указать псевдонимы команды
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandAliasesAttribute : Attribute
    {
        /// <summary>
        /// Псевдонимы команды
        /// </summary>
        public string CommandAliases { get; }

        /// <summary>
        /// Базовый конструктор
        /// </summary>
        /// <param name="commandAliases">Псевдонимы команды через запятую</param>
        public CommandAliasesAttribute(string commandAliases)
        {
            CommandAliases = commandAliases;
        }
    }
}
