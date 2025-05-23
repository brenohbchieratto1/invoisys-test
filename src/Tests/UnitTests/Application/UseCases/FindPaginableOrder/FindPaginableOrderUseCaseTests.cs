using App.InvoiSysTest.Application.UseCases.Order.FindPaginableOrder;
using App.InvoiSysTest.Application.UseCases.Order.FindPaginableOrder.Input;
using App.InvoiSysTest.Application.UseCases.Order.FindPaginableOrder.Output;
using App.InvoiSysTest.Domain.Interfaces;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Strategyo.Results.Contracts.Paginable;

namespace App.InvoisysTest.UnitTest.Application.UseCases.FindPaginableOrder;

public class FindPaginableOrderUseCaseTests : GlobalUsings
{
    private readonly FindPaginableOrderUseCase _useCase;

    private readonly IOrderRepository _repository = Substitute.For<IOrderRepository>();
    private readonly ILogger<FindPaginableOrderUseCase> _logger = Substitute.For<ILogger<FindPaginableOrderUseCase>>();

    public FindPaginableOrderUseCaseTests()
    {
        _useCase = new FindPaginableOrderUseCase(_repository, _logger);
    }

    [Fact]
    public async Task Should_exception()
    {
        // Arrange
        var input = Fixture.Create<FindPaginableOrderInput>();

        _repository
           .PaginableAsync<PaginableOutput>(input.PageNumber, input.PageSize, CancellationToken)
           .Throws(new Exception("Error"));

        // Act
        var result = await _useCase.HandleAsync(input, CancellationToken);

        // Assert
        result.HasErrors.Should().BeTrue();
    }

    [Fact]
    public async Task Should_success_return_paginable()
    {
        // Arrange
        var input = Fixture.Create<FindPaginableOrderInput>();

        var items = Fixture
                   .Build<PaginableOutput>()
                   .With(x => x.Id, Ulid.NewUlid)
                   .CreateMany(10)
                   .ToList();

        var paginable = new PaginableResult<PaginableOutput>()
        {
            Items = items,
        };

        _repository
           .PaginableAsync<PaginableOutput>(input.PageNumber, input.PageSize, CancellationToken)
           .ReturnsForAnyArgs(paginable);

        // Act
        var result = await _useCase.HandleAsync(input, CancellationToken);

        // Assert
        result.HasErrors.Should().BeFalse();
        result.Value!.Items.Should().NotBeNullOrEmpty();
    }
}