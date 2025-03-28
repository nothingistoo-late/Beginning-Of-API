﻿using Final4.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Final4.Service.Email;
using Quartz;
using Final4.IRepository;
using Final4.Repository;
using Final4.Injection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policy => policy.WithOrigins("http://127.0.0.1:5500") // Hoặc "http://localhost:3000" nếu chạy React/Vue
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
});

builder.Services.AddScoped<IAccountRepository, AccountRepository>();

// khai báo dịch vụ Quartz 
builder.Services.AddQuartz();

builder.Services.AddHostedService<EmailReminderService>();

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

builder.Logging.AddConsole(); // Thêm logging vào Console

// Đăng ký EmailQueue
builder.Services.AddSingleton<EmailQueue>();

// Add services to the container.
builder.Services.AddControllers();

builder.Services.ServicesInjection(builder.Configuration);


// Cấu hình Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
var jwtConfig = builder.Configuration.GetSection("JwtConfig");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;  // Chỉ dùng trong phát triển, bỏ khi triển khai thực tế
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtConfig["Issuer"],  // Lấy Issuer từ appsettings.json
        ValidateAudience = true,
        ValidAudience = jwtConfig["Audience"], // Lấy Audience từ appsettings.json
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["Key"])) // Lấy Key từ appsettings.json
    };
});

//fe
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
//fe
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Cấu hình Bearer token cho Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter Bearer token as 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.AddAuthorization(options =>
{
    // Tạo policy cho từng role
    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("UserPolicy", policy =>
        policy.RequireRole("User"));

    options.AddPolicy("SellerPolicy", policy =>
        policy.RequireRole("Seller"));

    options.AddPolicy("StaffPolicy", policy =>
        policy.RequireRole("Staff"));
});
builder.Services.AddSingleton<EmailService>();
//builder.Services.AddControllers()
//    .AddJsonOptions(options =>
//    {
//        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
//    });

var app = builder.Build();

app.UseCors("AllowLocalhost"); // Áp dụng CORS

app.UseHttpsRedirection(); // Đảm bảo chuyển hướng HTTP sang HTTPS
app.UseAuthentication();   // Kích hoạt authentication
app.UseAuthorization();    // Kích hoạt authorization

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
