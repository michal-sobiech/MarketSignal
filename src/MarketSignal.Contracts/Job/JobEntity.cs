using MarkeSignal.Infrastructure.Job;

namespace MarketSignal.Infrastructure.Job;

public record JobEntity(
    Guid JobId,
    JobStatus Status
);