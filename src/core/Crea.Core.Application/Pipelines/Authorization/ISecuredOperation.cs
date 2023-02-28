namespace Crea.Core.Application.Pipelines.Authorization;

public interface ISecuredOperation
{
    public string[] Roles { get; }
}
