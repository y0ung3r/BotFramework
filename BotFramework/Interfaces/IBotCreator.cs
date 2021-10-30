namespace BotFramework.Interfaces
{
    /// <summary>
    /// Определяет фабричный метод для создания бота
    /// </summary>
    public interface IBotCreator
    {
        /// <summary>
        /// Создать бота с указанным типом
        /// </summary>
        /// <typeparam name="TBot">Тип бота</typeparam>
        /// <param name="branch">Цепочка обязанностей</param>
        TBot Create<TBot>(RequestDelegate branch)
            where TBot : IBot;
    }
}
