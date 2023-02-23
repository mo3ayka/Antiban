using System;
using System.Collections.Generic;
using System.Linq;

namespace Antiban.Implementations.AntibanCheckers
{
    /// <summary>
    /// Проверщик отправки сообщений с первым приоритетом
    /// </summary>
    internal class FirstPriorityAntibanChecker : AntibanCheckerBase
    {
        public FirstPriorityAntibanChecker(TimeSpan period) : base(period) { }

        #region #region Override AntibanCheckerBase

        public override IDictionary<EventMessage, AntibanResult> GetLastCorrectMessages(EventMessage eventMessage, IDictionary<EventMessage, AntibanResult> result)
        {
            return result.Where(pair => pair.Key.Phone == eventMessage.Phone && pair.Key.Priority == 1)
                         .ToDictionary(pair => pair.Key, pair => pair.Value); ;
        }

        public override bool MatchPeriodForMes(EventMessage message, IDictionary<EventMessage, AntibanResult> lastCorrectMes)
        {
            if (message.Priority != 1)
                return true;

            return base.MatchPeriodForMes(message, lastCorrectMes);
        }

        #endregion
    }
}
