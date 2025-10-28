using System;

namespace eventcollectingservice.Models.Futbol24
{
    /// <summary>
    /// 對應 'team' table
    /// </summary>
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LeagueId { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
