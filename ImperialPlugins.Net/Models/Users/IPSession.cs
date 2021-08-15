namespace ImperialPlugins.Models.Users
{
    public class IPSession
    {
        public string UserID;
        public string UserName;

        public Merchant Merchant;

        public IPRole[] Roles;
        public string[] UserClaims;
    }
}