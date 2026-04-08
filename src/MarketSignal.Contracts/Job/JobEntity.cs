using MarketSignal.Contracts.Job.Payload;

namespace MarketSignal.Contracts.Job;

public record JobEntity(
    Guid JobId,
    JobStatus JobStatus,
    JobPayload JobPayload
) {
    public static JobEntity CreateNew(Guid jobId, JobPayload payload) {
        return new JobEntity(jobId, JobStatus.PENDING, payload);
    }
}