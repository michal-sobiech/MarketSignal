using MarketSignal.Contracts.Job;
using MarketSignal.Contracts.Job.Store;

using Microsoft.AspNetCore.Mvc;

namespace MarketSignal.Api.Job;

[ApiController]
[Route("/jobs")]
public class InstrumentIndicatorController(
    IJobStore jobStore
) : ControllerBase {

    private readonly IJobStore _jobStore = jobStore;

    [HttpGet("{jobId}/status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<GetJobStatusResponse>> GetJobStatus(Guid jobId) {
        JobEntity? jobEntity = await _jobStore.Fetch(jobId);

        if (jobEntity is null) {
            return NotFound();
        }

        var jobStatus = jobEntity.JobStatus;
        GetJobStatusResponse response = new(jobStatus);

        await _jobStore.Delete(jobId);

        return Ok(response);
    }

}