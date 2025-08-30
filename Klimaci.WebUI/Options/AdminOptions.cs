namespace Klimaci.WebUI.Options
{
    public sealed class AdminOptions
    {
        public string Email { get; set; } = "admin@admin.com";
        public string UserName { get; set; } = "admin";
        public string Password { get; set; } = "Admin.123";
        public string Role { get; set; } = "Admin";
        public bool ResetPasswordOnStartup { get; set; } = false;
    }
}
