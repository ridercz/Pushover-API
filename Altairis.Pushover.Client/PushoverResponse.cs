namespace Altairis.Pushover.Client;

/// <summary>
/// This record represents general response of Pushover API.
/// </summary>
public record PushoverResponse {

    /// <summary>
    /// Gets a value indicating whether this <see cref="PushoverResponse"/> is successful.
    /// </summary>
    /// <value>
    ///   <c>true</c> if request was successful; otherwise, <c>false</c>.
    /// </value>
    public bool Status { get; init; }

    /// <summary>
    /// Gets the request id.
    /// </summary>
    /// <value>
    /// The request id.
    /// </value>
    public string Request { get; init; } = string.Empty;

    /// <summary>
    /// Gets the collection of errors.
    /// </summary>
    /// <value>
    /// The errors.
    /// </value>
    public ICollection<string> Errors { get; init; } = new HashSet<string>();

}

/// <summary>
/// This record represents response to message send request.
/// </summary>
/// <seealso cref="Altairis.Pushover.Client.PushoverResponse" />
public record PushoverMessageResponse : PushoverResponse {

    /// <summary>
    /// Gets the receipt id for Emergency priority messages.
    /// </summary>
    /// <value>
    /// The receipt id.
    /// </value>
    public string? Receipt { get; init; }

}

/// <summary>
/// This record represents response for receipt status request.
/// </summary>
/// <seealso cref="Altairis.Pushover.Client.PushoverResponse" />
public record PushoverReceiptResponse : PushoverResponse {

    /// <summary>
    /// Gets a value indicating whether the message is acknowledged.
    /// </summary>
    /// <value>
    ///   <c>true</c> if acknowledged; otherwise, <c>false</c>.
    /// </value>
    public bool Acknowledged { get; init; }

    /// <summary>
    /// Gets the time when the message was acknowledged.
    /// </summary>
    /// <value>
    /// The time when the message was acknowledged or <c>null</c> if not acknowledged.
    /// </value>
    public DateTimeOffset? AcknowledgedAt { get; init; }

    /// <summary>
    /// Gets the id of user who first acknowledged the message.
    /// </summary>
    /// <value>
    /// The user id or <c>null</c> if not acknowledged.
    /// </value>
    public string? AcknowledgedBy { get; init; }

    /// <summary>
    /// Gets the name of device who first acknowledged this message.
    /// </summary>
    /// <value>
    /// The name of device or <c>null</c> if not acknowledged.
    /// </value>
    public string? AcknowledgedByDevice { get; init; }

    /// <summary>
    /// Gets the time of last notification.
    /// </summary>
    /// <value>
    /// The last notification time.
    /// </value>
    public DateTimeOffset? LastDeliveredAt { get; init; }

    /// <summary>
    /// Gets a value indicating whether the message is expired.
    /// </summary>
    /// <value>
    ///   <c>true</c> if expired; otherwise, <c>false</c>.
    /// </value>
    public bool Expired { get; init; }

    /// <summary>
    /// Gets the time when the message expires.
    /// </summary>
    /// <value>
    /// The time when the message expires.
    /// </value>
    public DateTimeOffset ExpiresAt { get; init; }

    /// <summary>
    /// Gets a value indicating whether the callback URL was requested.
    /// </summary>
    /// <value>
    ///   <c>true</c> if callback URL was called; otherwise, <c>false</c>.
    /// </value>
    public bool CalledBack { get; init; }

    /// <summary>
    /// Gets the time when callback URL was requested.
    /// </summary>
    /// <value>
    /// The time when callback URL was requested.
    /// </value>
    public DateTimeOffset? CalledBackAt { get; init; }

}

/// <summary>
/// This record represents response to the limits request.
/// </summary>
/// <seealso cref="Altairis.Pushover.Client.PushoverResponse" />
public record PushoverLimitsResponse : PushoverResponse {

    /// <summary>
    /// Gets the monthly message limit.
    /// </summary>
    /// <value>
    /// The number of messages.
    /// </value>
    public int Limit { get; init; }

    /// <summary>
    /// Gets the number of remaining messages.
    /// </summary>
    /// <value>
    /// The number of messages.
    /// </value>
    public int Remaining { get; init; }

    /// <summary>
    /// Gets the time when the limit resets.
    /// </summary>
    /// <value>
    /// The time when the limit resets.
    /// </value>
    public DateTimeOffset Reset { get; init; }

}

/// <summary>
/// This record represents response to the user or group validation request.
/// </summary>
/// <seealso cref="Altairis.Pushover.Client.PushoverResponse" />
public record PushoverValidateResponse : PushoverResponse {

    /// <summary>
    /// Gets a value indicating whether the provided id is a group.
    /// </summary>
    /// <value>
    ///   <c>true</c> if id is a group; otherwise, <c>false</c>.
    /// </value>
    public bool Group { get; init; }

    /// <summary>
    /// Gets the devices associated with user.
    /// </summary>
    /// <value>
    /// The devices associated with user.
    /// </value>
    public ICollection<string> Devices { get; init; } = new HashSet<string>();

    /// <summary>
    /// Gets the licenses associated with user.
    /// </summary>
    /// <value>
    /// The licenses associated with user.
    /// </value>
    public ICollection<string> Licenses { get; init; } = new HashSet<string>();

}

/// <summary>
/// This record represents response to the group information request.
/// </summary>
/// <seealso cref="Altairis.Pushover.Client.PushoverResponse" />
public record PushoverGroupResponse : PushoverResponse {

    /// <summary>
    /// This represents single user as a group member
    /// </summary>
    public record GroupUser(string User, string? Device, string? Memo, bool Disabled, bool Valid);

    /// <summary>
    /// Gets the group name.
    /// </summary>
    /// <value>
    /// The group name.
    /// </value>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Gets the group members.
    /// </summary>
    /// <value>
    /// The group members.
    /// </value>
    public ICollection<GroupUser> Users { get; init; } = new HashSet<GroupUser>();

}

/// <summary>
/// This record represents response to the sound list request.
/// </summary>
/// <seealso cref="Altairis.Pushover.Client.PushoverResponse" />
public record PushoverSoundsResponse : PushoverResponse {

    /// <summary>
    /// Gets the supported sounds.
    /// </summary>
    /// <value>
    /// The supported sounds.
    /// </value>
    public Dictionary<string, string> Sounds { get; init; } = new();

}