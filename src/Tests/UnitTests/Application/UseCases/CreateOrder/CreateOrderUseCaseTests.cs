using App.InvoiSysTest.Application.UseCases.Order.CreateOrder;
using App.InvoiSysTest.Application.UseCases.Order.CreateOrder.Input;
using App.InvoiSysTest.Domain.Entities;
using App.InvoiSysTest.Domain.Interfaces;
using AutoFixture;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Mapster;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace App.InvoisysTest.UnitTest.Application.UseCases.CreateOrder;

public class CreateOrderUseCaseTests : GlobalUsings
{
    private readonly CreateOrderUseCase _useCase;

    private readonly IOrderRepository _repository = Substitute.For<IOrderRepository>();
    private readonly IValidator<CreateOrderInput> _validator = Substitute.For<IValidator<CreateOrderInput>>();
    private readonly ILogger<CreateOrderUseCase> _logger = Substitute.For<ILogger<CreateOrderUseCase>>();

    public CreateOrderUseCaseTests()
    {
        _useCase = new CreateOrderUseCase(_repository, _validator, _logger);
    }

    [Fact]
    public async Task Should_validation_not_valid()
    {
        // Arrange
        var input = Fixture.Create<CreateOrderInput>();

        var validationResult = new ValidationResult(new List<ValidationFailure>()
        {
            new("Order", "Order could not be created")
        });

        _validator
           .ValidateAsync(input, CancellationToken)
           .ReturnsForAnyArgs(validationResult);

        // Act
        var result = await _useCase.HandleAsync(input, CancellationToken);

        // Assert
        result.HasErrors.Should().BeTrue();
    }

    [Fact]
    public async Task Should_exception()
    {
        // Arrange
        var input = Fixture.Create<CreateOrderInput>();

        _validator
           .ValidateAsync(input, CancellationToken)
           .Throws(new Exception("Error"));

        // Act
        var result = await _useCase.HandleAsync(input, CancellationToken);

        // Assert
        result.HasErrors.Should().BeTrue();
    }

    [Fact]
    public async Task Should_order_created()
    {
        // Arrange
        var input = Fixture.Create<CreateOrderInput>();

        _validator
           .ValidateAsync(input, CancellationToken)
           .ReturnsForAnyArgs(new ValidationResult());

        var entity = Fixture
                    .Build<Order>()
                    .With(x => x.Id, Ulid.NewUlid())
                    .Create();

        _repository
           .AddAsync(entity, CancellationToken)
           .ReturnsForAnyArgs(entity);

        // Act
        var result = await _useCase.HandleAsync(input, CancellationToken);

        // Assert
        result.HasErrors.Should().BeFalse();
        result.Value!.CorrelationId.Should().Be(input.CorrelationId);
    }
}