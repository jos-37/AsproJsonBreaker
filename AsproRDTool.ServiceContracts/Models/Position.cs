namespace AsproRDTool.ServiceContracts.Models
{
    public class Position
    {
        public int number { get; set; }
        public string quadrant { get; set; }
        public bool deciduous { get; set; }
        public string teethType { get; set; }
        public int toothChartId { get; set; }
        public int toothId { get; set; }
        public int procedureTeethToothId { get; set; }
    }

}