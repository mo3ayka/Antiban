using System.Collections.Generic;

namespace Antiban.Interfaces
{
    /// <summary>
    /// Обработчик отправляемых сообщений
    /// </summary>
    internal interface IAntibanHandler
    {
        /// <summary>
        /// Получить AntibanResult для сохраненных сообщений
        /// </summary>
        /// <returns>Список AntibanResult для сохраненных сообщений</returns>
        List<AntibanResult> GetAntibanResult();

        /// <summary>
        /// Сохранить информацию по сообщению
        /// </summary>
        /// <param name="eventMessage">Отправляемое сообщение</param>
        void SaveEventMessage(EventMessage eventMessage);
    }
}
