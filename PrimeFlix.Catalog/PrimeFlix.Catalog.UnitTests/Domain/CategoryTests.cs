using System.IO.Compression;
using PrimeFlix.Catalog.Domain.Entity;
using PrimeFlix.Catalog.Domain.Exceptions;
using Xunit;

namespace PrimeFlix.Catalog.UnitTests.Domain
{
    public class CategoryTests
    {
        const string NAMESPACE = "Domain";
        const string CATEGORY_GROUP = "Category - Aggregates";

        [Fact(DisplayName = nameof(CategoryInstance_ShouldCreate_NewCategory))]
        [Trait(NAMESPACE, CATEGORY_GROUP)]
        public void CategoryInstance_ShouldCreate_NewCategory()
        {
            var validData = new
            {
                Name = "Ação",
                Description = "Uma Descrição"
            };

            var datetimeBefore = DateTime.Now;
            var category = new Category(validData.Name, validData.Description);
            var datetimeAfter = DateTime.Now;

            Assert.NotNull(category);
            Assert.Equal(validData.Name, category.Name);
            Assert.Equal(validData.Description, category.Description);
            Assert.NotEqual(default(Guid), category.Id);
            Assert.NotEqual(default(DateTime), category.CreatedAt);
            Assert.True(category.CreatedAt > datetimeBefore);
            Assert.True(category.CreatedAt < datetimeAfter);
            Assert.True(category.IsActive);
        }

        [Theory(DisplayName = nameof(CategoryInstanceWithParameter_ShouldCreate_NewCategory))]
        [Trait(NAMESPACE, CATEGORY_GROUP)]
        [InlineData(true)]
        [InlineData(false)]
        public void CategoryInstanceWithParameter_ShouldCreate_NewCategory(bool isActive)
        {
            var validData = new
            {
                Name = "Ação",
                Description = "Uma Descrição"
            };

            var datetimeBefore = DateTime.Now;
            var category = new Category(validData.Name, validData.Description, isActive);
            var datetimeAfter = DateTime.Now;

            Assert.NotNull(category);
            Assert.Equal(validData.Name, category.Name);
            Assert.Equal(validData.Description, category.Description);
            Assert.NotEqual(default(Guid), category.Id);
            Assert.NotEqual(default(DateTime), category.CreatedAt);
            Assert.Equal(category.IsActive, isActive);
            Assert.True(category.CreatedAt > datetimeBefore);
            Assert.True(category.CreatedAt < datetimeAfter);
        }

        [Theory(DisplayName = nameof(CategoryWithEmptyName_ShouldCreate_ThrowException))]
        [Trait(NAMESPACE, CATEGORY_GROUP)]
        [InlineData(null)]
        [InlineData("   ")]
        [InlineData("")]
        public void CategoryWithEmptyName_ShouldCreate_ThrowException(string name)
        {
            Action action = () => new Category(name, "Descrição");

            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal(ExceptionMessage.CATEGORY_NAME_NULL_OR_EMPTY, exception.Message);
        }

        [Fact(DisplayName = nameof(CategoryNameLess3Char_ShouldCreate_ThrowException))]
        [Trait(NAMESPACE, CATEGORY_GROUP)]
        public void CategoryNameLess3Char_ShouldCreate_ThrowException()
        {
            var invalidData = new { Name = "o", Description = "Valid Description" };
            Action action = () => new Category(invalidData.Name, invalidData.Description);

            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal(ExceptionMessage.CATEGORY_NAME_LESS_3_CHARS, exception.Message);
        }
        
        [Fact(DisplayName = nameof(CategoryNameMore255Char_ShouldCreate_ThrowException))]
        [Trait(NAMESPACE, CATEGORY_GROUP)]
        public void CategoryNameMore255Char_ShouldCreate_ThrowException()
        {
            var invalidData = new 
            { 
                Name = string.Join(string.Empty,Enumerable.Range(0,256).Select(i => "A")),
                Description = "Valid Description" 
            };
            
            Action action = () => new Category(invalidData.Name, invalidData.Description);

            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal(ExceptionMessage.CATEGORY_NAME_MORE_255_CHARS, exception.Message);
        }
        
        [Theory(DisplayName = nameof(CategoryDescriptionNull_ShouldCreate_ThrowEcxeption))]
        [Trait(NAMESPACE, CATEGORY_GROUP)]
        [InlineData(null)]
        public void CategoryDescriptionNull_ShouldCreate_ThrowEcxeption(string description)
        {
            Action action = () => new Category("Name", description);

            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal(ExceptionMessage.CATEGORY_DESCRIPTION_NULL, exception.Message);
        }
        
        [Fact(DisplayName = nameof(CategoryDescriptionMore10_000Char_ShouldCreate_ThrowException))]
        [Trait(NAMESPACE, CATEGORY_GROUP)]
        public void CategoryDescriptionMore10_000Char_ShouldCreate_ThrowException()
        {
            var invalidData = new
            {
                Name = "ValidName", 
                Description = string.Join(string.Empty,Enumerable.Range(0,10001).Select(i => "A"))
            };
            
            Action action = () => new Category(invalidData.Name, invalidData.Description);
        
            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal(ExceptionMessage.CATEGORY_DESCRIPTION_MORE_10_000_CHARS, exception.Message);
        }

        [Fact(DisplayName = nameof(Category_CallActivate_ChangeCategoryStatus))]
        [Trait(NAMESPACE, CATEGORY_GROUP)]
        public void Category_CallActivate_ChangeCategoryStatus()
        {
            var category = new Category("Valid Name", "Valid Description",false);

            category.Activate();
            
            Assert.True(category.IsActive);
        }
        
        [Fact(DisplayName = nameof(Category_CallDeactivate_ChangeCategoryStatus))]
        [Trait(NAMESPACE, CATEGORY_GROUP)]
        public void Category_CallDeactivate_ChangeCategoryStatus()
        {
            var category = new Category("Valid Name", "Valid Description");

            category.Deactivate();
            
            Assert.False(category.IsActive);
        }

        [Fact(DisplayName = nameof(CategoryNameAndDescription_UpdateCategory_ChangeNameAndDescription))]
        [Trait(NAMESPACE, CATEGORY_GROUP)]
        public void CategoryNameAndDescription_UpdateCategory_ChangeNameAndDescription()
        {
            var category = new Category("Name", "Description");
            var newData = new { Name = "New Name", Description = "New Description" };

            category.Update(newData.Name, newData.Description);
            
            Assert.Equal(newData.Name,category.Name);
            Assert.Equal(newData.Description,category.Description);
        }
        
        [Fact(DisplayName = nameof(CategoryName_UpdateCategory_UpdateCategoryOnlyName))]
        [Trait(NAMESPACE, CATEGORY_GROUP)]
        public void CategoryName_UpdateCategory_UpdateCategoryOnlyName()
        {
            var category = new Category("Name", "Description");
            var newName = "New Name";
            var description = category.Description;
            
            category.Update(newName);
            
            Assert.Equal(newName,category.Name);
            Assert.Equal(description,category.Description);
        }
        
        [Theory(DisplayName = nameof(EmptyName_UpdateCategoryName_ThrowException))]
        [Trait(NAMESPACE, CATEGORY_GROUP)]
        [InlineData(null)]
        [InlineData("   ")]
        [InlineData("")]
        public void EmptyName_UpdateCategoryName_ThrowException(string name)
        {
            var category = new Category("Name", "Description");
            Action action = () => category.Update(name);

            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal(ExceptionMessage.CATEGORY_NAME_NULL_OR_EMPTY, exception.Message);
        }
    }
}