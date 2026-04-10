namespace MarketSignal.Core;

public class KeyValuePairsStringParser {
    public Dictionary<string, string> Parse(string value) {
        var output = new Dictionary<string, string>();

        foreach (var part in value.Split(',', StringSplitOptions.RemoveEmptyEntries)) {
            var pieces = part.Split('=', 2);
            if (pieces.Length != 2)
                throw new ArgumentException("Invalid format of key value pairs string");

            output[pieces[0]] = pieces[1];
        }

        return output;
    }
}