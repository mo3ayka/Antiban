using System.Collections.Generic;

namespace Antiban.Interfaces
{
    /// <summary>
    /// Хранилище истории отправляемых сообщений
    /// </summary>
    internal interface IMessageHistoryStorage
    {
        /// <summary>
        /// Получить первое отправляемое сообщение
        /// </summary>
        /// <remarks>Сообщение с наименьшим <see cref="EventMessage.DateTime"/></remarks>
        /// <returns></returns>
        EventMessage GetFirstEventMessage();

        /// <summary>
        /// Получить все хранимые сообщения
        /// </summary>
        /// <returns></returns>
        IEnumerable<EventMessage> GetEventMessages();

        /// <summary>
        /// Добавить сообщение в хранилище
        /// </summary>
        /// <param name="eventMessage">Информация о сообщении</param>
        void AddEventMessage(EventMessage eventMessage);
    }
}
