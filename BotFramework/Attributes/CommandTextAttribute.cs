using System;

namespace BotFramework.Attributes
{
    /// <summary>
    /// Атрибут, используемый для того, чтобы явно указать текст команды
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandTextAttribute : Attribute
    {
        /// <summary>
        /// Текст команды
        /// </summary>
        public string CommandText { get; }

        /// <summary>
        /// Базовый конструктор
        /// </summary>
        /// <param name="commandText">Текст команды</param>
        public CommandTextAttribute(string commandText)
        {
            CommandText = commandText;
        }
    }
}
