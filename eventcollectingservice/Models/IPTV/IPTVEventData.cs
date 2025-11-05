namespace eventcollectingservice.Models.IPTV
{
    public class IPTVEventData
    {
        public int EventId { get; set; }
        public DateTime KickoffTime { get; set; }
        public string LeagueName { get; set; } = string.Empty;
        public string HomeTeamName { get; set; } = string.Empty;
        public string AwayTeamName { get; set; } = string.Empty;
        public string Channel { get; set; } = string.Empty;
        public bool Status { get; set; }
        public bool Alive { get; set; }
    }

    public class IPTVEventRequest
    {
        public List<IPTVEventData>? Events { get; set; }
    }
}
