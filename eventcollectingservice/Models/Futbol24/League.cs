using System;

namespace eventcollectingservice.Models.Futbol24
{
    /// <summary>
    /// 對應 'league' table
    /// </summary>
    public class League
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }

        // parent_league_id 允許 Null (TRUE)，所以使用 int?
        public int? ParentLeagueId { get; set; }

        public DateTime UpdateTime { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
