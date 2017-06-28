using System.Reactive.Concurrency;

namespace RelatedRows.Helpers
{
    public interface ISchedulerProvider
    {
        IScheduler MainThread { get; }
        IScheduler Background { get; }
    }
}
