namespace MessageBroker.Abstractions
{
    public abstract record BaseContract
    {
        public bool IsSuccess { get; init; } = true;
        public string? ErrorMessage { get; init; }
        public string? ErrorCode { get; init; }
        public List<string>? Errors { get; init; }
        public Dictionary<string, string>? FieldErrors { get; init; }
    }
}