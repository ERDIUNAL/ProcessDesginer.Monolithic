namespace Crea.Core.Persistance.Repositories;

public class Entity
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }

    public Entity()
    {

    }

    public Entity(int id)
    {
        Id = id;
    }
}
