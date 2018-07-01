using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Oboete.Database;
using System;
using System.Data.Common;

namespace Oboete.Test.Database
{
    public class DbContextFactory : IDisposable
    {
        private DbConnection Connection { get; set; }

        private DbContextOptions<OboeteContext> CreateOptions()
        {
            return new DbContextOptionsBuilder<OboeteContext>()
                            .UseSqlite(Connection).Options;
        }

        public OboeteContext CreateContext()
        {
            if (Connection == null) {
                Connection = new SqliteConnection("DataSource=:memory:");
                Connection.Open();

                var options = CreateOptions();
                using (var context = new OboeteContext(options)) {
                    context.Database.EnsureCreated();
                }
            }

            return new OboeteContext(CreateOptions());
        }

        public void Dispose()
        {
            if (Connection != null) {
                Connection.Dispose();
                Connection = null;
            }
        }
    }
}