using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MakAkademi.Data;
using MakAkademi.Middleware;
using MakAkademi.Services;
using MakAkademi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// ── MVC + Views ──────────────────────────────────────────────────────────────
builder.Services.AddControllersWithViews();

// ── Entity Framework Core (SQL Server) ───────────────────────────────────────
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ── ASP.NET Core Identity ─────────────────────────────────────────────────────
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

// ── Session (30 dakika — Django ile aynı) ────────────────────────────────────
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

// ── Dependency Injection — Servisler ─────────────────────────────────────────
builder.Services.AddScoped<ITestimonialService, TestimonialService>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<ITimeSlotService, TimeSlotService>();
builder.Services.AddScoped<IReservationService, ReservationService>();

// ── Cookie ayarları (Admin giriş yönlendirmesi) ───────────────────────────────
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/yonetim/giris";
    options.LogoutPath = "/yonetim/cikis";
    options.AccessDeniedPath = "/yonetim/giris";
});

var app = builder.Build();

// ── Proxy / Forwarded Headers (Prod için IP tespiti) ──────────────────────────
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// ── Middleware Pipeline ───────────────────────────────────────────────────────
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.UsePageViewTracking();  // Ziyaretçi istatistikleri

// ── Routing ───────────────────────────────────────────────────────────────────

// API Controller'ları (attribute routing)
app.MapControllers();

// Admin Area: /Admin/...

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

// Varsayılan MVC rotası
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ── DB Migration + Seed (ilk çalıştırmada) ──────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();

    // Admin rolü ve kullanıcı seed
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    if (!await roleManager.RoleExistsAsync("Admin"))
        await roleManager.CreateAsync(new IdentityRole("Admin"));

    if (await userManager.FindByNameAsync("admin") == null)
    {
        var adminUser = new IdentityUser { UserName = "admin", Email = "admin@makakademi.com", EmailConfirmed = true };
        var result = await userManager.CreateAsync(adminUser, "Mak123456!");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}

app.Run();
