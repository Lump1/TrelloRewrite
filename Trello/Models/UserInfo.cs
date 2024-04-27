﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Trello;

public partial class UserInfo
{
    public long Id { get; set; }

    public string? Username { get; set; } = null!;

    public string? Email { get; set; } = null!;

    public string? Password { get; set; } = null!;

    public string? Role { get; set; }
    [JsonIgnore]
    public virtual ICollection<TeamUser> TeamUsers { get; set; } = new List<TeamUser>();

    [JsonIgnore]
    public virtual ICollection<UserCard> UserCards { get; set; } = new List<UserCard>();
}
