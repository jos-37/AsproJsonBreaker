namespace AsproRDTool.ServiceContracts.Models
{
    public class Toothstatus
    {
        public int id { get; set; }
        public bool? implant { get; set; }
        public bool? impacted { get; set; }
        public bool? mobility { get; set; }
        public bool? furcation { get; set; }
        public string[]? gumRegions { get; set; }
        public string? mobilityLevel { get; set; }
        public string? furcationLevel { get; set; }
        public bool? spaceMaintainer { get; set; }
        public bool? tissueRecession { get; set; }
        public bool? markBleedingGums { get; set; }
        public bool? periodontalDisease { get; set; }
        public string? tissueRecessionLevel { get; set; }
        public string? periodontalDiseaseLevel { get; set; }
        public int toothChartId { get; set; }
        public int toothId { get; set; }
        public int procedureTeethId { get; set; }
    }
}