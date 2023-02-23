using System;
using System.Collections.Generic;
using System.Linq;

namespace Antiban.Implementations.AntibanCheckers
{
    /// <summary>
    /// Проверщик отправки сообщений на один номер
    /// </summary>
    internal class OneNumberAntibanChecker : AntibanCheckerBase
    {
        public OneNumberAntibanChecker(TimeSpan period) : base(period) { }

        #region Override AntibanCheckerBase

        public override IDictionary<EventMessage, AntibanResult> GetLastCorrectMessages(EventMessage eventMessage, IDictionary<EventMessage, AntibanResult> result)
        {
            return result.Where(pair => pair.Key.Phone == eventMessage.Phone && pair.Key.DateTime <= eventMessage.DateTime)
                         .ToDictionary(pair => pair.Key, pair => pair.Value); ;
        }

        #endregion
    }
}
