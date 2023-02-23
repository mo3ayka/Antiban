using System.Collections.Generic;

namespace Antiban.Interfaces
{
    /// <summary>
    /// Проверщик сообщений на соответствие периодам отправки
    /// </summary>
    internal interface IAntibanChecker
    {
        /// <summary>
        /// Получить последние отправленные сообщения, которые могут повлиять на отправку текущего сообщения
        /// </summary>
        /// <param name="eventMessage">Отправляемое сообщение</param>
        /// <param name="result">Последние успешно отправленные сообщения</param>
        /// <returns></returns>
        IDictionary<EventMessage, AntibanResult> GetLastCorrectMessages(EventMessage eventMessage, IDictionary<EventMessage, AntibanResult> result);

        /// <summary>
        /// Веремя отправки сообщения соответвует периоду AntibanChecker'a исходя из информации по последним отправкам
        /// </summary>
        /// <param name="message">Отправляемое сообщение</param>
        /// <param name="lastCorrectMes">Последние успешно отправленные сообщения</param>
        /// <returns></returns>
        bool MatchPeriodForMes(EventMessage message, IDictionary<EventMessage, AntibanResult> lastCorrectMes);

        /// <summary>
        /// Получить AntibanResult с новым временем отправки
        /// </summary>
        /// <param name="message">Отправляемое сообщение</param>
        /// <param name="lastCorrectMes">Последнее успешно отправленное сообщение</param>
        /// <returns></returns>
        AntibanResult GetAntibanResultForMes(EventMessage message, KeyValuePair<EventMessage, AntibanResult> lastCorrectMes);
    }
}
