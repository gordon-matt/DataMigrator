using Microsoft.VisualStudio.Threading;

namespace DataMigrator.Common
{
    public static class AsyncHelper
    {
        //private static readonly TaskFactory taskFactory = new(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);

        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
        {
            using var joinableTaskContext = new JoinableTaskContext();
            var joinableTaskFactory = new JoinableTaskFactory(joinableTaskContext);
            return joinableTaskFactory.Run(func);

            //return taskFactory.StartNew(func).Unwrap().GetAwaiter().GetResult();
        }

        public static void RunSync(Func<Task> func)
        {
            using var joinableTaskContext = new JoinableTaskContext();
            var joinableTaskFactory = new JoinableTaskFactory(joinableTaskContext);
            joinableTaskFactory.Run(func);
            //taskFactory.StartNew(func).Unwrap().GetAwaiter().GetResult();
        }
    }
}