using System.Text.Json.Serialization;

namespace MarketSignal.Contracts.Job.Payload;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "jobKind")]
[JsonDerivedType(typeof(UpdateInstrumentRawDataJobPayload), "UPDATE_INSTRUMENT_RAW_DATA")]
[JsonDerivedType(typeof(CalcIndicatorJobPayload), "CALC_INDICATOR")]
public abstract record JobPayload;