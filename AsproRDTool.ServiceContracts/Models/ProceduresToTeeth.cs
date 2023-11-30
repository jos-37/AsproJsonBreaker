namespace AsproRDTool.ServiceContracts.Models
{
    public class ProceduresToTeeth
    {
        public Tooth tooth { get; set; }
        public ProcedureTeeth[]? procedureTeeth { get; set; }
        public int toothChartId { get; set; }
        public int? toothId { get; set; }
        public int? procedureTeethId { get; set; }

    }

}



