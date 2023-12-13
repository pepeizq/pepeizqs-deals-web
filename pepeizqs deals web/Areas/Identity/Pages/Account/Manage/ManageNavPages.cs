#nullable disable

using Microsoft.AspNetCore.Mvc.Rendering;

namespace  pepeizqs_deals_web.Areas.Identity.Pages.Account.Manage
{
    public static class ManageNavPages
    {
        public static string Keys => "Keys";

        public static string Notifications => "Notifications";

        public static string SteamAccount => "SteamAccount";

        public static string Index => "Index";

        public static string Email => "Email";

        public static string ChangePassword => "ChangePassword";

        public static string DownloadPersonalData => "DownloadPersonalData";

        public static string DeletePersonalData => "DeletePersonalData";

        public static string ExternalLogins => "ExternalLogins";

        public static string PersonalData => "PersonalData";

        public static string TwoFactorAuthentication => "TwoFactorAuthentication";

        public static string KeysNavClass(ViewContext viewContext) => PageNavClass(viewContext, Keys);

        public static string NotificationsNavClass(ViewContext viewContext) => PageNavClass(viewContext, Notifications);

		public static string SteamAccountNavClass(ViewContext viewContext) => PageNavClass(viewContext, SteamAccount);

        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        public static string EmailNavClass(ViewContext viewContext) => PageNavClass(viewContext, Email);

        public static string ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangePassword);

        public static string DownloadPersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, DownloadPersonalData);

        public static string DeletePersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, DeletePersonalData);

        public static string ExternalLoginsNavClass(ViewContext viewContext) => PageNavClass(viewContext, ExternalLogins);

        public static string PersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, PersonalData);

        public static string TwoFactorAuthenticationNavClass(ViewContext viewContext) => PageNavClass(viewContext, TwoFactorAuthentication);

        public static string PageNavClass(ViewContext viewContext, string pagina)
        {
            string paginaActiva = viewContext.ViewData["ActivePage"] as string
                ?? Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);

            if (string.Equals(paginaActiva, pagina, StringComparison.OrdinalIgnoreCase) == true)
            {
                return "active";
            }
            else
            {
                return null;
            }
        }
    }
}
