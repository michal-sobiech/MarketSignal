using MarketSignal.Contracts.Job;

namespace MarketSignal.Api.Job;

public record GetJobStatusResponse(
    JobStatus JobStatus
);