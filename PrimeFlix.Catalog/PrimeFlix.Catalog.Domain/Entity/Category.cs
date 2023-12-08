using PrimeFlix.Catalog.Domain.Exceptions;

namespace PrimeFlix.Catalog.Domain.Entity
{
    public class Category
    {
        public Category(string name, string description, bool isActive = true)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            CreatedAt = DateTime.Now;
            IsActive = isActive;

            Validate();
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public bool IsActive { get; private set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new EntityValidationException($"{nameof(Name)} should not be empty or null");
            if (Description is null) 
                throw new EntityValidationException($"{nameof(Description)} should not be empty or null");
        }
    }
}
