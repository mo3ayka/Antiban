using Antiban.Implementations;
using Antiban.Implementations.AntibanHandlers;
using Antiban.Interfaces;
using System.Collections.Generic;

namespace Antiban
{
    public class Antiban
    {
        public Antiban()
        {
            var messageHistoryStorage = new LocalMessageHistoryStorage();
            _antibanHandler = new AntibanHandlerWithSimplePeriods(messageHistoryStorage);
        }

        private readonly IAntibanHandler _antibanHandler;

        /// <summary>
        /// Добавление сообщений в систему, для обработки порядка сообщений
        /// </summary>
        /// <param name="eventMessage"></param>
        public void PushEventMessage(EventMessage eventMessage)
        {
            _antibanHandler.SaveEventMessage(eventMessage);
        }

        /// <summary>
        /// Вовзращает порядок отправок сообщений
        /// </summary>
        /// <returns></returns>
        public List<AntibanResult> GetResult()
        {
            return _antibanHandler.GetAntibanResult();
        }
    }
}
