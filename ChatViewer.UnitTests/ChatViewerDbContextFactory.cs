using System.Data.Common;
using ChatViewer.Repository;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ChatViewer.UnitTests
{
    public sealed class ChatViewerDbContextFactory : IDisposable, IAsyncDisposable
    {
        private DbConnection _connection;

        public ChatViewerDbContext CreateContext()
        {
            if (_connection != null)
            {
                return new ChatViewerDbContext(CreateOptions());
            }

            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
            
            using var dbCreationContext = new ChatViewerDbContext(CreateOptions());
            dbCreationContext.Database.Migrate();
            return new ChatViewerDbContext(CreateOptions());
        }

        private DbContextOptions<ChatViewerDbContext> CreateOptions()
        {
            return new DbContextOptionsBuilder<ChatViewerDbContext>()
                .UseSqlite(_connection, builder =>
                {
                    builder.MigrationsAssembly(typeof(ChatViewerDbContext).Assembly.FullName);
                }).Options;
        }

        public void Dispose()
        {
            _connection?.Dispose();
            _connection = null;
        }

        public async ValueTask DisposeAsync()
        {
            if (_connection is not null)
            {
                await _connection.DisposeAsync();
            }
            _connection = null;
        }
    }
}