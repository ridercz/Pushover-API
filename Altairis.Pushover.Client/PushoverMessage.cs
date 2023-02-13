namespace Altairis.Pushover.Client;

/// <summary>
/// This record represents properties of single message sent via Pushover.
/// </summary>
public record PushoverMessage(string User, string Message) {

    /// <summary>
    /// Gets or sets the device names.
    /// </summary>
    /// <value>
    /// The device names, separated by comma.
    /// </value>
    public string? Device { get; set; }

    /// <summary>
    /// Gets or sets the title of the message.
    /// </summary>
    /// <value>
    /// The title.
    /// </value>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the message format.
    /// </summary>
    /// <value>
    /// The format.
    /// </value>
    public MessageFormat Format { get; set; } = MessageFormat.Default;

    /// <summary>
    /// Gets or sets the message timestamp. If not explicitly set, API will use current date and time.
    /// </summary>
    /// <value>
    /// The timestamp.
    /// </value>
    public DateTimeOffset? Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the supplementary URL. When set, the <see cref="PushoverMessage.SupplementaryUrlTitle"/> property must be set as well.
    /// </summary>
    /// <value>
    /// The supplementary URL.
    /// </value>
    public Uri? SupplementaryUrl { get; set; }

    /// <summary>
    /// Gets or sets the supplementary URL title. When set, the <see cref="PushoverMessage.SupplementaryUrl"/> property must be set as well.
    /// </summary>
    /// <value>
    /// The supplementary URL title.
    /// </value>
    public string? SupplementaryUrlTitle { get; set; }

    /// <summary>
    /// Gets or sets the notification sound.
    /// </summary>
    /// <value>
    /// The sound.
    /// </value>
    public string Sound { get; set; } = MessageSound.Default;

    /// <summary>
    /// Gets or sets the message priority.
    /// </summary>
    /// <value>
    /// The priority.
    /// </value>
    public MessagePriority Priority { get; set; } = MessagePriority.Normal();

};

/// <summary>
/// This class represents the message notification priority.
/// </summary>
public class MessagePriority {

    // Factory methods

    /// <summary>
    /// Gets the lowest priority. No notification is shown.
    /// </summary>
    /// <returns></returns>
    public static MessagePriority Lowest() => new() { Level = MessagePriorityLevel.Lowest };

    /// <summary>
    /// Gets the low priority. Only visual notification is shown.
    /// </summary>
    /// <returns></returns>
    public static MessagePriority Low() => new() { Level = MessagePriorityLevel.Low };

    /// <summary>
    /// Gets the normal priority. Notifications are used according to user preferences.
    /// </summary>
    /// <returns></returns>
    public static MessagePriority Normal() => new() { Level = MessagePriorityLevel.Normal };

    /// <summary>
    /// Gets the high priority. Notifications bypass user quiet hours.
    /// </summary>
    /// <returns></returns>
    public static MessagePriority High() => new() { Level = MessagePriorityLevel.High };

    /// <summary>
    /// Gets the emergency priority.
    /// </summary>
    /// <param name="retry">The notification retry time.</param>
    /// <param name="expires">The notification retry expires.</param>
    /// <param name="callback">The callback URL.</param>
    /// <param name="tags">The tags, may be used to cancel the message.</param>
    /// <returns></returns>
    public static MessagePriority Emergency(TimeSpan retry, TimeSpan expires, Uri? callback = null, params string[] tags) => new() {
        Level = MessagePriorityLevel.Emergency,
        Callback = callback,
        Expire = expires,
        Retry = retry,
        Tags = tags
    };

    // Properties

    /// <summary>
    /// Gets the level.
    /// </summary>
    /// <value>
    /// The level.
    /// </value>
    public MessagePriorityLevel Level { get; private init; }

    /// <summary>
    /// Gets the retry time.
    /// </summary>
    /// <value>
    /// The retry time.
    /// </value>
    public TimeSpan Retry { get; private init; }

    /// <summary>
    /// Gets the retry expire time.
    /// </summary>
    /// <value>
    /// The retry expire time.
    /// </value>
    public TimeSpan Expire { get; private init; }

    /// <summary>
    /// Gets the callback URL.
    /// </summary>
    /// <value>
    /// The callback URL.
    /// </value>
    public Uri? Callback { get; private init; }

    /// <summary>
    /// Gets the tags.
    /// </summary>
    /// <value>
    /// The tags.
    /// </value>
    public string[] Tags { get; private init; } = new string[0];

}

/// <summary>
/// Message text format
/// </summary>
public enum MessageFormat {
    /// <summary>
    /// The default format, URLs are automatically converted to links.
    /// </summary>
    Default = 0,
    /// <summary>
    /// The HTML format, simple tags as b, i, u, a, font color are permitted.
    /// </summary>
    Html = 1,
    /// <summary>
    /// The monospace format, monospace font is used to display message.
    /// </summary>
    Monospace = 2,
}

/// <summary>
/// Message priority
/// </summary>
public enum MessagePriorityLevel {
    /// <summary>
    /// The lowest priority (no notifications).
    /// </summary>
    Lowest = -2,
    /// <summary>
    /// The low priority (visual notifications only).
    /// </summary>
    Low = -1,
    /// <summary>
    /// The normal priority.
    /// </summary>
    Normal = 0,
    /// <summary>
    /// The high priority (bypass user quiet hours).
    /// </summary>
    High = 1,
    /// <summary>
    /// The emergency priority (repeat until acknowledged).
    /// </summary>
    Emergency = 2
}

/// <summary>
/// Message sounds.
/// </summary>
public static class MessageSound {
    /// <summary>
    /// The pushover sound.
    /// </summary>
    public const string Pushover = "pushover";
    /// <summary>
    /// The bike sound.
    /// </summary>
    public const string Bike = "bike";
    /// <summary>
    /// The bugle sound.
    /// </summary>
    public const string Bugle = "bugle";
    /// <summary>
    /// The cash register sound.
    /// </summary>
    public const string CashRegister = "cashregister";
    /// <summary>
    /// The classical sound.
    /// </summary>
    public const string Classical = "classical";
    /// <summary>
    /// The cosmic sound.
    /// </summary>
    public const string Cosmic = "cosmic";
    /// <summary>
    /// The falling sound.
    /// </summary>
    public const string Falling = "falling";
    /// <summary>
    /// The gamelan sound.
    /// </summary>
    public const string Gamelan = "gamelan";
    /// <summary>
    /// The incoming sound.
    /// </summary>
    public const string Incoming = "incoming";
    /// <summary>
    /// The intermission sound.
    /// </summary>
    public const string Intermission = "intermission";
    /// <summary>
    /// The magic sound.
    /// </summary>
    public const string Magic = "magic";
    /// <summary>
    /// The mechanical sound.
    /// </summary>
    public const string Mechanical = "mechanical";
    /// <summary>
    /// The piano bar sound.
    /// </summary>
    public const string PianoBar = "pianobar";
    /// <summary>
    /// The siren sound.
    /// </summary>
    public const string Siren = "siren";
    /// <summary>
    /// The space alarm sound.
    /// </summary>
    public const string SpaceAlarm = "spacealarm";
    /// <summary>
    /// The tug boat sound.
    /// </summary>
    public const string TugBoat = "tugboat";
    /// <summary>
    /// The alien alarm sound.
    /// </summary>
    public const string AlienAlarm = "alien";
    /// <summary>
    /// The climb sound.
    /// </summary>
    public const string Climb = "climb";
    /// <summary>
    /// The persistent sound.
    /// </summary>
    public const string Persistent = "persistent";
    /// <summary>
    /// The pushover echo sound.
    /// </summary>
    public const string PushoverEcho = "echo";
    /// <summary>
    /// The up down sound.
    /// </summary>
    public const string UpDown = "updown";
    /// <summary>
    /// Vibrate only.
    /// </summary>
    public const string Vibrate = "vibrate";
    /// <summary>
    /// No sound.
    /// </summary>
    public const string None = "none";
    /// <summary>
    /// The default sound.
    /// </summary>
    public const string Default = "";
}