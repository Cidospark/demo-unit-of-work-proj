using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using UnitOfWork.Core.IConfiguration;
using UnitOfWork.Core.IRepository;
using UnitOfWork.Core.Repository;
using UnitOfWork.Data;

namespace UnitOfWork.Data
{
    public class UnitOfworkClass : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _ctx;
        private readonly ILogger _logger;
        public IUserRepository Users { get; private set; }

        public UnitOfworkClass(ApplicationDbContext context, ILoggerFactory logger)
        {
            _ctx = context;
            _logger = logger.CreateLogger("logs");
            Users = new UserRepository(_ctx, _logger);
        }

        public async Task CompleteAsync()
        {
            await _ctx.SaveChangesAsync();
        }

        public void Dispose()
        {
            _ctx.Dispose();
        }
    }
}
