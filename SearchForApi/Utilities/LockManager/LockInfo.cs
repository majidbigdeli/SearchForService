using System.Threading;
using System.Threading.Tasks;

namespace SearchForApi.Utilities.LockManager
{
    public class LockInfo
    {
        public SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        public LockInfo(string value)
        {
            Value = value;
        }

        public Task WaitAsync()
        {
            return semaphoreSlim.WaitAsync();
        }

        public void Release()
        {
            semaphoreSlim.Release();
        }

        public string Value { get; private set; }
    }
}