using DDDZamin.Core.Contracts.Data.Commands;

namespace DDDZamin.Infrastructure.Data.Sql.Commands;

public class BaseEntityFrameworkUnitOfWork<TDbContext>:IUnitOfWork where TDbContext:BaseCommandDbContext
{
    protected readonly TDbContext _dbContext;

    public BaseEntityFrameworkUnitOfWork(TDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void BeginTransaction()
    {
        _dbContext.BeginTransaction();
    }

    public int Commit()
    {
        return _dbContext.SaveChanges();
    }

    public async Task<int> CommitAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }

    public void CommitTransaction()
    {
        _dbContext.CommitTransaction();
    }

    public void RollbackTransaction()
    {
        _dbContext.RollbackTransaction();
    }
}