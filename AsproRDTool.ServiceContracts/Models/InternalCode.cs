namespace AsproRDTool.ServiceContracts.Models
{
    public class Internalcode
    {
        public int id { get; set; }
        public string code { get; set; }
        public float charge { get; set; }
        public float credit { get; set; }
        public bool system { get; set; }
        public bool deleted { get; set; }
        public bool required { get; set; }
        public string description { get; set; }
        public bool? insurancePayment { get; set; }
        public string internalCodeGroup { get; set; }
        public int toothChartId { get; set; }
        public int? proceduresToMouthId { get; set; }
        public int? procedureteethId { get; set; }
    }
}
