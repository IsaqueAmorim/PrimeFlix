namespace PrimeFlix.Catalog.Domain.SeedWorks;

public abstract class Entity
{
    public Entity() => Guid.NewGuid();
    
    public Guid Id { get; protected set; }
}