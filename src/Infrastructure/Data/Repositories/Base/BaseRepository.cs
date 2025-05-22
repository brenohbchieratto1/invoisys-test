using System.Linq.Expressions;
using App.InvoisysTest.Domain.Entities.Base;
using App.InvoisysTest.Domain.Interfaces.Base;
using App.InvoisysTest.Infrastructure.Data.Abstractions;
using App.InvoisysTest.Infrastructure.Entities.Base;
using Strategyo.Results.Contracts.Results;

namespace App.InvoisysTest.Infrastructure.Data.Repositories.Base;

public abstract class BaseRepository<TEntity, TCollection> : SetCollection<TCollection>, IBaseRepository<TEntity>
    where TEntity : BaseEntity
    where TCollection : BaseCollection
{
    
    public async Task<Result<TEntity>> AddAsync(TEntity item, CancellationToken ct = default)
    {
        var database = await GetCollectionAsync(ct);
        
        database.
    }

    public Task<Result<TEntity>> UpdateAsync(TEntity item, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteAsync(TEntity item, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<TEntity>> FindOneAsync(Ulid id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<TEntity>> FindOneAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IReadOnlyList<TEntity>>> FindAsync(CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IReadOnlyList<TEntity>>> FindAsync(List<Ulid> ids, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IReadOnlyList<TEntity>>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}