namespace AsproRDTool.ServiceContracts.Models
{
    public class JsonBreakerDetail
    {
        public string server { get; set; }
        public string dbName { get; set; }
        public string[] tableList = { "tooth_chart_history", "perio_history" };
    }
}
