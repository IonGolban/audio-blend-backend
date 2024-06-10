using AudioBlend.Core.MusicData;
using AudioBlend.Core.MusicData.Repositories.Implementations;
using AudioBlend.Core.MusicData.Services.Interfaces;
using AudioBlend.Core.Repositories.Interfaces;
using AudioBlend.Core.UserAccess.Models.Users;
using AudioBlend.Core.UserAccess.Services.Implementations.Registration;
using AudioBlend.Core.UserAccess.Services.Interfaces.Registration;
using AudioBlend.Core.UserAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AudioBlend.Core.UserAccess.Services.Implementations.Login;
using AudioBlend.Core.UserAccess.Services.Interfaces.Login;
using AudioBlend.API.Services;
using AudioBlend.Core.Streaming.Services.Interfaces;
using AudioBlend.Core.Streaming.Services.Implementations;
using AudioBlend.Core.UserAccess.Services.Interfaces.Users;
using AudioBlend.Core.UserAccess.Services.Implementations.Users;
using AudioBlend.API.Services.Implementations;
using AudioBlend.Core.MusicData.Repositories.Interfaces;
using AudioBlend.Core.MusicData.Services.Implementations;
using Microsoft.OpenApi.Models;
using AudioBlend.API.DbInitializer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });

    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "AudioBlend API"
    });
});

var connectionString = builder.Configuration.GetConnectionString("AudioBlendConnection");
builder.Services.AddDbContext<AudioBlendContext>(options =>
    options.UseNpgsql(connectionString, o => o.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "music_data"))
);
builder.Services.AddDbContext<UserAccessContext>(options =>
    options.UseNpgsql(connectionString, o => o.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "user_access"))
);

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<UserAccessContext>()
    .AddDefaultTokenProviders();

builder.Services.AddIdentityCore<User>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<UserAccessContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"] ?? string.Empty))
    };
});

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<ILoginService, LoginService>();

builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", b =>
{
    b.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
}));

builder.Services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IAlbumRepository, AlbumRepository>();
builder.Services.AddScoped<IAlbumService, AlbumService>();
builder.Services.AddScoped<IAzureBlobStorageService, AzureBlobStorageService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAudioProivderService, AudioProivderService>();
builder.Services.AddScoped<ISongRepository, SongRepository>();
builder.Services.AddScoped<ISongService, SongService>();
builder.Services.AddScoped<IPlaylistRepository, PlaylistRepository>();
builder.Services.AddScoped<IPlaylistService, PlaylistService>();
builder.Services.AddScoped<IPlaylistServiceCommand, PlaylistServiceCommand>();
builder.Services.AddScoped<IPlaylistSongRepository, PlaylistSongRepository>();
builder.Services.AddScoped<IPlaylistSongService, PlaylistSongService>();
builder.Services.AddScoped<IArtistRepository, ArtistRepository>();
builder.Services.AddScoped<IArtistService, ArtistService>();
builder.Services.AddScoped<IFollowArtistRepository, FollowArtistRepository>();
builder.Services.AddScoped<IFollowService, FollowService>();
builder.Services.AddScoped<IRecommendationService, RecommendationService>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<ILikeAlbumRepository, LikeAlbumRepository>();
builder.Services.AddScoped<ILikePlaylistRepository, LikePlaylistRepository>();
builder.Services.AddScoped<ILikeSongRepository, LikeSongRepository>();
builder.Services.AddScoped<ILikeService, LikeService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AudioBlendContext>();
    dbContext.Database.EnsureCreated();
    app.UseSwagger();
    app.UseSwaggerUI();
    // Apply pending migrations
}
//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    var userContext = services.GetRequiredService<AudioBlendContext>();
//    var userAccessContext = services.GetRequiredService<UserAccessContext>();

//    userAccessContext.Database.EnsureCreated();
//    userContext.Database.EnsureCreated();

//    await DbInitMusicData.InitData(services);


//    userContext.SaveChanges();

//}
app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
