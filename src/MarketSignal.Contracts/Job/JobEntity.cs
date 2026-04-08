namespace MarketSignal.Contracts.Job;

public record JobEntity(
    Guid JobId,
    JobKind Kind,
    JobStatus Status
) {
    public static JobEntity createNew(Guid jobId, JobKind kind) {
        return new JobEntity(jobId, kind, JobStatus.PENDING);
    }
}