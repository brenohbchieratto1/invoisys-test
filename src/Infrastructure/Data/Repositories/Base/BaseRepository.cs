using System.Linq.Expressions;
using App.InvoiSysTest.Domain.Entities.Base;
using App.InvoiSysTest.Domain.Interfaces.Base;
using App.InvoiSysTest.Infrastructure.Data.Abstractions;
using App.InvoiSysTest.Infrastructure.Entities.Base;
using Mapster;
using Strategyo.Extensions.Conversions;
using Strategyo.Results.Contracts.Results;

namespace App.InvoiSysTest.Infrastructure.Data.Repositories.Base;

public abstract class BaseRepository<TEntity, TCollection> : SetCollection<TCollection>, IBaseRepository<TEntity>
    where TEntity : BaseEntity
    where TCollection : BaseCollection
{
    public async Task<Result<TEntity>> AddAsync(TEntity item, CancellationToken ct = default)
    {
        var database = await GetCollectionAsync(ct).ConfigureAwait(false);

        var adapt = item.Adapt<TCollection>();

        database.Add(adapt);

        await SaveChangesAsync(database, ct).ConfigureAwait(false);

        return item;
    }

    public async Task<Result<TEntity>> UpdateAsync(TEntity item, CancellationToken ct = default)
    {
        var database = await GetCollectionAsync(ct).ConfigureAwait(false);

        var itemUpdate = database.FirstOrDefault(x => x.Id == item.Id);

        if (itemUpdate == null)
            return Errors.NotFound("Não foi encontrado o item para ser atualizado");

        var adapt = itemUpdate.Adapt<TCollection>();

        database.Remove(itemUpdate);
        database.Add(adapt);

        await SaveChangesAsync(database, ct).ConfigureAwait(false);

        return item;
    }

    public async Task<Result> DeleteAsync(TEntity item, CancellationToken ct = default)
    {
        var database = await GetCollectionAsync(ct).ConfigureAwait(false);

        var itemRemove = database.FirstOrDefault(x => x.Id == item.Id);

        if (itemRemove == null)
            return Errors.NotFound("Não foi encontrado o item para ser removido");

        database.Remove(itemRemove);

        await SaveChangesAsync(database, ct).ConfigureAwait(false);

        return true;
    }

    public async Task<Result<TEntity>> FindOneAsync(Ulid id, CancellationToken ct = default)
    {
        var database = await GetCollectionAsync(ct).ConfigureAwait(false);

        var item = database.FirstOrDefault(x => x.Id == id);

        if (item == null)
            return Errors.NotFound("Não foi encontrado o item");

        var itemAdapt = item.Adapt<TEntity>();

        return itemAdapt;
    }

    public async Task<Result<TEntity>> FindOneAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
    {
        var database = await GetCollectionAsync(ct).ConfigureAwait(false);

        var adaptPredicate = predicate.Adapt<TEntity, TCollection>();
        var func = adaptPredicate.Compile();

        var item = database.FirstOrDefault(func);

        if (item == null)
            return Errors.NotFound("Não foi encontrado o item");

        var itemAdapt = item.Adapt<TEntity>();

        return itemAdapt;
    }

    public async Task<Result<IReadOnlyList<TEntity>>> FindAsync(CancellationToken ct = default)
    {
        var database = await GetCollectionAsync(ct).ConfigureAwait(false);

        var itemAdapt = database.Adapt<List<TEntity>>();

        return itemAdapt;
    }

    public async Task<Result<IReadOnlyList<TEntity>>> FindAsync(List<Ulid> ids, CancellationToken ct = default)
    {
        var database = await GetCollectionAsync(ct).ConfigureAwait(false);

        var item = database.FindAll(x => ids.Contains(x.Id));

        var itemAdapt = item.Adapt<List<TEntity>>();

        return itemAdapt;
    }

    public async Task<Result<IReadOnlyList<TEntity>>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
    {
        var database = await GetCollectionAsync(ct).ConfigureAwait(false);

        var adaptPredicate = predicate.Adapt<TEntity, TCollection>();
        var func = adaptPredicate.Compile();

        var findAllPredicate = new Predicate<TCollection>(func);

        var item = database.FindAll(findAllPredicate);

        var itemAdapt = item.Adapt<List<TEntity>>();

        return itemAdapt;
    }
}