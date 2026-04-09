namespace MarketSignal.Contracts.Job.Store;

public interface IJobStore {
    public Task Save(JobEntity job);
    public Task<JobEntity?> Fetch(Guid jobId);
    public Task Delete(Guid jobId);
}