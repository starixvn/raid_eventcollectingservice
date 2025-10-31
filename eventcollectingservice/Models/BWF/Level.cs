namespace eventcollectingservice.Models.BWF
{
    /// <summary>
    /// 對應 'bwf_level' table
    /// </summary>
    public class Level
    {
        public int Id { get; set; }
        
        // name 允許 Null，所以使用 string?
        public string? Name { get; set; }
        
        public DateTime UpdateTime { get; set; }
        public DateTime CreateTime { get; set; }
    }
}

