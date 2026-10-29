
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Application.Services;
using LibraryManagement.Infrastructure.Repositories;
using LibraryManagement.Swagger;
using LibraryManagement.Application.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi;
using LibraryManagement.Auth;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    var tenant = builder.Configuration["AzureAd:TenantId"];
    var scope = $"api://{builder.Configuration["AzureAd:ClientId"]}/access_as_user";

    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri($"https://login.microsoftonline.com/{tenant}/oauth2/v2.0/authorize"),
                TokenUrl = new Uri($"https://login.microsoftonline.com/{tenant}/oauth2/v2.0/token"),
                Scopes = new Dictionary<string, string> { { scope, "Access the Api" } }
            }
        }
    });
    options.DocumentFilter<AuthorizeCheckDocumentFilter>(scope);
});

builder.Services.Configure<LibrarySettings>(builder.Configuration.GetSection("Library"));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddAuthorization();
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("LibraryManagement.Infrastructure"))); builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IAuthenticationService, EntraAuthenticationService>();
builder.Services.AddScoped<IAuthorRepo, AuthorRepository>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IBookRepo, BookRepository>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<ICategoryRepo, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ILibraryMemberRepo, LibraryMemberRepository>();
builder.Services.AddScoped<ILibraryMemberService, LibraryMemberService>();
builder.Services.AddScoped<IBorrowRecordRepo, BorrowRecordRepository>();
builder.Services.AddScoped<IBorrowRecordService, BorrowService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.OAuthClientId(builder.Configuration["AzureAd:ClientId"]);
        options.OAuthUsePkce();
    });
}

app.UseHttpsRedirection();
app.UseMiddleware<LibraryManagement.MiddleWare.ExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
