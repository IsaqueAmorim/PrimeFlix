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
            //ARRANGE
            var validData = new
            {
                Name = "Ação",
                Description = "Uma Descrição"
            };

            //ACT
            var datetimeBefore = DateTime.Now;
            var category = new Category(validData.Name, validData.Description);
            var datetimeAfter = DateTime.Now;

            //ASSERT
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
            //ARRANGE
            var validData = new
            {
                Name = "Ação",
                Description = "Uma Descrição"
            };

            //ACT
            var datetimeBefore = DateTime.Now;
            var category = new Category(validData.Name, validData.Description, isActive);
            var datetimeAfter = DateTime.Now;

            //ASSERT
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

        [Theory(DisplayName = nameof(CategoryDescriptionNull_ShouldCreate_ThrowEcxeption))]
        [Trait(NAMESPACE, CATEGORY_GROUP)]
        [InlineData(null)]
        public void CategoryDescriptionNull_ShouldCreate_ThrowEcxeption(string description)
        {
            Action action = () => new Category("Name", description);

            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal(ExceptionMessage.CATEGORY_DESCRIPTION_NULL, exception.Message);
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
    }
}