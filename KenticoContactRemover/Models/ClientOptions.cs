namespace I2M.KenticoContactRemover.Models
{
    public class ClientOptions
    {
        public string ConnectionString { get; set; }
        public string InactiveQuery { get; set; }
        public int InactivePeriod { get; set; }
        public int BatchLimit { get; set; }
    }
}
