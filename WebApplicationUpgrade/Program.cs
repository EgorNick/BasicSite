using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplicationUpgrade.Data;
using WebApplicationUpgrade.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // 
            ValidateAudience = true, //
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
        
    });

builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddSingleton<CloudinaryService>();
builder.Services.AddScoped<ISavingInfo, SavingInfo>();
builder.Services.AddScoped<IJwtAuthenticationManager, JwtAuthenticationManager>();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

if (!builder.Environment.IsProduction())
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1",
            new OpenApiInfo { Title = "RR.OpenAi", Version = "v1" });

        var securityScheme = new OpenApiSecurityScheme
        {
            Name = "RR.OpenAi",
            Description = "Enter JWT Bearer token (w/o Bearer)",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Reference = new OpenApiReference
            {
                Id = JwtBearerDefaults.AuthenticationScheme,
                Type = ReferenceType.SecurityScheme
            }
        };

        c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            { securityScheme, new string[] { } }
        });

        var currentAssembly = Assembly.GetExecutingAssembly();
        var xmlDocs = AppDomain.CurrentDomain.GetAssemblies()
            .Where(x => /*x.FullName.StartsWith("RR") || */x.FullName == currentAssembly.FullName)
            .Select(a =>
                Path.Combine(Path.GetDirectoryName(currentAssembly.Location) ?? string.Empty, $"{a.GetName().Name}.xml"))
            .Where(File.Exists)
            .ToArray();

        foreach (var xmlDoc in xmlDocs)
            c.IncludeXmlComments(xmlDoc, true);
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
    
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("./v1/swagger.json", "RR.OpenAi API v1");
        c.DocumentTitle = "RR.OpenAi";
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();