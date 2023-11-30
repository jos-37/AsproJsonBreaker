namespace AsproRDTool.ServiceContracts.Models
{
    public class Tooth
    {
        public int id { get; set; }
        public string? name { get; set; }
        public string? note { get; set; }
        public Position? position { get; set; }
        public bool? isDeleted { get; set; }
        public Toothstatus? toothStatus { get; set; }
        public int toothChartId { get; set; }
        public int? proceduresToMouthId { get; set; }
        public int? procedureTeethId { get; set; }

    }
}