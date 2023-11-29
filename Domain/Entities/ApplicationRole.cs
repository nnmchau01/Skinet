﻿using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class ApplicationRole : IdentityRole<Guid>
{
    public string? Description { get; set; }
}