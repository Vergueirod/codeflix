using System.ComponentModel;
using System;
using Xunit;
using DomainEntity = Codeflix.Catalog.Domain.Entity;
using Codeflix.Catalog.Domain.Exceptions;
namespace codeflix.Catalog.UnitTests.Entity.Category
{
    public class CategoryTest
    {

    // Test 1
        [Fact(DisplayName = nameof(Instantiate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Instantiate()
        {
            // Triple A
            // Arrange
            var validData = new
            {
                Name = "category name",
                Description = "category description"
            };
            var datetimeBefore = DateTime.Now;

            // Act
            var category = new DomainEntity.Category(validData.Name, validData.Description);
            var datetimeAfter = DateTime.Now;

            // Assert
            Assert.NotNull(category);
            Assert.Equal(validData.Name, category.Name);
            Assert.Equal(validData.Description, category.Description);
            Assert.NotEqual(default(Guid), category.Id);
            Assert.NotEqual(default(DateTime), category.CreatedAt);
            Assert.True(category.CreatedAt > datetimeBefore);
            Assert.True(category.CreatedAt < datetimeAfter);
            Assert.True(category.IsActive);
        }

        // Test 2
        [Theory(DisplayName = nameof(InstantiateWithIsActiveStatus))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData(true)]
        [InlineData(false)]
        public void InstantiateWithIsActiveStatus(bool isActive)
        {
            // Triple A
            // Arrange
            var validData = new
            {
                Name = "category name",
                Description = "category description"
            };
            var datetimeBefore = DateTime.Now;

            // Act
            var category = new DomainEntity.Category(validData.Name, validData.Description, isActive);
            var datetimeAfter = DateTime.Now;

            // Assert
            Assert.NotNull(category);
            Assert.Equal(validData.Name, category.Name);
            Assert.Equal(validData.Description, category.Description);
            Assert.NotEqual(default(Guid), category.Id);
            Assert.NotEqual(default(DateTime), category.CreatedAt);
            Assert.True(category.CreatedAt > datetimeBefore);
            Assert.True(category.CreatedAt < datetimeAfter);
            Assert.Equal(isActive, category.IsActive);
        }

        [Theory(DisplayName = "")]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("    ")]

        public void InstantiateErrorWhenNameIsEmpty(string? name)
        {
            Action action = 
                () => new DomainEntity.Category(name!, "Category Description");
            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal("Name should not be empty or null", exception.Message);
        }

    // Nome deve ter no mínimo 3 caracteres
    // Nome deve ter no máximo 255 caracteres
    // Descrião deve ter no máximo 10_000 caracteres
    }
}