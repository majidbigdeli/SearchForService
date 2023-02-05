using System;
using System.Threading.Tasks;

namespace SearchForApi.Utilities
{
    public interface ILockManager<T> : IDisposable
    {
        Task AcquireLock(T input);
    }
}