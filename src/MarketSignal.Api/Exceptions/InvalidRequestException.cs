namespace MarketSignal.Api.Exceptions;

public class InvalidRequestException(string message) : Exception(message);