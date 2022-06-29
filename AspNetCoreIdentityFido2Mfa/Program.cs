using AspNetCoreIdentityFido2Mfa.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Fido2NetLib;
using Fido2Identity;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddTokenProvider<Fifo2UserTwoFactorTokenProvider>("FIDO2");

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.Secure = CookieSecurePolicy.Always;
    options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
    //options.OnAppendCookie = cookieContext =>
    //    CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
    //options.OnDeleteCookie = cookieContext =>
    //    CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
});

builder.Services.AddControllers();

builder.Services.AddRazorPages();

builder.Services.Configure<Fido2Configuration>(builder.Configuration.GetSection("fido2"));
builder.Services.AddScoped<Fido2Store>();
// Adds a default in-memory implementation of IDistributedCache.
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapControllers();
});

app.Run();
