using System;

namespace eventcollectingservice.Models.Futbol24
{
    /// <summary>
    /// 對應 'category' table (拼字可能應為 Category)
    /// </summary>
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
