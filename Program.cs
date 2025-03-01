using flow_view.Auth;
using flow_view.Ratings;
using flow_view_database.ApplicationDbContext;
using flow_view_database.ApplicationRole;
using flow_view_database.ApplicationUser;
using flow_view_database.Rating;
using flow_view_database.RefreshToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var flowViewWeb = "flowViewWeb";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: flowViewWeb,
        policy =>
        {
            policy.WithOrigins("http://localhost:3000");
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
            policy.AllowCredentials();
        });
});

builder.Services.AddAuthorizationBuilder();
builder.Services.AddJWTAuthentication(builder.Configuration);

builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.Password.RequiredLength = 7;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;
})
    .AddRoles<ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddApiEndpoints();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("SQLServerIdentityConnection")
    ?? throw new InvalidOperationException("Connection string 'SQLServerIdentityConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly("flow-view")));

builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IAuthApiHelper, AuthApiHelper>();

builder.Services.AddScoped<IRatingRepository, RatingRepository>();
builder.Services.AddScoped<IRatingHelper, RatingHelper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference(options =>
    {
        options.DarkMode = true;
        options.HideModels = true;
    });
    app.MapOpenApi();
}

app.UseCors(flowViewWeb);

app.UseAuthentication();

app.UseAuthorization();

app.UseHttpsRedirection();

app.MapAuthEndpoints();

app.MapRatingEndpoints();

app.Run();
