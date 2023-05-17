using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using L10N.API.BAL;
using L10N.API.Common;
using L10N.API.Contracts;
using L10N.API.Secrets;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddSingleton<IAzureKeyVaultDataProvider, AzureKeyVaultDataProvider>();
builder.Services.AddSingleton<ICosmosService, CosmosService>();
builder.Services.AddSingleton<IAPIService, APIService>();

//Adds Microsoft Identity platform to protect this Api
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddMicrosoftIdentityWebApi(options =>
        {
            builder.Configuration.Bind("AzureAd", options);

            options.TokenValidationParameters.NameClaimType = "name";
            options.TokenValidationParameters.ValidateAudience = false;

        },
options => { builder.Configuration.Bind("AzureAd", options); });
//End of the Microsoft Identity platform block   


builder.Services.AddControllers().AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add logging to the container


builder.Services.AddApplicationInsightsTelemetry(builder.Configuration.GetValue<string>("ApplicationInsights:ConnectionString"));
//builder.WebHost.UseKestrel(option => option.AddServerHeader = false);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

ConfigValues.AzureServiceConnectionString = builder.Configuration.GetSection(L10N.API.Common.Constants.AzureServiceConnectionString).Value;
ConfigValues.KeyVaultURI = builder.Configuration.GetSection(L10N.API.Common.Constants.L10nKeyVaultUri).Value;
ConfigValues.ContainerName = builder.Configuration.GetSection(L10N.API.Common.Constants.StorageContainer).Value;

#if LOCAL
ConfigValues.CosmosDbServerURI = builder.Configuration.GetRequiredSection(L10N.API.Common.Constants.CosmosServerURI).Value;
ConfigValues.PrimaryReadOnlyKey = builder.Configuration.GetRequiredSection(L10N.API.Common.Constants.PrimaryKeyReadOnly).Value;
#else
string clientid = builder.Configuration.GetRequiredSection(L10N.API.Common.Constants.ClientId).Value;
var client = new SecretClient(new Uri(ConfigValues.KeyVaultURI), new DefaultAzureCredential( new DefaultAzureCredentialOptions{ ManagedIdentityClientId = clientid }));
var cosmosURI=   client.GetSecret("CosmosServerURI");
var primaryReadOnlyKey = client.GetSecret("PrimaryKeyReadOnly");
var SAKey = client.GetSecret("StorageAccountAccessKey");
ConfigValues.CosmosDbServerURI = cosmosURI.Value.Value; 
ConfigValues.PrimaryReadOnlyKey = primaryReadOnlyKey.Value.Value;
ConfigValues.SAKey = SAKey.Value.Value;
#endif
ConfigValues.DatabaseID = builder.Configuration.GetRequiredSection(L10N.API.Common.Constants.DatabaseId).Value;
ConfigValues.OmniaContainerId = builder.Configuration.GetRequiredSection(L10N.API.Common.Constants.OmniaContainerId).Value;
ConfigValues.LevviaContainerId = builder.Configuration.GetRequiredSection(L10N.API.Common.Constants.LevviaContainerId).Value;
ConfigValues.GeneralAppsContainerId = builder.Configuration.GetRequiredSection(L10N.API.Common.Constants.GeneralAppsContainerId).Value;
ConfigValues.ApplicationName = builder.Configuration.GetRequiredSection(L10N.API.Common.Constants.ApplicationName).Value;
ConfigValues.CosmosPreferredRegion = builder.Configuration.GetRequiredSection(L10N.API.Common.Constants.CosmosPreferredRegion).Value;
ConfigValues.CosmosOtherRegions = builder.Configuration.GetRequiredSection(L10N.API.Common.Constants.CosmosOtherRegions).Value;
ConfigValues.StorageAccountConnectionString = builder.Configuration.GetRequiredSection(L10N.API.Common.Constants.StorageAccountConnectionString).Value;
//ConfigValues.SAKey = builder.Configuration.GetRequiredSection(L10N.API.Common.Constants.SAKey).Value;

//app.Logger.Log(LogLevel.Information, "The Cosmos URI is " + ConfigValues.CosmosDbServerURI);
//app.Logger.Log(LogLevel.Information, "The primary key is " + ConfigValues.PrimaryReadOnlyKey);
app.Run();
