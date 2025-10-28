namespace eventcollectingservice.Models.BetCity
{
    public class BetCityEventData
    {
        public int EventId { get; set; }
        public string EventDate { get; set; } = string.Empty;
        public string EventDateTime { get; set; } = string.Empty;
        public int LeagueGroupId { get; set; }
        public int LeagueId { get; set; }
        public string League { get; set; } = string.Empty;
        public int HomeId { get; set; }
        public string Home { get; set; } = string.Empty;
        public int AwayId { get; set; }
        public string Away { get; set; } = string.Empty;
        public string HomeCountry { get; set; } = string.Empty;
        public string AwayCountry { get; set; } = string.Empty;
        public int Status { get; set; }
        public int Future { get; set; }
        public int Statistics { get; set; }
    }

    public class BetCityEventsRequestDto
    {
        public List<BetCityEventData>? Events { get; set; }
    }
}
