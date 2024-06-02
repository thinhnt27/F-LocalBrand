using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using F_LocalBrand.Middlewares;
using F_LocalBrand.Settings;
using System.Text;
using F_LocalBrand.Mapper;
using F_LocalBrand.Models;
using F_LocalBrand.Repository;
using F_LocalBrand.UnitOfWorks;
using F_LocalBrand.Services;
using F_LocalBrand.Service;
using FluentValidation;
using F_LocalBrand.Validation;
using F_LocalBrand.Dtos;
using F_LocalBrand.Helpers;
using StackExchange.Redis;
using static Org.BouncyCastle.Math.EC.ECCurve;
using F_LocalBrand.Hubs;

namespace F_LocalBrand.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ExceptionMiddleware>();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddSignalR();

        //Add Mapper
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new ApplicationMapper());
        });

        IMapper mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);

        ////Set time for PostgreSQL
        //AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        //var jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
        var secretKey = Environment.GetEnvironmentVariable("SECRET_KEY");
        if (string.IsNullOrEmpty(secretKey))
        {
            throw new InvalidOperationException("JWT Secret Key is not configured.");
        }
        var jwtSettings = new JwtSettings
        {
            Key = secretKey
        };
        services.Configure<JwtSettings>(val =>
        {
            val.Key = jwtSettings.Key;
        });

        //services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));

        //services.Configure<CloundSettings>(configuration.GetSection(nameof(CloundSettings)));

        services.AddAuthorization();

        services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });
        //Get config mail form environment
        services.Configure<MailSettings>(options =>
        {
            options.Server = Environment.GetEnvironmentVariable("MailSettings__Server");
            options.Port = int.Parse(Environment.GetEnvironmentVariable("MailSettings__Port") ?? "0");
            options.SenderName = Environment.GetEnvironmentVariable("MailSettings__SenderName");
            options.SenderEmail = Environment.GetEnvironmentVariable("MailSettings__SenderEmail");
            options.UserName = Environment.GetEnvironmentVariable("MailSettings__UserName");
            options.Password = Environment.GetEnvironmentVariable("MailSettings__Password");
        });

        //Get Connection String config from environment
        var dbServer = Environment.GetEnvironmentVariable("DB_SERVER");
        var dbName = Environment.GetEnvironmentVariable("DB_NAME");
        var dbUser = Environment.GetEnvironmentVariable("DB_USER");
        var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
        var dbTrustServerCertificate = Environment.GetEnvironmentVariable("DB_TRUST_SERVER_CERTIFICATE");
        var dbMultipleActiveResultSets = Environment.GetEnvironmentVariable("DB_MULTIPLE_ACTIVE_RESULT_SETS");

        //Set Connection String
        var connectionString = $"Data Source={dbServer};Initial Catalog={dbName};User ID={dbUser};Password={dbPassword};TrustServerCertificate={dbTrustServerCertificate};MultipleActiveResultSets={dbMultipleActiveResultSets}";

        //Add connection to database
        services.AddDbContext<SWD_FLocalBrandContext>(opt =>
        {
            opt.UseSqlServer(connectionString);
        });

        var redisConnection = new RedisConnection();
        configuration.GetSection("RedisConnection").Bind(redisConnection);

        // Register RedisConfiguration as a singleton
        services.AddSingleton(redisConnection);

        // Configure Redis connection
        services.AddSingleton<IConnectionMultiplexer>(option =>
           ConnectionMultiplexer.Connect(new ConfigurationOptions
           {

               EndPoints = { $"{redisConnection.Host}:{redisConnection.Port}" },
               //Ssl = redisConnection.IsSSL,
               //Password = redisConnection.Password


           }));
        services.AddSingleton<MessageHub>();

        // Add StackExchangeRedisCache as the IDistributedCache implementation
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = $"{redisConnection.Host}:{redisConnection.Port}";
        });

        // Register ResponseCacheService
        services.AddSingleton<IResponseCacheService, ResponseCacheService>();

        //add repositories
        services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddTransient<IUserRepository, UserRepository>();

        //Add UnitOfWork
        services.AddTransient<IUnitOfWork, UnitOfWork>();

        //Add Services
        services.AddScoped<IdentityService>();
        services.AddScoped<UserService>();
        services.AddScoped<JwtSettings>();
        services.AddScoped<EmailService>();

        //Add Validation
        services.AddScoped<IValidator<UserModel>, UserValidation>();


        //services.AddScoped(typeof(IRepository<,>), typeof(GenericRepository<,>));
        //services.AddScoped<DatabaseInitialiser>();
        //services.AddScoped<IdentityService>();
        //services.AddScoped<UserService>();
        //services.AddScoped<UserRoleService>();
        //services.AddScoped<PermissionService>();
        //services.AddScoped<SyllabusService>();
        //services.AddScoped<OutlineMaterialsServices>();
        //services.AddScoped<SyllabusOutlineLearningObjServices>();
        //services.AddScoped<SyllabusOutlineUnitServices>();
        //services.AddScoped<EmailService>();
        //services.AddScoped<CloudService>();
        //services.AddScoped<TrainingProgramServices>();
        //services.AddScoped<ManaService>();
        //services.AddScoped<ClassService>();
        //services.AddScoped<CreateFullSyllabusService>();
        //services.AddScoped<AssessmentSchemeService>();
        //services.AddScoped<OutputStandardService>();
        //services.AddScoped<ViewTrainingCalendarService>();
        //services.AddScoped<RoomService>();
        //services.AddScoped<EnrollmentService>();
        //services.AddScoped<SemesterService>();

        return services;
    }
}