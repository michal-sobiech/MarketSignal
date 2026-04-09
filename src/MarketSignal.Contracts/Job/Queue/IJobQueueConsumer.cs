namespace MarketSignal.Contracts.Job.Queue;

public interface IJobQueueConsumer {
    public Task<Guid?> DequeueJob();
}