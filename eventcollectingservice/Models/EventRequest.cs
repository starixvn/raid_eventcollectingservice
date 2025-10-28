namespace eventcollectingservice.Models
{
    public class EventRequest
    {
        public string? EventName { get; set; }
        public object? EventData { get; set; }
    }
}
