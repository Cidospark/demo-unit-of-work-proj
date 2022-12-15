using System;
using System.Threading.Tasks;
using UnitOfWork.Core.IRepository;

namespace UnitOfWork.Core.IConfiguration
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }

        Task CompleteAsync();
    }
}
