using Antiban.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Antiban.Implementations
{
    /// <summary>
    /// Локальное хранилище истории сообщений
    /// </summary>
    internal class LocalMessageHistoryStorage : IMessageHistoryStorage
    {
        public LocalMessageHistoryStorage()
        {
            _messageHistoryStorage = new();
        }

        private readonly List<EventMessage> _messageHistoryStorage;

        #region IMessageHistoryStorage

        public EventMessage GetFirstEventMessage()
        {
            return _messageHistoryStorage.Aggregate((leftArg, rightArg) => leftArg.DateTime < rightArg.DateTime ? leftArg : rightArg);
        }

        public void AddEventMessage(EventMessage eventMessage)
        {
            _messageHistoryStorage.Add(eventMessage);
        }

        public IEnumerable<EventMessage> GetEventMessages()
        {
            return _messageHistoryStorage;
        }

        #endregion
    }
}
