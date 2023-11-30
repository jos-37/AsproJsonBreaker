namespace AsproRDTool.ServiceContracts.Models
{
    public class ProceduresToMouth
    {
        public int id { get; set; }
        public string? arch { get; set; }
        public string? note { get; set; }
        public Tooth? tooth { get; set; }
        public string? status { get; set; }
        public bool? deleted { get; set; }
        public Priority? priority { get; set; }
        public string? quadrant { get; set; }
        public bool? addedLate { get; set; }
        public bool? completed { get; set; }
        public string? placement { get; set; }
        public Procedure? procedure { get; set; }
        public bool? additional { get; set; }
        public long? deletedDate { get; set; }
        public int[]? diagnosisIds { get; set; }
        public Internalcode? internalCode { get; set; }
        public string[]? toothRegions { get; set; }
        public long? completedDate { get; set; }
        public DeletedDoctor deletedByDoctor { get; set; }
        public bool? deletedCompleted { get; set; }
        public int toothChartId { get; set; }
    }

}
