using System.Text.Json.Serialization;
using Elsa.Workflows.Core.Serialization.Converters;

namespace Elsa.MassTransit.Messages;

public record DispatchResumeWorkflows(
    string ActivityTypeName,
    [property: JsonConverter(typeof(PolymorphicConverter))]
    object BookmarkPayload,
    string? CorrelationId,
    string? WorkflowInstanceId,
    IDictionary<string, object>? Input
);