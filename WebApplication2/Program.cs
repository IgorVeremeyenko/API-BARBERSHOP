using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApplication2.Models;
using WebApplication2.Services;
using WebApplication2.Services.Cache;
using WebApplication2.Services.Costumers;
using WebApplication2.ServicesList.Admins.Controllers.Delete;
using WebApplication2.ServicesList.Admins.Controllers.Get;
using WebApplication2.ServicesList.Admins.Controllers.Post;
using WebApplication2.ServicesList.Admins.Controllers.Put;
using WebApplication2.ServicesList.Appointments.Controllers.Delete;
using WebApplication2.ServicesList.Appointments.Controllers.Get;
using WebApplication2.ServicesList.Appointments.Controllers.Post;
using WebApplication2.ServicesList.Appointments.Controllers.Put;
using WebApplication2.ServicesList.Auth.Controllers.Get;
using WebApplication2.ServicesList.Auth.Controllers.Get.Validate;
using WebApplication2.ServicesList.Auth.Controllers.Post;
using WebApplication2.ServicesList.Auth.Controllers.Post.Login;
using WebApplication2.ServicesList.Auth.Controllers.Post.Registration;
using WebApplication2.ServicesList.Auth.Controllers.Post.SendCode;
using WebApplication2.ServicesList.Auth.Controllers.Post.ValidateOTP;
using WebApplication2.ServicesList.Costumers.Controllers.Delete;
using WebApplication2.ServicesList.Costumers.Controllers.Get;
using WebApplication2.ServicesList.Costumers.Controllers.Post;
using WebApplication2.ServicesList.Costumers.Controllers.Put;
using WebApplication2.ServicesList.Masters.Controllers.Delete;
using WebApplication2.ServicesList.Masters.Controllers.Get;
using WebApplication2.ServicesList.Masters.Controllers.Post;
using WebApplication2.ServicesList.Masters.Controllers.Put;
using WebApplication2.ServicesList.MasterSchedules.Controllers.Delete;
using WebApplication2.ServicesList.MasterSchedules.Controllers.Get;
using WebApplication2.ServicesList.MasterSchedules.Controllers.Post;
using WebApplication2.ServicesList.MasterSchedules.Controllers.Put;
using WebApplication2.ServicesList.Service.Controllers.Delete;
using WebApplication2.ServicesList.Service.Controllers.Get;
using WebApplication2.ServicesList.Service.Controllers.Post;
using WebApplication2.ServicesList.Service.Controllers.Put;
using WebApplication2.ServicesList.Statistics.Controllers.Delete;
using WebApplication2.ServicesList.Statistics.Controllers.Get;
using WebApplication2.ServicesList.Statistics.Controllers.Post;
using WebApplication2.ServicesList.Statistics.Controllers.Put;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options => {
    options.AddPolicy(name: MyAllowSpecificOrigins,
        builder => {
            builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options => 
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
    );
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<CacheService>();
builder.Services.AddScoped<CalculateRating>();
builder.Services.AddScoped<GeneratingSaltForPasswordHashing>();
//Auth start
builder.Services.AddScoped<RegistrationService>();
builder.Services.AddScoped<ValidateTokenService>();
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<SendCodeService>();
builder.Services.AddScoped<ValidateOtpService>();
builder.Services.AddScoped<CheckUser>();
//Auth end
builder.Services.AddScoped<HashingPassword>();
//Admin start
builder.Services.AddScoped<GetByIdService>();
builder.Services.AddScoped<GetListService>();
builder.Services.AddScoped<PutByIdService>();
builder.Services.AddScoped<PostAdminService>();
builder.Services.AddScoped<DeleteAdminService>();
//Admin end
//Appointments start
builder.Services.AddScoped<GetAppListService>();
builder.Services.AddScoped<GetByAdminIdService>();
builder.Services.AddScoped<GetListByMasterIdService>();
builder.Services.AddScoped<PostAppService>();
builder.Services.AddScoped<PutAppService>();
builder.Services.AddScoped<DeleteAppService>();
//Appointments end
//Costumers start
builder.Services.AddScoped<GetCostByIdService>();
builder.Services.AddScoped<GetCostListService>();
builder.Services.AddScoped<PostCostService>();
builder.Services.AddScoped<PutCostByIdService>();
builder.Services.AddScoped<DeleteCostService>();
//Costumers end
//Masters start
builder.Services.AddScoped<GetMasterByIdService>();
builder.Services.AddScoped<GetMasterByNameService>();
builder.Services.AddScoped<GetMasterListByIdService>();
builder.Services.AddScoped<GetMasterListService>();
builder.Services.AddScoped<GetMasterSpecialListService>();
builder.Services.AddScoped<PostMasterService>();
builder.Services.AddScoped<PutMasterService>();
builder.Services.AddScoped<DeleteMasterService>();
//Masters end
//Schedules start
builder.Services.AddScoped<GetShedulesByIdService>();
builder.Services.AddScoped<GetShedulesListService>();
builder.Services.AddScoped<PostShedulesService>();
builder.Services.AddScoped<PutShedulesService>();
builder.Services.AddScoped<DeleteShedulesService>();
//Schedules end
//Services start
builder.Services.AddScoped<GetAppFilterService>();
builder.Services.AddScoped<GetGroupListService>();
builder.Services.AddScoped<GetListByNameService>();
builder.Services.AddScoped<GetListForMasterEditService>();
builder.Services.AddScoped<GetServByIdService>();
builder.Services.AddScoped<GetTreeNodeService>();
builder.Services.AddScoped<GetServListService>();
builder.Services.AddScoped<PostServService>();
builder.Services.AddScoped<PutServService>();
builder.Services.AddScoped<DeleteServService>();
//Services end
//Statistics start
builder.Services.AddScoped<GetStatByIdService>();
builder.Services.AddScoped<GetStatListService>();
builder.Services.AddScoped<PostStatService>();
builder.Services.AddScoped<DeleteStatService>();
builder.Services.AddScoped<PutStatService>();
//Statistics end
builder.Services.AddDbContext<MyDatabaseContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options => {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();

app.Run();
