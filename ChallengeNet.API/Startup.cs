using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChallengeNet.Core.Core.Workers;
using ChallengeNet.Core.Handlers;
using ChallengeNet.Core.Handlers.GenerateXmlStrategy;
using ChallengeNet.Core.Handlers.TaxCalculateStrategy;
using ChallengeNet.Core.Handlers.TaxCalculateStrategy.Strategies;
using ChallengeNet.Core.Infrastructure;
using ChallengeNet.Core.Interfaces;
using ChallengeNet.Core.Models;
using ChallengeNet.Core.Models.Register;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace ChallengeNet.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Controllers and Heath Check

            services.AddControllers();

            services.AddHealthChecks();

            #endregion

            #region Swagger

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "Challenge.API", Version = "v1" });

                x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization Header",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                });

                x.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        Array.Empty<string>()
                    }
                });
            });

            #endregion

            #region Authentication

            var key = Encoding.ASCII.GetBytes(Configuration[Consts.SecretKey]);
            var validIssuer = Configuration[Consts.Issuer];
            var validAudience = Configuration[Consts.Audience];

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = validIssuer,
                    ValidAudience = validAudience
                };
            });

            #endregion

            #region Workers, HostedWorkers, Services, Repositories and Handlers

            services.AddSingleton<IAuthenticationWorker, AuthenticationWorker>();
            services.AddSingleton<IUserTokenHandler, UserTokenHandler>();
            services.AddSingleton<IUserRepository, UserRepository>();

            services.AddSingleton<IRegisterPessoaFisicaWorker, RegisterPessoaFisicaWorker>();
            services.AddSingleton<IRegisterPessoaJuridicaWorker, RegisterPessoaJuridicaWorker>();
            services.AddSingleton<IPessoaRepository<PessoaFisica>, PessoaFisicaRepository>();
            services.AddSingleton<IPessoaRepository<PessoaJuridica>, PessoaJuridicaRepository>();

            services.AddSingleton<IGenerateXmlHandler, GenerateXmlHandler>();

            services.AddSingleton<Func<ProductType, ITaxCalculateStrategy>>((productType) =>
            {
                return productType switch
                {
                    ProductType.Nfe => new TaxCalculateNfeStrategy(),
                    ProductType.Nfce => new TaxCalculateNfceStrategy(),
                    _ => throw new ArgumentException($"{nameof(ITaxCalculateStrategy)} not found"),
                };
            });

            services.AddSingleton<ITaxCalculateWithFuncHandler, TaxCalculateWithFuncHandler>();

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(x =>
                {
                    x.SwaggerEndpoint("/swagger/v1/swagger.json", "Challenge.API v1");
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseHealthChecks("/healthcheck");
        }
    }
}