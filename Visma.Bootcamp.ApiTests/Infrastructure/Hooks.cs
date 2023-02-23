using System;
using TechTalk.SpecFlow;

namespace Visma.Bootcamp.ApiTests.Infrastructure
{    
    [Binding]
    public class Hooks
    {
        [BeforeScenario(Order = 0)]
        public static void InitUniqueTestRunId()
        {
            Current.TestScenarioCorrelationId = Guid.NewGuid();
        }
    }
}
