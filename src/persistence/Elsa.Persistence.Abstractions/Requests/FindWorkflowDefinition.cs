using Elsa.Mediator.Contracts;
using Elsa.Models;
using Elsa.Persistence.Models;

namespace Elsa.Persistence.Requests;

public record FindWorkflowByDefinitionId(string DefinitionId, VersionOptions VersionOptions) : IRequest<Workflow?>;