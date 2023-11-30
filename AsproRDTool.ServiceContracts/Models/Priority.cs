namespace AsproRDTool.ServiceContracts.Models
{
    public class Priority
    {
        public int? id { get; set; }
        public string name { get; set; }
        public int toothChartId { get; set; }
        public int? proceduresToMouthId { get; set; }
        public int? procedureTeethId { get; set; }
    }
}