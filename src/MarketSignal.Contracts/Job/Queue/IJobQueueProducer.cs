namespace MarketSignal.Contracts.Job.Queue;

public interface IJobQueueProducer {
    public Task EnqueueJob(Guid jobId);
}