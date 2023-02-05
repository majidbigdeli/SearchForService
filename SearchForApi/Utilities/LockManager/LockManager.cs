using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace SearchForApi.Utilities.LockManager
{
    public class LockManager<T> : ILockManager<T>
    {
        private static readonly ConcurrentDictionary<string, LockInfo> _locks = new ConcurrentDictionary<string, LockInfo>();
        private string _object;
        private string _bucket;
        private bool _ignore;

        public LockManager(string bucket, bool ignore = false)
        {
            _bucket = bucket;
            _ignore = ignore;
        }

        public async Task AcquireLock(T input)
        {
            if (_ignore)
            {
                return;
            }

            try
            {
                _object = $"{_bucket}_{input}".ToLower().Trim();
                _locks.GetOrAdd(_object, new LockInfo(_object));
                await _locks[_object].WaitAsync();
            }
            catch (Exception e)
            {
                Serilog.Log.Fatal(e, "exception in account lock manager acquire");
                throw;
            }
        }

        public void Dispose()
        {
            if (_ignore)
            {
                return;
            }

            try
            {
                _locks[_object].Release();
            }
            catch (Exception e)
            {
                Serilog.Log.Fatal(e, "exception in account lock manager dispose");
                throw;
            }
        }
    }
}