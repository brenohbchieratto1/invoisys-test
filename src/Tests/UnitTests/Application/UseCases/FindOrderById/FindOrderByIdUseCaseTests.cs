using App.InvoiSysTest.Application.UseCases.Order.FindOrderById;
using App.InvoiSysTest.Application.UseCases.Order.FindOrderById.Input;
using App.InvoiSysTest.Domain.Entities;
using App.InvoiSysTest.Domain.Interfaces;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace App.InvoisysTest.UnitTest.Application.UseCases.FindOrderById;

public class FindOrderByIdUseCaseTests : GlobalUsings
{
    private readonly FindOrderByIdUseCase _useCase;

    private readonly IOrderRepository _repository = Substitute.For<IOrderRepository>();
    private readonly ILogger<FindOrderByIdUseCase> _logger = Substitute.For<ILogger<FindOrderByIdUseCase>>();

    public FindOrderByIdUseCaseTests()
    {
        _useCase = new FindOrderByIdUseCase(_repository, _logger);
    }

    [Fact]
    public async Task Should_exception()
    {
        // Arrange
        var id = Ulid.NewUlid().ToString();

        var input = new FindOrderByIdInput(id);

        _repository
           .FindOneAsync(input.Id, CancellationToken)
           .Throws(new Exception("Error"));

        // Act
        var result = await _useCase.HandleAsync(input, CancellationToken);

        // Assert
        result.HasErrors.Should().BeTrue();
    }

    [Fact]
    public async Task Should_success_return_order()
    {
        // Arrange
        var id = Ulid.NewUlid();

        var input = new FindOrderByIdInput(id.ToString());

        var entity = Fixture
                    .Build<Order>()
                    .With(x => x.Id, id)
                    .Create();

        _repository
           .FindOneAsync(input.Id, CancellationToken)
           .ReturnsForAnyArgs(entity);

        // Act
        var result = await _useCase.HandleAsync(input, CancellationToken);

        // Assert
        result.HasErrors.Should().BeFalse();
        result.Value!.Id.Should().BeEquivalentTo(entity.Id);
    }
}