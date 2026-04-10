namespace MarketSignal.Contracts.Indicator.Spec;

public class InvalidIndicatorArgsException : Exception {

    public InvalidIndicatorArgsException() : base($"Indicator args are not valid.") { }

}