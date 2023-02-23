using System;
using System.Collections.Generic;
using System.Linq;

namespace Antiban.Implementations.AntibanCheckers
{
    /// <summary>
    /// Проверщик отправки сообщений на разные номера
    /// </summary>
    internal class DifNumberAntibanChecker : AntibanCheckerBase
    {
        public DifNumberAntibanChecker(TimeSpan period) : base(period) { }

        #region Override AntibanCheckerBase

        public override IDictionary<EventMessage, AntibanResult> GetLastCorrectMessages(EventMessage eventMessage, IDictionary<EventMessage, AntibanResult> result)
        {
            return result.Where(pair => pair.Key.DateTime <= eventMessage.DateTime)
                         .ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        #endregion
    }
}
