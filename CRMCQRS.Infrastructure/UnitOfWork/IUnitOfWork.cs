using CRMCQRS.Infrastructure.Pages;
using CRMCQRS.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace CRMCQRS.Infrastructure.UnitOfWork;

public interface IUnitOfWork<out TContext> : IUnitOfWork where TContext : DbContext
{
    TContext DbContext { get; }

    Task<int> SaveChangesAsync(params IUnitOfWork[] unitOfWorks);

}

public interface IUnitOfWork
{
    IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false) where TEntity : class;

    int SaveChanges();

    Task<int> SaveChangesAsync();

    int ExecuteSqlCommand(string sql, params object[] parameters);

    Task<int> ExecuteSqlCommandAsync(string sql, params object[] parameters);

    IQueryable<TEntity> FromSqlRaw<TEntity>(string sql, params object[] parameters) where TEntity : class;

    void TrackGraph(object rootEntity, Action<EntityEntryGraphNode> callback);

    Task<IDbContextTransaction> BeginTransactionAsync(bool useIfExists = false);

    IDbContextTransaction BeginTransaction(bool useIfExists = false);

    void SetAutoDetectChanges(bool value);

    SaveChangesResult LastSaveChangesResult { get; }
}
