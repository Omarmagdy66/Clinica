
using Data;
using Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using DAL;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.Extensions.Options;


namespace Cls.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var con = builder.Configuration.GetConnectionString("con");
            builder.Services.AddDbContext<clinicdbContext>(options => options.UseSqlServer(con));
            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

            //            builder.Services.AddAuthentication(options =>
            //            {
            //                options.DefaultAuthenticateScheme = "Cookies";  // Default to Cookies for authentication
            //                options.DefaultChallengeScheme = "Cookies";     // Default challenge scheme
            //                options.DefaultSignInScheme = "Cookies";        // Default sign-in scheme
            //            })
            //.AddCookie("Cookies", options =>
            //{
            //    options.LoginPath = "/User/Login";  // Redirect to login page if not authenticated
            //    options.LogoutPath = "/User/Logout";
            //})
            //.AddJwtBearer("Bearer", options =>
            //{
            //    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateLifetime = true,
            //        ValidateIssuerSigningKey = true,
            //        ValidIssuer = builder.Configuration["Jwt:Issuer"],
            //        ValidAudience = builder.Configuration["Jwt:Audience"],
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            //    };
            //});
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
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
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

                // Define the security scheme
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT token",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // Or the authentication scheme you are using, e.g., "bearer"
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                c.AddSecurityDefinition("Bearer", securityScheme);

                // Make sure Swagger UI requires a JWT token to be passed
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
            new string[] { }
        }
    });
            });

            builder.Services.AddAuthorization();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });

            });
            var app = builder.Build();
            app.UseCors("MyPolicy");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

//using Data;
//using Interfaces;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.EntityFrameworkCore;
//using DAL;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.OpenApi.Models;
//using System.Text;

//namespace Cls.Api
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            var builder = WebApplication.CreateBuilder(args);
//            var con = builder.Configuration.GetConnectionString("con");

//            // Modify the AddDbContext call to include EnableRetryOnFailure
//            builder.Services.AddDbContext<clinicdbContext>(options =>
//    options.UseSqlServer(con, sqlOptions =>
//        sqlOptions.EnableRetryOnFailure(
//            maxRetryCount: 10, // Increase the number of retry attempts
//            maxRetryDelay: TimeSpan.FromSeconds(30),
//            errorNumbersToAdd: null
//        )));

//            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

//            builder.Services.AddAuthentication(options =>
//            {
//                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//            }).AddJwtBearer(options =>
//            {
//                options.SaveToken = true;
//                options.TokenValidationParameters = new TokenValidationParameters()
//                {
//                    ValidateIssuer = true,
//                    ValidateAudience = true,
//                    ValidateLifetime = true,
//                    ValidateIssuerSigningKey = true,
//                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
//                    ValidAudience = builder.Configuration["Jwt:Audience"],
//                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
//                };
//            });

//            builder.Services.AddControllers();
//            builder.Services.AddEndpointsApiExplorer();
//            builder.Services.AddSwaggerGen(c =>
//            {
//                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

//                // Define the security scheme
//                var securityScheme = new OpenApiSecurityScheme
//                {
//                    Name = "JWT Authentication",
//                    Description = "Enter JWT token",
//                    In = ParameterLocation.Header,
//                    Type = SecuritySchemeType.Http,
//                    Scheme = "bearer",
//                    BearerFormat = "JWT",
//                    Reference = new OpenApiReference
//                    {
//                        Type = ReferenceType.SecurityScheme,
//                        Id = "Bearer"
//                    }
//                };

//                c.AddSecurityDefinition("Bearer", securityScheme);

//                // Make sure Swagger UI requires a JWT token to be passed
//                c.AddSecurityRequirement(new OpenApiSecurityRequirement
//                {
//                    {
//                        new OpenApiSecurityScheme
//                        {
//                            Reference = new OpenApiReference
//                            {
//                                Type = ReferenceType.SecurityScheme,
//                                Id = "Bearer"
//                            }
//                        },
//                        new string[] { }
//                    }
//                });
//            });

//            builder.Services.AddAuthorization();
//            var app = builder.Build();

//            // Configure the HTTP request pipeline.
//            if (app.Environment.IsDevelopment())
//            {
//                app.UseSwagger();
//                app.UseSwaggerUI();
//            }

//            app.UseHttpsRedirection();
//            app.UseAuthentication();
//            app.UseAuthorization();

//            app.MapControllers();
//            app.Run();
//        }
//    }
//}

