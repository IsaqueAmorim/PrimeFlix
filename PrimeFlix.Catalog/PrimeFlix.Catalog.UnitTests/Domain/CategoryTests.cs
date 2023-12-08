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
            var category = new Category(validData.Name, validData.Description,isActive);
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

        [Theory(DisplayName = nameof(CategoryIntaceWithEmptyName_ShouldCreate_ThrowExeption))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData(null)]
        [InlineData("   ")]
        [InlineData("")]
        public void CategoryIntaceWithEmptyName_ShouldCreate_ThrowExeption(string name)
        {
            Action action = () => new Category(name, "Descrição");

            var exception = Assert.Throws<EntityValidationException>(action);
        }
    }
}
