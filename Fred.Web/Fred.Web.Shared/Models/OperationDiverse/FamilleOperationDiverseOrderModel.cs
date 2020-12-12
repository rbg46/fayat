namespace Fred.Web.Shared.Models.OperationDiverse
{
    public class FamilleOperationDiverseOrderModel : FamilleOperationDiverseModel
    {
        public string[] CodeOrder { get; } = new[] { "RCT", "MO", "ACH", "MIT", "MI", "OTH", "FG", "OTHD" };
    }
}
