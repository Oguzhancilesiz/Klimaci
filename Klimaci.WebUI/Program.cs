using Klimaci.Core.Abstracts;
using Klimaci.DAL;
using Klimaci.Entity;
using Klimaci.Services.Abstracts;
using Klimaci.Services.Concrete;
using Klimaci.Services.MapsterMap;
using Klimaci.WebUI.Infrastructure;
using Klimaci.WebUI.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Klimaci.WebUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // DbContext + provider
            builder.Services.AddDbContext<BaseContext>(opt =>
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // IEFContext -> BaseContext (scoped)
            builder.Services.AddScoped<IEFContext>(sp => sp.GetRequiredService<BaseContext>());
            builder.Services.AddScoped<IProjectService, ProjectService>();
            builder.Services.AddScoped<IBlogService, BlogService>();
            builder.Services.AddScoped<IPageService, PageService>();
            builder.Services.AddScoped<IPartnerService, PartnerService>();
            builder.Services.AddScoped<IServiceItemService, ServiceItemService>();
            builder.Services.AddScoped<IBrandService, BrandService>();
            builder.Services.AddScoped<ITagService, TagService>();
            builder.Services.AddScoped<IMediaService, MediaService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IContactMessageService, ContactMessageService>();
            builder.Services.AddScoped<ITestimonialService, TestimonialService>();
            builder.Services.AddScoped<INavigationService, NavigationService>();
            builder.Services.AddScoped<ISettingService, SettingService>();
            builder.Services.AddScoped<IUrlRedirectService, UrlRedirectService>();
            builder.Services.AddScoped<ITeamMemberService, TeamMemberService>();
            builder.Services.AddScoped<IFaqService, FaqService>();
            builder.Services.AddScoped<IOfficeService, OfficeService>();
            builder.Services.AddScoped<INewsletterService, NewsletterService>();
            builder.Services.AddScoped<ILeadService, LeadService>();
            builder.Services.AddScoped<IAppointmentRequestService, AppointmentRequestService>();
            builder.Services.AddScoped<IAdminDashboardService, AdminDashboardService>();

            // Options
            builder.Services.Configure<AdminOptions>(builder.Configuration.GetSection("Admin"));

            // UoW
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Identity
            builder.Services
                .AddIdentity<AppUser, AppRole>(opt =>
                {
                    opt.User.RequireUniqueEmail = true;
                    opt.Password.RequiredLength = 6;
                    opt.Lockout.AllowedForNewUsers = true;
                })
                .AddEntityFrameworkStores<BaseContext>()
                .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(opt =>
            {
                opt.LoginPath = "/Admin/Account/Login";
                opt.AccessDeniedPath = "/Admin/Account/Denied";
                opt.LogoutPath = "/Admin/Account/Logout";
                opt.Cookie.Name = "Klimaci.Auth";
                opt.SlidingExpiration = true;
                opt.ExpireTimeSpan = TimeSpan.FromHours(8);
            });

            builder.Services.AddAuthorization(opt =>
            {
                opt.AddPolicy("AdminOnly", p => p.RequireRole("Admin"));
            });

            // AddControllersWithViews tek kez ve convention ile
            builder.Services.AddControllersWithViews(o =>
            {
                o.Conventions.Add(new AdminAreaAuthorization("Admin", "AdminOnly"));
            });

            builder.WebHost.ConfigureKestrel(o => o.Limits.MaxRequestBodySize = 104_857_600); // 100MB

            var app = builder.Build();

            // Migrate + Seed (sync bloklama)
            using (var scope = app.Services.CreateScope())
            {
                var sp = scope.ServiceProvider;
                var db = sp.GetRequiredService<BaseContext>();
                db.Database.Migrate();
                IdentitySeeder.SeedAsync(sp).GetAwaiter().GetResult();
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // Tek çaðrýda StaticFiles + cache header
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers["Cache-Control"] = "public,max-age=2592000";
                }
            });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            // Mapster
            MapsterConfig.Register();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
