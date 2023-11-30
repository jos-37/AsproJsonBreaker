namespace AsproRDTool.ServiceContracts.Models
{
    public class Procedure
    {
        public int id { get; set; }
        public string code { get; set; }
        public string value { get; set; }
        public string description { get; set; }
        public bool downgradable { get; set; }
        public string? procedureGroup { get; set; }
        public string? friendlyDescription { get; set; }
        public int toothChartId { get; set; }
        public int? proceduresToMouthId { get; set; }
        public int? procedureTeethId { get; set; }

    }
}