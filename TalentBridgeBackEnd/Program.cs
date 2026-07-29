using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using TalentBridgeBackEnd.Data;
using TalentBridgeBackEnd.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger with JWT Bearer Authentication & full parameter display
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TalentBridge Candidate Access Platform API",
        Version = "v1",
        Description = "API for Candidate Access Platform supporting Dual-Projection security, 2-hour JWT access tokens, and refresh tokens."
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT Bearer token. Example: `Bearer <your-token>`"
    });

    options.AddSecurityRequirement((doc) => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer"),
            new List<string>()
        }
    });
});

// Register Application Services
builder.Services.AddScoped<ReferenceCodeGenerator>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<GrantResolverService>();
builder.Services.AddScoped<PreviewProjectionService>();
builder.Services.AddScoped<FullProjectionService>();
builder.Services.AddScoped<MaskingEngine>();
builder.Services.AddScoped<ModerationService>();
builder.Services.AddScoped<BatchCurationService>();
builder.Services.AddScoped<CompanyService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<AccessEventService>();
builder.Services.AddScoped<AuditService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<OutcomeService>();
builder.Services.AddScoped<FollowUpService>();
builder.Services.AddScoped<DashboardService>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins("http://localhost:5173", "http://localhost:5174")
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

// Configure Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Configure Authentication & Authorization
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? "TalentBridge-Super-Secret-Key-2026-Must-Be-At-Least-32-Characters-Long!";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"] ?? "TalentBridge",
            ValidAudience = jwtSettings["Audience"] ?? "TalentBridgeUsers",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SuperAdminOnly", policy => policy.RequireRole("SuperAdmin"));
    options.AddPolicy("AdminAccess", policy => policy.RequireRole("SuperAdmin", "OpsAdmin"));
    options.AddPolicy("CompanyAccess", policy => policy.RequireRole("CompanyUser"));
    options.AddPolicy("CandidateAccess", policy => policy.RequireRole("Candidate"));
});

var app = builder.Build();

// Enable Swagger UI unconditionally
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TalentBridge API v1");
    c.RoutePrefix = "swagger";
    c.DisplayRequestDuration();
});

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

// Initialize database & seed initial data on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<AppDbContext>();
        await DbInitializer.InitializeAsync(dbContext, connectionString);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while initializing the database.");
    }
}

app.MapControllers();

app.Run();
