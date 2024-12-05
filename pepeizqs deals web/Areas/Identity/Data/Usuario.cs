﻿//Comandos:
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
	public bool? NotificationBundles { get; set; }

	[PersonalData]
	[Column(TypeName = "bit(1)")]
	public bool? NotificationFree { get; set; }

	[PersonalData]
	[Column(TypeName = "bit(1)")]
	public bool? NotificationSubscriptions { get; set; }

	[PersonalData]
	[Column(TypeName = "bit(1)")]
	public bool? NotificationOthers { get; set; }

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
	[Column(TypeName = "bit(1)")]
	public bool? WishlistOption2 { get; set; }

	[PersonalData]
	[Column(TypeName = "int(4)")]
	public int? WishlistOption3 { get; set; }

	[PersonalData]
	[Column(TypeName = "decimal(9)")]
	public decimal? WishlistOption4 { get; set; }

	[PersonalData]
	[Column(TypeName = "int(4)")]
	public int? HistoricalLowsSort { get; set; }

	[PersonalData]
	[Column(TypeName = "bit(1)")]
	public bool? HistoricalLowsOption1 { get; set; }

	[PersonalData]
	[Column(TypeName = "int(4)")]
	public int? HistoricalLowsOption2 { get; set; }

	[PersonalData]
	[Column(TypeName = "decimal(9)")]
	public decimal? HistoricalLowsOption3 { get; set; }

	[PersonalData]
	[Column(TypeName = "nvarchar(max)")]
	public string? HistoricalLowsStores { get; set; }

	[PersonalData]
	[Column(TypeName = "nvarchar(max)")]
	public string? HistoricalLowsDRMs { get; set; }

	[PersonalData]
	[Column(TypeName = "nvarchar(max)")]
	public string? HistoricalLowsCategories { get; set; }

	[PersonalData]
	[Column(TypeName = "bit(1)")]
	public bool? IndexOption1 { get; set; }

	[PersonalData]
	[Column(TypeName = "nvarchar(max)")]
	public string? IndexDRMs { get; set; }

	[PersonalData]
	[Column(TypeName = "nvarchar(max)")]
	public string? IndexCategories { get; set; }

    [PersonalData]
    [Column(TypeName = "int(4)")]
    public int? RewardsCoins { get; set; }

    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string? RewardsLastLogin { get; set; }

	[PersonalData]
	[Column(TypeName = "nvarchar(50)")]
	public string? Language { get; set; }

	[PersonalData]
	[Column(TypeName = "datetime(8)")]
	public DateTime? PatreonLastCheck { get; set; }

	[PersonalData]
	[Column(TypeName = "nvarchar(128)")]
	public string? PatreonMail { get; set; }

	[PersonalData]
	[Column(TypeName = "int(4)")]
	public int? PatreonCoins { get; set; }

	[PersonalData]
	[Column(TypeName = "datetime(8)")]
	public DateTime? PatreonLastLogin { get; set; }

	[PersonalData]
	[Column(TypeName = "bit(1)")]
	public bool? MailSummary { get; set; }
}

