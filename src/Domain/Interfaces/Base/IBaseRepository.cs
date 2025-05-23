using System.Linq.Expressions;
using App.InvoiSysTest.Domain.Entities.Base;
using Strategyo.Results.Contracts.Results;

namespace App.InvoiSysTest.Domain.Interfaces.Base;

public interface IBaseRepository<TEntity>
    where TEntity : BaseEntity
{
    Task<Result<TEntity>> AddAsync(TEntity item, CancellationToken ct = default);

    Task<Result<TEntity>> UpdateAsync(TEntity item, CancellationToken ct = default);

    Task<Result> DeleteAsync(TEntity item, CancellationToken ct = default);

    Task<Result<TEntity>> FindOneAsync(Ulid id, CancellationToken ct = default);
    Task<Result<TEntity>> FindOneAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default);

    Task<Result<IReadOnlyList<TEntity>>> FindAsync(CancellationToken ct = default);
    Task<Result<IReadOnlyList<TEntity>>> FindAsync(List<Ulid> ids, CancellationToken ct = default);
    Task<Result<IReadOnlyList<TEntity>>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default);
}