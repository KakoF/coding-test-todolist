﻿using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Settings;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Service.Services;

namespace WebApi.Extensions
{
	public static class BuildExtension
	{

		public static void AddConfiguration(this WebApplicationBuilder builder)
		{
			builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

		}

		public static void AddAuthentication(this WebApplicationBuilder builder)
		{
			/*builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			});*/
			/*builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options => { options.TokenValidationParameters = new TokenValidationParameters { ValidateIssuer = true, ValidateAudience = true, ValidateLifetime = true, ValidateIssuerSigningKey = true, ValidIssuer = jwtSettings.Issuer, ValidAudience = jwtSettings.Audience, IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)) }; 
			});*/
		}
		public static void AddDocumentation(this WebApplicationBuilder builder)
		{
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					In = ParameterLocation.Header,
					Description = "Por favor insira 'Bearer' [espaço] e depois seu token JWT",
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer"
				});
				c.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme, Id = "Bearer" }
						},
						new string[] { }
					} 
				});
			});

		}

		public static void AddDataContexts(this WebApplicationBuilder builder)
		{
			builder
				.Services
				.AddDbContext<AppDataContext>(
					x =>
					{
						x.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"),
							sqlOptions => sqlOptions.MigrationsAssembly("Infrastructure")).ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));

					});

		}


		public static void AddServices(this WebApplicationBuilder builder)
		{
			builder.Services.AddHttpContextAccessor();
			builder.Services.AddScoped<IUserService, UserService>();
			builder.Services.AddScoped<IToDoService, TodoService>();
			builder.Services.AddScoped<ITokenService, TokenService>();
			builder.Services.AddScoped<IUserRepository, UserRepository>();
			builder.Services.AddScoped<IToDoRepository, ToDoRepository>();
			builder.Services.AddScoped<IEncryptService, EncryptService>();
		}
	}
}