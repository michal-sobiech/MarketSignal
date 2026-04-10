namespace MarketSignal.Contracts.Job;

public enum JobStatus {
    SUCCESS,
    ERROR,
    PENDING
}

public static class JobStatusExtensions {
    public static bool IsFinal(this JobStatus status) {
        return status is JobStatus.SUCCESS or JobStatus.ERROR;
    }
}
