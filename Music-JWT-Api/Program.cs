using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Music_JWT_Api.Models;

var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddCors(options => {
        options.AddPolicy("policy", 
        policy => {
            policy
                .AllowAnyOrigin() // Hepsine açar
                // .WithOrigins("https://localhost:7090") // Belirli adrese açar
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    });

    builder.Services.AddDbContext<MusicContext>();
    builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<MusicContext>().AddDefaultTokenProviders();

    builder.Services.Configure<IdentityOptions>(options => {
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireDigit = false;

        options.User.RequireUniqueEmail = true;
        options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 5;
    });

    builder.Services.AddAuthentication(x => {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(x => {
        x.RequireHttpsMetadata = false; // Https zorunlu değil. Http'den talepler de gelebilir eder.
        x.TokenValidationParameters = new TokenValidationParameters // Token'ın validasyonu aşamasında kullanılacak şartlar
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true, // Token'ı validate ediyoruz. GenerateJWT aşamasındaki bilgilerden IssuerSigningKey değerini oluşturuyoruz.
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetSection("AppSettings:Secret").Value ?? "")),
            ValidateLifetime = true // Token'ın süre bilgisi validate edilsin mi
        };
    });

    builder.Services.AddHttpClient();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

    app.UseRouting();

    app.UseAuthentication();

    app.UseCors("policy");

app.UseAuthorization();

app.MapControllers();

app.Run();
