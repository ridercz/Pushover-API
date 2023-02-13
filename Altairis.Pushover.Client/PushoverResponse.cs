namespace Altairis.Pushover.Client;

public record PushoverResponse {

    public bool Status { get; init; }

    public string Request { get; init; } = string.Empty;

    public ICollection<string> Errors { get; init; } = new HashSet<string>();

}

public record PushoverMessageResponse : PushoverResponse {

    public string? Receipt { get; init; }

}

public record PushoverReceiptResponse : PushoverResponse {

    public bool Acknowledged { get; set; }

    public DateTimeOffset? AcknowledgedAt { get; set; }

    public string? AcknowledgedBy { get; set; }

    public string? AcknowledgedByDevice { get; set; }

    public DateTimeOffset? LastDeliveredAt { get; set; }

    public bool Expired { get; set; }

    public DateTimeOffset ExpiresAt { get; set; }

    public bool CalledBack { get; set; }

    public DateTimeOffset? CalledBackAt { get; set; }

}

public record PushoverLimitsResponse : PushoverResponse {

    public int Limit { get; set; }

    public int Remaining { get; set; }

    public DateTimeOffset Reset { get; set; }

}

public record PushoverValidateResponse : PushoverResponse {

    public bool Group { get; set; }

    public ICollection<string> Devices { get; set; } = new HashSet<string>();

    public ICollection<string> Licenses { get; set; } = new HashSet<string>();

}

public record PushoverGroupResponse : PushoverResponse {

    public record GroupUser(string User, string? Device, string? Memo, bool Disabled, bool Valid);

    public string Name { get; set; } = string.Empty;

    public ICollection<GroupUser> Users { get; set; } = new HashSet<GroupUser>();

}

public record PushoverSoundsResponse : PushoverResponse {

    public Dictionary<string, string> Sounds { get; set; } = new();

}