using Elsa.Features.Abstractions;
using Elsa.Features.Attributes;
using Elsa.Features.Services;
using Elsa.Quartz.Handlers;
using Elsa.Quartz.Jobs;
using Elsa.Quartz.Services;
using Elsa.Scheduling;
using Elsa.Scheduling.Features;
using Elsa.Workflows;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Elsa.Quartz.Features;

/// <summary>
/// A feature that installs Quartz.NET implementations for <see cref="IWorkflowScheduler"/>.
/// </summary>
[DependsOn(typeof(SchedulingFeature))]
public class QuartzSchedulerFeature(IModule module) : FeatureBase(module)
{
    /// <inheritdoc />
    public override void Configure()
    {
        Module.Configure<SchedulingFeature>(scheduling =>
        {
            // Configure the scheduling feature to use the Quartz workflow scheduler.
            scheduling.WorkflowScheduler = sp => sp.GetRequiredService<QuartzWorkflowScheduler>();

            // Configure the cron parser to use the Quartz cron parser.
            scheduling.CronParser = sp => sp.GetRequiredService<QuartzCronParser>();
        });
    }

    /// <inheritdoc />
    public override void Apply()
    {
        Services.AddSingleton<IActivityDescriptorModifier, CronActivityDescriptorModifier>();
        Services.AddSingleton<QuartzCronParser>();
        Services.AddSingleton<QuartzWorkflowScheduler>();
        Services.AddQuartz(quartz => quartz
            .AddJob<RunWorkflowJob>(job => job.WithIdentity(RunWorkflowJob.JobKey).StoreDurably())
            .AddJob<ResumeWorkflowJob>(job => job.WithIdentity(ResumeWorkflowJob.JobKey).StoreDurably()));
    }
}