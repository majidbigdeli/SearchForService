using System.Reflection;

namespace SearchForApi.Utilities
{
    public static class MethodTimeLogger
    {
        public static void Log(MethodBase methodBase, long elapsed, string message)
        {
            Serilog.Log.Information("method: {Method}, elapsed: {ElapsedMilliseconds}, parameters: {Parameters}", $"{methodBase.DeclaringType.Name}_{methodBase.Name}", elapsed, message);
        }
    }
}

