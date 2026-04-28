using MakAkademi.Data;
using MakAkademi.Models.Entities;

namespace MakAkademi.Middleware
{
    /// <summary>
    /// Her public sayfayı PageViews tablosuna kayıt eden middleware.
    /// Django'daki PageView modeli ile aynı mantık: benzersiz session key başına 30dk cooldown.
    /// </summary>
    public class PageViewMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<PageViewMiddleware> _logger;

        // Admin, API ve statik dosya yolları sayılmaz
        private static readonly string[] _skipPrefixes =
        [
            "/Admin", "/api", "/lib", "/css", "/js", "/images",
            "/uploads", "/_framework", "/favicon", "/.well-known"
        ];

        public PageViewMiddleware(RequestDelegate next, ILogger<PageViewMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IServiceScopeFactory scopeFactory)
        {
            var path = context.Request.Path.Value ?? "/";

            // Skip non-GET and non-HTML paths
            if (context.Request.Method != "GET" ||
                _skipPrefixes.Any(p => path.StartsWith(p, StringComparison.OrdinalIgnoreCase)) ||
                path.Contains('.'))
            {
                await _next(context);
                return;
            }

            // Session key oluştur (yoksa yeni bir tane)
            if (!context.Session.TryGetValue("session_key", out var sessionBytes))
            {
                var key = Guid.NewGuid().ToString("N");
                context.Session.SetString("session_key", key);
            }

            var sessionKey = context.Session.GetString("session_key") ?? Guid.NewGuid().ToString("N");

            // Arka planda kaydet (request'i bloklamaz)
            _ = Task.Run(async () =>
            {
                try
                {
                    using var scope = scopeFactory.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    db.PageViews.Add(new PageView
                    {
                        Path = path,
                        SessionKey = sessionKey,
                        UserAgent = context.Request.Headers.UserAgent.ToString(),
                        IpAddress = context.Connection.RemoteIpAddress?.ToString() ?? "",
                        CreatedAt = DateTime.Now
                    });
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "PageView kaydedilemedi: {Path}", path);
                }
            });

            await _next(context);
        }
    }

    public static class PageViewMiddlewareExtensions
    {
        public static IApplicationBuilder UsePageViewTracking(this IApplicationBuilder app)
            => app.UseMiddleware<PageViewMiddleware>();
    }
}
