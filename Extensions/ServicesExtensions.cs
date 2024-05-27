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

namespace F_LocalBrand.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ExceptionMiddleware>();
        services.AddControllers();
        //services.AddFluentValidation(fv =>
        //{
        //    fv.RegisterValidatorsFromAssemblyContaining<UserValidation>();
        //});
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

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

        var dbServer = Environment.GetEnvironmentVariable("DB_SERVER");
        var dbName = Environment.GetEnvironmentVariable("DB_NAME");
        var dbUser = Environment.GetEnvironmentVariable("DB_USER");
        var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
        var dbTrustServerCertificate = Environment.GetEnvironmentVariable("DB_TRUST_SERVER_CERTIFICATE");
        var dbMultipleActiveResultSets = Environment.GetEnvironmentVariable("DB_MULTIPLE_ACTIVE_RESULT_SETS");

        var connectionString = $"Data Source={dbServer};Initial Catalog={dbName};User ID={dbUser};Password={dbPassword};TrustServerCertificate={dbTrustServerCertificate};MultipleActiveResultSets={dbMultipleActiveResultSets}";

        services.AddDbContext<SWD_FLocalBrandContext>(opt =>
        {
            opt.UseSqlServer(connectionString);
        });
        services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IdentityService>();
        services.AddScoped<UserService>();
        services.AddScoped<JwtSettings>();
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