﻿#nullable disable

using Patreon.Net;
using Patreon.Net.Models;

namespace Herramientas
{
	public static class Patreon
	{
		public static async void Leer()
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder();

			PatreonClient cliente = new PatreonClient(
				builder.Configuration.GetValue<string>("Patreon:CreatorAccessToken"),
				builder.Configuration.GetValue<string>("Patreon:CreatorRefreshToken"),
				builder.Configuration.GetValue<string>("Patreon:ClientID")
			);

			PatreonResourceArray<Campaign, CampaignRelationships> campañas = await cliente.GetCampaignsAsync(Includes.All);

			await foreach (var campaña in campañas)
			{
				PatreonResourceArray<Member, MemberRelationships> usuarios = await cliente.GetCampaignMembersAsync(campaña.Id);

				if (usuarios != null)
				{
					do
					{
						foreach (var usuario in usuarios.Resources)
						{
							if (string.IsNullOrEmpty(usuario.Email) == false)
							{

							}
						}

						string siguientePagina = usuarios.Meta.Pagination.Cursor?.Next;

						if (siguientePagina != null)
						{
							usuarios = await cliente.GetCampaignMembersAsync(campaña.Id, siguientePagina);
						}							
						else
						{
							usuarios = null;
						}			
					}
					while (usuarios != null);
				}
			}

			cliente.Dispose();
		}
	}
}
