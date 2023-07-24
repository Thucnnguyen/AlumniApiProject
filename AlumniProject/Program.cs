using AlumniProject.Data;
using AlumniProject.Data.Repostitory;
using AlumniProject.Data.Repostitory.RepositoryImp;
using AlumniProject.Dto;
using AlumniProject.Service;
using AlumniProject.Service.ServiceImp;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContextPool<AlumniDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("userDb"))
);
builder.Services.AddTransient<RedisService>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    return new RedisService(configuration);
    });
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddTransient<IAlumniService, AlumniService>();
builder.Services.AddTransient<ICardItem, CardItemRepo>();
builder.Services.AddTransient<ISchoolService, SchoolService>();
builder.Services.AddTransient<IClassService, AlumniClassService>();
builder.Services.AddTransient<IGradeService, GradeService>();
builder.Services.AddTransient<IMajorService, MajorService>();
builder.Services.AddTransient<IAlumniToClassService, AlumniToClassService>();
builder.Services.AddTransient<IEducationService, EducationService>();
builder.Services.AddTransient<IEventParticipantService, EventParticipantService>();
builder.Services.AddTransient<IEventService, EventService>();
builder.Services.AddTransient<IFundService, fundService>();
builder.Services.AddTransient<INewsService, NewsService>();
builder.Services.AddTransient<INewsTageNewsService, NewsTageNewsService>();
builder.Services.AddTransient<ITagNewsService, TagNewsService>();
builder.Services.AddTransient<IPostService, PostService>();
builder.Services.AddTransient<IAlumniRequestService, AccessRequestService>();
builder.Services.AddTransient<IRoleService, RoleService>();


builder.Services.AddTransient<IAlumniClassRepo, AlumniClassRepo>();
builder.Services.AddTransient<IAttributeRepo, AttributeRepo >();
builder.Services.AddTransient<IAlumniRepo, AlumniRepo>();
builder.Services.AddTransient<ISchoolRepo, SchoolRepo>();
builder.Services.AddTransient<IGradeRepo, GradeRepo>();
builder.Services.AddTransient<IMajorRepo, MajorRepo>();
builder.Services.AddTransient<IAlumniToClassRepo, AlumniToClassRepo>();
builder.Services.AddTransient<IEducationRepo, EducationRepo>();
builder.Services.AddTransient<IEventParticipantRepo, EventParticipantRepo>();
builder.Services.AddTransient<IEventRepo, EventRepo>();
builder.Services.AddTransient<IFundRepo, FundRepo>();
builder.Services.AddTransient<INewRepo, NewRepo>();
builder.Services.AddTransient<INewsTagNewRepo, NewsTagNewRepo>();
builder.Services.AddTransient<ITagnewRepo, TagNewRepo>();
builder.Services.AddTransient<IPostRepo, PostRepo>();
builder.Services.AddTransient<IAccessRequestRepo, AccessRequestRepo>();
builder.Services.AddTransient<IRoleRepo, RoleRepo>();

builder.Services.AddTransient<Lazy<IClassService>>(provider => new Lazy<IClassService>(provider.GetRequiredService<IClassService>));
builder.Services.AddTransient<Lazy<ISchoolService>>(provider => new Lazy<ISchoolService>(provider.GetRequiredService<ISchoolService>));
builder.Services.AddTransient<Lazy<IAlumniService>>(provider => new Lazy<IAlumniService>(provider.GetRequiredService<IAlumniService>));
builder.Services.AddTransient<Lazy<IGradeService>>(provider => new Lazy<IGradeService>(provider.GetRequiredService<IGradeService>));
builder.Services.AddTransient<Lazy<IMajorService>>(provider => new Lazy<IMajorService>(provider.GetRequiredService<IMajorService>));
builder.Services.AddTransient<Lazy<IAlumniToClassService>>(provider => new Lazy<IAlumniToClassService>(provider.GetRequiredService<IAlumniToClassService>));
builder.Services.AddTransient<Lazy<IEducationService>>(provider => new Lazy<IEducationService>(provider.GetRequiredService<IEducationService>));
builder.Services.AddTransient<Lazy<IEventParticipantService>>(provider => new Lazy<IEventParticipantService>(provider.GetRequiredService<IEventParticipantService>));
builder.Services.AddTransient<Lazy<IEventService>>(provider => new Lazy<IEventService>(provider.GetRequiredService<IEventService>));
builder.Services.AddTransient<Lazy<IFundService>>(provider => new Lazy<IFundService>(provider.GetRequiredService<IFundService>));
builder.Services.AddTransient<Lazy<INewsTageNewsService>>(provider => new Lazy<INewsTageNewsService>(provider.GetRequiredService<INewsTageNewsService>));
builder.Services.AddTransient<Lazy<ITagNewsService>>(provider => new Lazy<ITagNewsService>(provider.GetRequiredService<ITagNewsService>));
builder.Services.AddTransient<Lazy<IPostService>>(provider => new Lazy<IPostService>(provider.GetRequiredService<IPostService>));
builder.Services.AddTransient<Lazy<IAlumniRequestService>>(provider => new Lazy<IAlumniRequestService>(provider.GetRequiredService<IAlumniRequestService>));
builder.Services.AddTransient<Lazy<IRoleService>>(provider => new Lazy<IRoleService>(provider.GetRequiredService<IRoleService>));
builder.Services.AddCors();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
             .GetBytes(builder.Configuration.GetSection("Token:secret").Value)),
            ValidateIssuer = false,
            ValidateAudience = false,

        };
    });
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (example: bearer {Token})",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();

});


var app = builder.Build();

FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile("alumniproject-c8ba6-firebase-adminsdk-134w2-ac692403f8.json")
});

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors(builder =>
{
    builder.AllowAnyHeader();
    builder.AllowAnyMethod();
    builder.AllowAnyOrigin();
});
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
