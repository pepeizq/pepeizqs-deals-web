﻿using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace pepeizqs_deals_web.Areas.Identity.Data;

public class Usuario : IdentityUser
{
	[PersonalData]
	[Column(TypeName = "nvarchar(100)")]
    public string? Role { get; set; }

	[PersonalData]
	[Column(TypeName = "nvarchar(256)")]
	public string? SteamAccount { get; set; }

    [PersonalData]
    [Column(TypeName = "nvarchar(max)")]
    public string? SteamGames { get; set; }

    [PersonalData]
    [Column(TypeName = "nvarchar(max)")]
    public string? SteamWishlist { get; set; }
}

