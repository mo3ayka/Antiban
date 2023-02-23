using Antiban.Implementations.AntibanCheckers;
using Antiban.Interfaces;
using System;
using System.Collections.Generic;

namespace Antiban.Implementations.AntibanHandlers
{
    internal class AntibanHandlerWithSimplePeriods : AntibanHandlerBase
    {
        public AntibanHandlerWithSimplePeriods(IMessageHistoryStorage messageHistoryStorage)
            : base(messageHistoryStorage) { }

        protected override IEnumerable<IAntibanChecker> Checkers => new List<IAntibanChecker>()
        {
            new FirstPriorityAntibanChecker(TimeSpan.FromDays(1)),
            new OneNumberAntibanChecker(TimeSpan.FromMinutes(1)),
            new DifNumberAntibanChecker(TimeSpan.FromSeconds(10))
        };
    }
}
