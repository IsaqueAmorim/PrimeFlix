
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
                throw new EntityValidationException(ExceptionMessage.CATEGORY_NAME_NULL_OR_EMPTY);
            if(Name.Length < 3)
                throw new EntityValidationException(ExceptionMessage.CATEGORY_NAME_LESS_3_CHARS);
            if(Name.Length > 255)
                throw new EntityValidationException(ExceptionMessage.CATEGORY_NAME_MORE_255_CHARS);
            if (Description is null) 
                throw new EntityValidationException(ExceptionMessage.CATEGORY_DESCRIPTION_NULL);
            if (Description.Length > 10_000)
                throw new EntityValidationException(ExceptionMessage.CATEGORY_DESCRIPTION_MORE_10_000_CHARS);
        }
    }
}
