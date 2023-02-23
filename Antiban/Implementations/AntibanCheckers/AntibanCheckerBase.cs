using Antiban.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Antiban.Implementations.AntibanCheckers
{
    internal abstract class AntibanCheckerBase : IAntibanChecker
    {
        public AntibanCheckerBase(TimeSpan period)
        {
            Period = period;
        }

        /// <summary>
        /// Период, который должен соблюдаться для отправки сообщения
        /// </summary>
        protected TimeSpan Period { get; init; }

        #region IAntibanChecker

        public abstract IDictionary<EventMessage, AntibanResult> GetLastCorrectMessages(EventMessage eventMessage, IDictionary<EventMessage, AntibanResult> result);

        public virtual bool MatchPeriodForMes(EventMessage message, IDictionary<EventMessage, AntibanResult> lastCorrectMes)
        {
            return lastCorrectMes.All(mes => MatchPeriodBase(message, mes, Period));
        }

        public AntibanResult GetAntibanResultForMes(EventMessage message, KeyValuePair<EventMessage, AntibanResult> lastCorrectMes)
        {
            var antibanResult = new AntibanResult()
            {
                EventMessageId = message.Id,
                SentDateTime = lastCorrectMes.Value.SentDateTime.Add(Period),
            };

            UpdateSentDateTime(message, antibanResult);

            return antibanResult;
        }

        #endregion

        /// <summary>
        /// Базовая проверка соответствия периода отправки
        /// </summary>
        /// <param name="checkedMessage">Проверяемое сообщение</param>
        /// <param name="lastOtherMes">Последнее аналогичное сообщение</param>
        /// <param name="period">Период, который должен пройти перед повторной отправкой</param>
        /// <returns></returns>
        protected bool MatchPeriodBase(EventMessage checkedMessage, KeyValuePair<EventMessage, AntibanResult> lastOtherMes, TimeSpan period)
        {
            return checkedMessage.Id == lastOtherMes.Key.Id ||
                lastOtherMes.Value.SentDateTime - checkedMessage.DateTime >= period ||
                checkedMessage.DateTime - lastOtherMes.Value.SentDateTime >= period;
        }

        /// <summary>
        /// Обновить время отправки сообщения
        /// </summary>
        /// <param name="message">Отправляемое сообщение</param>
        /// <param name="antibanResult">AntibanResult по сообщению</param>
        private void UpdateSentDateTime(EventMessage message, AntibanResult antibanResult)
        {
            message.DateTime = antibanResult.SentDateTime;
            message.ExpireDateTime = message.Priority == 0 ? message.DateTime.AddHours(1) : message.DateTime.AddDays(1);
        }
    }
}
