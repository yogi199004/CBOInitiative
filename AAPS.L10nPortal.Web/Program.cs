using CAPPortal.Bal;
using CAPPortal.Bal.AzureBlob;
using CAPPortal.Bal.Services;
using CAPPortal.Bal.Translation;
using CAPPortal.Bal.TranslationExchange;
using CAPPortal.Contracts.Managers;
using CAPPortal.Contracts.Providers;
using CAPPortal.Contracts.Repositories;
using CAPPortal.Contracts.Services;
using CAPPortal.Dal;
using CAPPortal.Secrets;
using CAPPortal.Web.Handlers;
using CAPPortal.Common;
using CAPPortal.Common.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.FileProviders;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Serilog;


var builder = WebApplication.CreateBuilder(args);
var configValue = builder.Configuration.GetSection("AppSettings").Get<AppSettings>();
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
     .Enrich.WithProperty("Environment", "UI - " + configValue.Environment)
    .WriteTo.Seq(configValue.Sequrl, period: TimeSpan.Zero, batchPostingLimit: 5).CreateLogger();
builder.Logging.AddSerilog(logger);


builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
    options.HandleSameSiteCookieCompatibility();
});


builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(options => builder.Configuration.Bind("AzureAd", options));

//    .AddCookie(DefaultAuthenticationTypes.ApplicationCookie, options =>
//{

//    options.LoginPath = "/Login";
//    options.LogoutPath = "/Logout";
//});


builder.WebHost.ConfigureKestrel(options =>
{
    options.AddServerHeader = false;
});


builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = int.MaxValue;
});


builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MultipartBoundaryLengthLimit = int.MaxValue;
    o.MultipartHeadersCountLimit = int.MaxValue;
    o.MultipartHeadersLengthLimit = int.MaxValue;
    o.BufferBodyLengthLimit = int.MaxValue;
    o.BufferBody = true;
    o.ValueCountLimit = int.MaxValue;
});

builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
}).AddMicrosoftIdentityUI().AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null); ;

builder.Services.AddRazorPages();

//Adding AppInsights Logging configuration


builder.Services.AddAntiforgery(options => options.Cookie.Name = ".AspNetCore.Antiforgery");


// Add services to the container.

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddSingleton<IOpmDataProvider, OpmDataProvider>();

builder.Services.AddSingleton<IConnectionStringProvider, AppConfigConnectionStringProvider>();

builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IPrincipalDataService, PrincipalDataService>();
builder.Services.AddSingleton<IUserManager, UserManager>();
builder.Services.AddSingleton<IPermissionDataService, PermissionDataService>();
builder.Services.AddSingleton<ILocaleManager, LocaleManager>();
builder.Services.AddSingleton<ILocaleRepository, LocaleRepository>();
builder.Services.AddSingleton<IApplicationLocaleRepository, ApplicationLocaleRepository>();
builder.Services.AddSingleton<ITranslationManager, TranslationManager>();
builder.Services.AddSingleton<IAzureKeyVaultDataProvider, AzureKeyVaultDataProvider>();
builder.Services.AddSingleton<ITranslationExchangeManager, TranslationExchangeManager>();
builder.Services.AddSingleton<IApplicationLocaleManager, ApplicationLocaleManager>();
builder.Services.AddSingleton<IApplicationLocaleAssetManager, ApplicationLocaleAssetManager>();
builder.Services.AddSingleton<IApplicationLocaleAssetRepository, ApplicationLocaleAssetRepository>();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();

//Add response headers

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(
               Path.Combine(Directory.GetCurrentDirectory(), "App", "bundles")),

    RequestPath = new PathString("")
});


app.UseMiddleware<ExceptionHandler>();


BlobService.IConfigurationConfigure(app.Services.GetService<IConfiguration>());

app.UseCookiePolicy();
app.UseRouting();

app.UseAuthentication();


app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
   pattern: "{controller=Home}/{action=Index}/{id?}");

CAPPortal.Web.MapperConfig.RegisterMaps();
app.Run();
