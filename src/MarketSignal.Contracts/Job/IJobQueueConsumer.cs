namespace MarkeSignal.Infrastructure.Job;

public interface IJobQueueConsumer {
    public Task<Guid> DequeueJob();
}