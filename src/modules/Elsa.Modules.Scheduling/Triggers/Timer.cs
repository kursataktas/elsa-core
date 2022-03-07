﻿using System;
using System.Collections.Generic;
using Elsa.Attributes;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Modules.Scheduling.Triggers;

/// <summary>
/// Represents a timer to periodically trigger the workflow.
/// </summary>
public class Timer : EventGenerator
{
    [Input] public Input<TimeSpan> Interval { get; set; } = default!;

    protected override IEnumerable<object> GetTriggerData(TriggerIndexingContext context)
    {
        var interval = context.ExpressionExecutionContext.Get(Interval);
        var clock = context.ExpressionExecutionContext.GetRequiredService<ISystemClock>();
        var executeAt = clock.UtcNow.Add(interval);
        yield return new TimerPayload(executeAt, interval);
    }
}

public record TimerPayload(DateTimeOffset StartAt, TimeSpan Interval);