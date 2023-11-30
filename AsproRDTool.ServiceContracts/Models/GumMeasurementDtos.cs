namespace AsproRDTool.ServiceContracts.Models
{
    public class GumMeasurementDtos
    {
        public int id { get; set; }
        public int toothid { get; set; }
        public int? leftGumMeasurement { get; set; }
        public int? rightGumMeasurement { get; set; }
        public int? middleGumMeasurement { get; set; }
        public int? innerLeftGumMeasurement { get; set; }
        public int? innerRightGumMeasurement { get; set; }
        public int? innerMiddleGumMeasurement { get; set; }
        public int perioHistoryId { get; set; }
    }
}
