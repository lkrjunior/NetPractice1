using System;
using System.Collections.Generic;
using ChallengeNet.API.Controllers;
using ChallengeNet.API.Extensions;
using ChallengeNet.Core.Core.Workers;
using ChallengeNet.Core.Handlers;
using ChallengeNet.Core.Handlers.GenerateXmlStrategy;
using ChallengeNet.Core.Handlers.TaxCalculateStrategy;
using ChallengeNet.Core.Handlers.TaxCalculateStrategy.Strategies;
using ChallengeNet.Core.Infrastructure;
using ChallengeNet.Core.Interfaces;
using ChallengeNet.Core.Models;
using ChallengeNet.Core.Models.Register;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            #region Controllers, HttpClient and Heath Check

            services.AddControllers();

            services.AddHealthChecks();

            services.AddHttpClient(Consts.NyTimesHttpClient, httpClient =>
            {
                var baseAddress = Configuration[$"{Consts.NyTimesApiSection}:{Consts.NyTimesApiBaseAddress}"];

                httpClient.BaseAddress = new Uri(baseAddress);
            });

            #endregion

            #region Swagger

            services.ConfigureSwagger();

            #endregion

            #region Authentication

            services.ConfigureAuthentication(Configuration);

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

            services.AddSingleton<ITaxCalculateStrategy, TaxCalculateNfceStrategy>();
            services.AddSingleton<ITaxCalculateStrategy, TaxCalculateNfeStrategy>();

            services.AddSingleton<ITaxCalculateWithFuncHandler, TaxCalculateWithFuncHandler>();

            services.AddSingleton<INyTimesWorker, NyTimesWorker>();

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