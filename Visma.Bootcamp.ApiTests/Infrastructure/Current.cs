using System;

namespace Visma.Bootcamp.ApiTests.Infrastructure
{
    public class Current
    {
        [ThreadStatic] public static Guid TestScenarioCorrelationId;
    }
}
