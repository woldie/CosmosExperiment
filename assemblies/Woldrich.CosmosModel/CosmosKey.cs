namespace Woldrich.CosmosModel;

public class CosmosKey {
    public Guid HashKey { get; set; }
    public Guid RangeKey { get; set; }

    public void GenerateRandomKeys() {
        HashKey = Guid.NewGuid();
        RangeKey = Guid.NewGuid();
    }
}