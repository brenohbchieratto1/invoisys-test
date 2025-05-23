using App.InvoiSysTest.Domain.Entities;
using App.InvoiSysTest.Domain.Interfaces;
using App.InvoiSysTest.Infrastructure.Data.Repositories.Base;
using Mapster;
using Strategyo.Results.Contracts.Paginable;

namespace App.InvoiSysTest.Infrastructure.Data.Repositories;

public class OrderRepository : BaseRepository<Order, Entities.Order>, IOrderRepository
{
    public async Task<PaginableResult<T>> PaginableAsync<T>(int pageNumber = 1, int pageSize = 10, CancellationToken ct = default)
    {
        var database = await GetCollectionAsync(ct).ConfigureAwait(false);
        
        var safePageNumber = pageNumber <= 0 ? 1 : pageNumber;
        var safePageSize = pageSize     <= 0 ? 10 : pageSize;
        
        var totalItems = database.Count;
        
        var items = database
                   .OrderByDescending(x => x.Id)
                   .Skip((safePageNumber - 1) * safePageSize)
                   .Take(safePageSize)
                   .ToList();

        var paginable = new PaginableResult<T>()
        {
            Items = items.Adapt<List<T>>(),
            TotalItems = totalItems,
            PageNumber = pageNumber,
            PageSize = pageSize,
        };

        return paginable;
    }
}