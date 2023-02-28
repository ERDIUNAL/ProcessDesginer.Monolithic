namespace Crea.Core.Persistance.Repositories;

public interface IQuery<T>
{
    IQueryable<T> Query();
}
