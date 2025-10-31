namespace eventcollectingservice.Models.BWF
{
    /// <summary>
    /// 對應 'bwf_player' table
    /// </summary>
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public DateTime UpdateTime { get; set; }
        public DateTime CreateTime { get; set; }
    }
}

