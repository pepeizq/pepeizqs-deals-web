//Comandos:
//Add-Migration "Initial Crate"
//Update-Database

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace pepeizqs_deals_web.Areas.Identity.Data;

public class Usuario : IdentityUser
{
    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string? Nickname { get; set; }

    [PersonalData]
	[Column(TypeName = "nvarchar(100)")]
    public string? Role { get; set; }

	[PersonalData]
	[Column(TypeName = "nvarchar(256)")]
	public string? SteamAccount { get; set; }

	[PersonalData]
	[Column(TypeName = "nvarchar(256)")]
	public string? SteamAccountLastCheck { get; set; }

	[PersonalData]
    [Column(TypeName = "nvarchar(max)")]
    public string? SteamGames { get; set; }

    [PersonalData]
    [Column(TypeName = "nvarchar(max)")]
    public string? SteamWishlist { get; set; }

	[PersonalData]
	[Column(TypeName = "nvarchar(max)")]
	public string? Wishlist { get; set; }

	[PersonalData]
	[Column(TypeName = "nvarchar(256)")]
	public string? Avatar { get; set; }

	[PersonalData]
	[Column(TypeName = "nvarchar(100)")]
	public string? OfficialGroup { get; set; }

	[PersonalData]
	[Column(TypeName = "nvarchar(max)")]
	public string? Keys { get; set; }

	[PersonalData]
	[Column(TypeName = "bit(1)")]
	public bool? NotificationLows { get; set; }

	[PersonalData]
	[Column(TypeName = "bit(1)")]
	public bool? NotificationNews { get; set; }

	[PersonalData]
	[Column(TypeName = "nvarchar(100)")]
	public string? OfficialGroup2 { get; set; }

	[PersonalData]
	[Column(TypeName = "nvarchar(100)")]
	public string? SteamId { get; set; }

	[PersonalData]
	[Column(TypeName = "int(4)")]
	public int? WishlistSort { get; set; }

	[PersonalData]
	[Column(TypeName = "bit(1)")]
	public bool? WishlistOption1 { get; set; }

	[PersonalData]
	[Column(TypeName = "int(4)")]
	public int? HistoricalLowsSort { get; set; }

	[PersonalData]
	[Column(TypeName = "bit(1)")]
	public bool? HistoricalLowsOption1 { get; set; }
}

