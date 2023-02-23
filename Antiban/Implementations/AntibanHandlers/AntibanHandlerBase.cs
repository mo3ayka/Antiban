using Antiban.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Antiban.Implementations.AntibanHandlers
{
    /// <summary>
    /// Базовый обработчик отправляемых сообщений
    /// </summary>
    /// <remarks>Проверяет корректность отправляемых сообщений по списку чекеров <see cref="Checkers"/></remarks>
    internal abstract class AntibanHandlerBase : IAntibanHandler
    {
        public AntibanHandlerBase(IMessageHistoryStorage messageHistoryStorage)
        {
            _messageHistoryStorage = messageHistoryStorage;
        }

        private readonly IMessageHistoryStorage _messageHistoryStorage;

        #region Properties

        /// <summary>
        /// Чекеры, использующиеся для проверки корректности отправки сообщений
        /// </summary>
        protected abstract IEnumerable<IAntibanChecker> Checkers { get; }

        #endregion

        #region IAntibanHandler

        public void SaveEventMessage(EventMessage eventMessage)
        {
            _messageHistoryStorage.AddEventMessage(eventMessage);
        }

        public List<AntibanResult> GetAntibanResult()
        {
            var messages = _messageHistoryStorage.GetEventMessages();
            var result = new Dictionary<EventMessage, AntibanResult>();

            var firstAntibanResult = GetFirstAntibanResult();
            result.TryAdd(firstAntibanResult.Key, firstAntibanResult.Value);

            var messagesWithoutResult = GetMessagesWithoutResult(messages, result);

            while (messagesWithoutResult.Any())
            {
                FillAntibanResultForMessage(messagesWithoutResult.First(), result);

                messagesWithoutResult = GetMessagesWithoutResult(messages, result);
            }

            return result.Values.OrderBy(mes => mes.SentDateTime)
                                .ToList();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Заполнить AntibanResult для сообщения
        /// </summary>
        /// <param name="eventMessage">Отправляемое сообщение</param>
        /// <param name="result">Уже заполненные сообщения</param>
        private void FillAntibanResultForMessage(EventMessage eventMessage, IDictionary<EventMessage, AntibanResult> result)
        {
            if (Checkers.Any(checker => HasUpdate(eventMessage, result, checker)))
                return;

            result.TryAdd(eventMessage, new AntibanResult()
            {
                EventMessageId = eventMessage.Id,
                SentDateTime = eventMessage.DateTime
            });
        }

        /// <summary>
        /// Сообщение было изменено чекером
        /// </summary>
        /// <param name="eventMessage"></param>
        /// <param name="result"></param>
        /// <param name="checker"></param>
        /// <returns></returns>
        private bool HasUpdate(EventMessage eventMessage, IDictionary<EventMessage, AntibanResult> result, IAntibanChecker checker)
        {
            var hasUpdate = false;

            var lastMessages = checker.GetLastCorrectMessages(eventMessage, result);

            while (!checker.MatchPeriodForMes(eventMessage, lastMessages))
            {
                var newAntibanResult = checker.GetAntibanResultForMes(eventMessage, lastMessages.LastOrDefault());
                result.AddOrUpdate(eventMessage, newAntibanResult);

                hasUpdate = true;

                lastMessages = checker.GetLastCorrectMessages(eventMessage, result);
            }

            return hasUpdate;
        }

        /// <summary>
        /// Получить сообщения по которым еще не выставлен итоговый AntibanResult
        /// </summary>
        /// <param name="messages"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private IEnumerable<EventMessage> GetMessagesWithoutResult(IEnumerable<EventMessage> messages, IDictionary<EventMessage, AntibanResult> result)
        {
            return messages.Where(mes => !result.Any(antiban => antiban.Value.EventMessageId == mes.Id));
        }

        /// <summary>
        /// Получить AntibanResult для первого отправляемого сообщения
        /// </summary>
        /// <returns></returns>
        private KeyValuePair<EventMessage, AntibanResult> GetFirstAntibanResult()
        {
            var firstMessage = _messageHistoryStorage.GetFirstEventMessage();
            var antibanResult = new AntibanResult()
            {
                EventMessageId = firstMessage.Id,
                SentDateTime = firstMessage.DateTime
            };

            return new KeyValuePair<EventMessage, AntibanResult>(firstMessage, antibanResult);
        }

        #endregion
    }
}
