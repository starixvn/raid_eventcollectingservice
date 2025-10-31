using System;

namespace eventcollectingservice.Models.Futbol24
{
    /// <summary>
    /// 對應 'match' table
    /// 為了避免與 C# 關鍵字或常用類 (System.Text.RegularExpressions.Match) 衝突，
    /// 建議將 class 命名為 GameMatch 或類似名稱。
    /// </summary>
    public class GameMatch
    {
        public int Id { get; set; }
        public int Team1Id { get; set; }
        public int Team2Id { get; set; }
        public bool IsActive { get; set; }
        public int Status { get; set; }
        public DateTime KickoffTime { get; set; }
        public int LeagueId { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
