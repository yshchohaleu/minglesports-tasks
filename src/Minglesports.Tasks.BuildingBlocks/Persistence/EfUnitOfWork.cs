using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Minglesports.Tasks.BuildingBlocks.Domain;
using Minglesports.Tasks.BuildingBlocks.Exceptions;
using Minglesports.Tasks.BuildingBlocks.Messages;

namespace Minglesports.Tasks.BuildingBlocks.Persistence
{
    /// <summary>
    /// Entity Framework unit of work implementation.
    /// This class exposes one public method `CommitAsync()`. During it's execution entities are stored in database
    /// plus all events stored in aggregate roots are dispatched.
    /// </summary>
    public abstract class EfUnitOfWork<TEntity, TDbContext> : IUnitOfWork
        where TEntity : class, IAggregateRoot
        where TDbContext: DbContext
    {
        private readonly ISendMessages _messageSender;
        protected readonly TDbContext DbContext;

        protected EfUnitOfWork(TDbContext dbContext, ISendMessages messageSender)
        {
            _messageSender = messageSender;
            DbContext = dbContext;
        }

        public async Task CommitAsync()
        {
            var entries = DbContext.ChangeTracker.Entries<TEntity>();
            var sendTasks = new List<Task>();

            foreach (var entry in entries)
            {
                sendTasks.Add(_messageSender.PublishEvents(entry.Entity.GetUncommittedEvents()));
            }

            try
            {
                await DbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new ConcurrencyException(
                    $"Concurrency exception while saving [{typeof(TEntity).FullName}]", e);
            }

            await Task.WhenAll(sendTasks);
        }
    }
}