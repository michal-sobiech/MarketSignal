namespace MarkeSignal.Infrastructure.Job;

public interface IJobQueueProducer {
    public Task EnqueueJob(Guid jobId);
}