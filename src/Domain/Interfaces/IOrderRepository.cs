using App.InvoiSysTest.Domain.Entities;
using App.InvoiSysTest.Domain.Interfaces.Base;
using Strategyo.Results.Contracts.Paginable;

namespace App.InvoiSysTest.Domain.Interfaces;

public interface IOrderRepository : IBaseRepository<Order>
{
    Task<PaginableResult<T>> PaginableAsync<T>(int pageNumber = 1, int pageSize = 10, CancellationToken ct = default);
}