namespace AsproRDTool.ServiceContracts.Models
{
    public class DeletedDoctor
    {
        public int id { get; set; }
        public string fullName { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public int procedureTeethId { get; set; }
    }
}
