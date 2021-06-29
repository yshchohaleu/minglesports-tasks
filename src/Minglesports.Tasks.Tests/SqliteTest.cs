using System;
using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Minglesports.Tasks.Providers.Entities;

namespace Minglesports.Tasks.Tests
{
    public abstract class SqliteTest : IDisposable
    {
        private readonly SqliteConnection _connection;

        protected readonly IFixture Fixture = new Fixture()
            .Customize(new AutoMoqCustomization())
            .Customize(new TodoListDomainCustomization());

        internal readonly TodoListDbContext DbContext;

        protected SqliteTest()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            DbContextOptions<TodoListDbContext> options = new DbContextOptionsBuilder<TodoListDbContext>()
                .UseSqlite(_connection)
                .Options;

            DbContext = new TodoListDbContext(options);

            DbContext.Database.EnsureDeleted();
            DbContext.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _connection?.Dispose();
            DbContext?.Dispose();
        }
    }
}