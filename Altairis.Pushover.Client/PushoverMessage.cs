namespace Altairis.Pushover.Client;

public record PushoverMessage(string User, string Message) {

    public string? Device { get; set; }

    public string? Title { get; set; }

    public MessageFormat Format { get; set; } = MessageFormat.Default;

    public DateTimeOffset? Timestamp { get; set; }

    public Uri? SupplementaryUrl { get; set; }

    public string? SupplementaryUrlTitle { get; set; }

    public string Sound { get; set; } = MessageSound.Default;

    public MessagePriority Priority { get; set; } = MessagePriority.Normal();

};

public class MessagePriority {

    // Factory methods

    public static MessagePriority Lowest() => new() { Level = MessagePriorityLevel.Lowest };

    public static MessagePriority Low() => new() { Level = MessagePriorityLevel.Low };

    public static MessagePriority Normal() => new() { Level = MessagePriorityLevel.Normal };

    public static MessagePriority High() => new() { Level = MessagePriorityLevel.High };

    public static MessagePriority Emergency(TimeSpan retry, TimeSpan expires, Uri? callback = null, params string[] tags) => new() {
        Level = MessagePriorityLevel.Emergency,
        Callback = callback,
        Expire = expires,
        Retry = retry,
        Tags = tags
    };

    // Properties

    public MessagePriorityLevel Level { get; private init; }

    public TimeSpan Retry { get; private init; }

    public TimeSpan Expire { get; private init; }

    public Uri? Callback { get; private init; }

    public string[] Tags { get; private init; } = new string[0];

}

public enum MessageFormat {
    Default = 0,
    Html = 1,
    Monospace = 2,
}

public enum MessagePriorityLevel {
    Lowest = -2,
    Low = -1,
    Normal = 0,
    High = 1,
    Emergency = 2
}

public static class MessageSound {
    public const string Pushover = "pushover";
    public const string Bike = "bike";
    public const string Bugle = "bugle";
    public const string CashRegister = "cashregister";
    public const string Classical = "classical";
    public const string Cosmic = "cosmic";
    public const string Falling = "falling";
    public const string Gamelan = "gamelan";
    public const string Incoming = "incoming";
    public const string Intermission = "intermission";
    public const string Magic = "magic";
    public const string Mechanical = "mechanical";
    public const string PianoBar = "pianobar";
    public const string Siren = "siren";
    public const string SpaceAlarm = "spacealarm";
    public const string TugBoat = "tugboat";
    public const string AlienAlarm = "alien";
    public const string Climb = "climb";
    public const string Persistent = "persistent";
    public const string PushoverEcho = "echo";
    public const string UpDown = "updown";
    public const string Vibrate = "vibrate";
    public const string None = "none";
    public const string Default = "";
}