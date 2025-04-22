using Microsoft.Extensions.DependencyInjection;
using ProjectSpace.Services.Validation;
using System.ComponentModel.DataAnnotations;

namespace ProjectSpaceTests;

[TestClass]
public sealed class ValidationServiceTests
{
    [TestMethod]
    public void ValidationPassesWithValidModel()
    {
        // Arrange
        ServiceCollection services = new();
        using var serviceProvider = services.BuildServiceProvider();
        ValidationService validationService = new(serviceProvider);
        Model model = new() { Name = "Sam" };

        // Act
        var result = validationService.ValidateObject(model);

        // Assert
        Assert.IsTrue(result.Success);
        Assert.IsNull(result.Errors);
    }

    [TestMethod]
    public void ValidationFailsWhenModelIsInvalid()
    {
        // Arrange
        ServiceCollection services = new();
        using var serviceProvider = services.BuildServiceProvider();
        ValidationService validationService = new(serviceProvider);
        Model model = new() { Name = null! };

        // Act
        var result = validationService.ValidateObject(model);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.IsNotNull(result.Errors);
        Assert.IsTrue(
            result.Errors.ContainsKey(nameof(Model.Name)),
            $"Errors does not contain {nameof(Model.Name)} property errors");
    }

    [TestMethod]
    public void ValidateObject_ShouldThrowArgumentNullExceptionWhenValueIsNull()
    {
        // Arrange
        ServiceCollection services = new();
        using var serviceProvider = services.BuildServiceProvider();
        ValidationService validationService = new(serviceProvider);

        // Assert
        Assert.ThrowsException<ArgumentNullException>(() => validationService.ValidateObject(null!));
    }

    [TestMethod]
    public void ValidationShouldIncludeAllPropertiesAndNotPass()
    {
        // Arrange
        ServiceCollection services = new();
        using var serviceProvider = services.BuildServiceProvider();
        ValidationService validationService = new(serviceProvider);
        Model model = new() { Name = "Sam", Email = "email.com" };

        var result = validationService.ValidateObject(model);

        // Assert
        Assert.IsFalse(result.Success, "Validation should fail as email is invalid");
        Assert.IsTrue(result.Errors.ContainsKey(nameof(model.Email)), "Email key should be included in errors");
    }

    private sealed class Model
    {
        [Required, MaxLength(5)]
        public required string Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
    }
}