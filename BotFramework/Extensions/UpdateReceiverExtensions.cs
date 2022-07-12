using System.Linq;
using System.Reflection;
using BotFramework.Interfaces;

namespace BotFramework.Extensions;

/// <summary>
/// Методы-расширения для <see cref="IUpdateReceiver"/>
/// </summary>
internal static class UpdateReceiverExtensions
{
    /// <summary>
    /// Вызывает обобщенный метод <see cref="IUpdateReceiver.Receive"/>
    /// </summary>
    /// <param name="receiver">Точка получения обновлений от внешней системы</param>
    /// <param name="update">Обновление</param>
    internal static void InvokeReceive(this IUpdateReceiver receiver, object update)
    {
        var receiverType = receiver.GetType();
        var receiveMethodName = nameof(IUpdateReceiver.Receive);
        
        var methods = receiverType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
        var targetMethod = methods.First(method => method.IsGenericMethod && method.Name == receiveMethodName);
        
        var updateType = update.GetType();
        targetMethod.MakeGenericMethod(updateType)
                    .Invoke(receiver, new[] { update });
    }
}